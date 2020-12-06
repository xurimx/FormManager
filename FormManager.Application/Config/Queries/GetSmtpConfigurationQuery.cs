using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Config.Queries
{
    public class GetSmtpConfigurationQuery : IRequest<SmtpConfiguration>
    {
    }

    public class GetSmtpConfigurationQueryHandler : IRequestHandler<GetSmtpConfigurationQuery, SmtpConfiguration>
    {
        private readonly ISmtpConfigurationService smtpSerive;

        public GetSmtpConfigurationQueryHandler(ISmtpConfigurationService smtpSerive)
        {
            this.smtpSerive = smtpSerive;
        }
        public Task<SmtpConfiguration> Handle(GetSmtpConfigurationQuery request, CancellationToken cancellationToken)
        {
            return smtpSerive.GetConfiguration(); 
        }
    }
}
