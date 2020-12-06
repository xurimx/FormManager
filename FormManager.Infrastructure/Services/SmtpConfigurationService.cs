using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
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
            PropertyInfo[] properties = typeof(SmtpConfiguration).GetProperties();
            SmtpConfiguration cfg = new SmtpConfiguration();
            try
            {
                var configs = context.Configs.ToList();
                foreach (var prop in properties)
                {
                    Domain.Entities.Config config = configs.Where(x => x.Key == prop.Name).First();
                    cfg.GetType().GetProperty(prop.Name).SetValue(cfg, config.Value);
                }
                return await Task.FromResult(cfg);
            }
            catch (Exception e)
            {
                throw new Exception("Smtp configuration was not initialized! Please set the configuration first.");
            }
        }

        public async Task UpdateConfiguration(SmtpConfiguration configuration)
        {
            PropertyInfo[] props = configuration.GetType().GetProperties();

            foreach (var prop in props)
            {
                Domain.Entities.Config config = context.Configs.Where(x => x.Key == prop.Name).FirstOrDefault();
                if (config == null)
                {
                    config = new Domain.Entities.Config
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
