using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Config.Commands
{
    public class UpdateConfigurationCommand : IRequest<bool>
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }

    public class UpdateConfigurationCommandHandle : IRequestHandler<UpdateConfigurationCommand, bool>
    {
        private readonly ISmtpConfigurationService smtpService;

        public UpdateConfigurationCommandHandle(ISmtpConfigurationService smtpService)
        {
            this.smtpService = smtpService;
        }
        public async Task<bool> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
        {
            await smtpService.UpdateConfiguration(new SmtpConfiguration
            {
                Host = request.Host,
                From = request.From,
                Password = request.Password,
                Port = request.Port,
                Username = request.Username,
                To = request.To
            });
            return true;
        }
    }
}
