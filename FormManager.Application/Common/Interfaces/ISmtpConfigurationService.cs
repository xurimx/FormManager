using FormManager.Application.Config.ViewModels;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Common.Interfaces
{
    public interface ISmtpConfigurationService
    {
        Task<SmtpConfiguration> GetConfiguration();
        Task UpdateConfiguration(SmtpConfiguration configuration);        
    }
}
