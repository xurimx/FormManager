using FluentEmail.Core;
using FluentEmail.Core.Models;
using FluentEmail.Smtp;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using FormManager.Domain.Entities;
using RazorLight;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private const string Template = "EmailTemplate.cshtml";
        private readonly IUserRepository repository;
        private readonly ISmtpConfigurationService service;
        private readonly RazorLightEngine _razorLightEngine;

        public EmailSender(IUserRepository repository, 
                           ISmtpConfigurationService service)
        {
            this.repository = repository;
            this.service = service;
            _razorLightEngine = new RazorLightEngineBuilder()
                                .UseEmbeddedResourcesProject(typeof(DependencyInjection))
                                .UseMemoryCachingProvider()
                                .Build();

        }
        public async Task SendEmail(Form from)
        {
            string html = await _razorLightEngine.CompileRenderAsync<object>(Template, from);
            User user = await repository.GetUserById(from.SenderId);
            SmtpConfiguration configuration = await service.GetConfiguration();

            Email.DefaultSender = new SmtpSender(new SmtpClient { 
                Host = configuration.Host,
                Port = int.Parse(configuration.Port),
                Credentials = new NetworkCredential(configuration.Username, configuration.Password)
            });
            SendResponse sendResponse = await Email
                .From(configuration.From)
                .To(configuration.To)                
                .Subject($"New Message from: {user.Username}")
                .Body(html, true)               
                .SendAsync();

            if (!sendResponse.Successful)
            {
                //Todo log
            }
        }
    }
}
