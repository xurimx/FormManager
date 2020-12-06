using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IUserRepository repository;
        private readonly ISmtpClientFactory smtpFactory;
        private readonly ISmtpConfigurationService service;

        public EmailSender(IUserRepository repository, 
                           ISmtpClientFactory smtpFactory,
                           ISmtpConfigurationService service)
        {
            this.repository = repository;
            this.smtpFactory = smtpFactory;
            this.service = service;
        }
        public async Task SendEmail(Form from)
        {
            SmtpClient smtpClient = await smtpFactory.CreateClient();
            SmtpConfiguration configuration = await service.GetConfiguration();

            User user = await repository.GetUserById(from.SenderId);
            MailMessage mailMessage = new MailMessage(configuration.From, configuration.To);
            mailMessage.Subject = $"New message from: {user.Username}";
            mailMessage.IsBodyHtml = true;

            string html = "";
            html += $"<h5>Name: {from.Name}</h5></br>";
            html += $"<p>Email: {from.Email}</p></br>";
            html += $"<p>Telephone: {from.Telephone}</p></br>";
            html += $"<p>Company: {from.Company}</p></br>";
            html += $"<p>Appointment: {from.Appointment}</p></br>";
            mailMessage.Body = html;

            smtpClient.Send(mailMessage);
        }
    }
}
