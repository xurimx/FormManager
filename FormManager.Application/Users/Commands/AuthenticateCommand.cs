using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Users.Responses;
using FormManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Commands
{
    public class AuthenticateCommand : IRequest<TokenResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, TokenResponse>
    {
        private readonly IAuthenticationService authentication;
        private readonly IUserRepository repository;

        public AuthenticateCommandHandler(IAuthenticationService authentication, IUserRepository repository)
        {
            this.authentication = authentication;
            this.repository = repository;
        }
        public async Task<TokenResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            User user = await repository.FindByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new NotFoundException("Username or password is incorrect.");
            }

            bool result = await authentication.CheckPassword(request.Username, request.Password);

            if (result == false)
            {
                throw new NotFoundException("Username or password is incorrect.");
            }
            return await authentication.CreateToken(user);
        }
    }
}
