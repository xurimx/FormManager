using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public GetUserByIdQuery(string Id)
        {
            this.Id = Id;
        }
        public string Id { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserRepository repository;

        public GetUserByIdQueryHandler(IUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetUserById(request.Id);
        }
    }
}
