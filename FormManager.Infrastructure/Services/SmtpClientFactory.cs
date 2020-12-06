using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class SmtpClientFactory : ISmtpClientFactory
    {
        private readonly ISmtpConfigurationService service;

        public SmtpClientFactory(ISmtpConfigurationService service)
        {
            this.service = service;
        }
        public async Task<SmtpClient> CreateClient()
        {
            SmtpConfiguration smtpConfig = await service.GetConfiguration();
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = smtpConfig.Host;
            smtpClient.Port = int.Parse(smtpConfig.Port);
            smtpClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);
            return smtpClient;
        }
    }
}
