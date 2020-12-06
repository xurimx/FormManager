using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Common.Interfaces
{
    public interface ISmtpClientFactory
    {
        Task<SmtpClient> CreateClient();
    }
}
