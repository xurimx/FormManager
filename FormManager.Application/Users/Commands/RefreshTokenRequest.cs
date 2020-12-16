using FormManager.Application.Common.Interfaces;
using FormManager.Application.Users.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Commands
{
    public class RefreshTokenRequest : IRequest<TokenResponse>
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
    {
        private readonly IAuthenticationService authentication;

        public RefreshTokenRequestHandler(IAuthenticationService authentication)
        {
            this.authentication = authentication;
        }
        public async Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            TokenResponse tokenResponse = await authentication.RefreshToken(request.Token, request.RefreshToken);
            return tokenResponse;
        }
    }
}
