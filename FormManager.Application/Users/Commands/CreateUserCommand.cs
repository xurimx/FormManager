using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Commands
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserRepository repository;

        public CreateUserCommandHandler(IUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User user = await repository.FindByUsernameAsync(request.Username);
            if (user != null)
            {
                throw new UserExistsException($"A User with {request.Username} already exists");
            }
            user = await repository.CreateUserAsync(request.Username, request.Email, request.Password, request.Role);

            return user.Id;            
        }
    }
}
