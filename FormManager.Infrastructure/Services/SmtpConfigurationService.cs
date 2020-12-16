using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    class SmtpConfigurationService : ISmtpConfigurationService
    {
        private readonly IAppDbContext context;

        public SmtpConfigurationService(IAppDbContext context)
        {
            this.context = context;
        }
        public async Task<SmtpConfiguration> GetConfiguration()
        {
            try
            {
                var configs = await context.Configs.ToListAsync();
                SmtpConfiguration cfg = new SmtpConfiguration
                {
                    From = configs.First(x => x.Key == "From").Value,
                    Host = configs.First(x => x.Key == "Host").Value,
                    Password = configs.First(x => x.Key == "Password").Value,
                    Port = configs.First(x => x.Key == "Port").Value,
                    To = configs.First(x => x.Key == "To").Value,
                    Username = configs.First(x => x.Key == "Username").Value
                };
                return cfg;
            }
            catch (Exception e)
            {
                throw new FormMgrException("Smtp configuration was not initialized! Please set the configuration first.", 400);
            }
        }

        public async Task UpdateConfiguration(SmtpConfiguration configuration)
        {
            PropertyInfo[] props = configuration.GetType().GetProperties();
            foreach (var prop in props)
            {
                Config config = context.Configs.Where(x => x.Key == prop.Name).FirstOrDefault();
                if (config == null)
                {
                    config = new Config
                    {
                        Key = prop.Name,
                        Value = (string)prop.GetValue(configuration)
                    };
                    context.Configs.Add(config);
                }
                else
                {
                    config.Value = (string)prop.GetValue(configuration);
                    context.Configs.Update(config);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
