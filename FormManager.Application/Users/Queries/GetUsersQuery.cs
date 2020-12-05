using FormManager.Application.Common.Interfaces;
using FormManager.Application.Common.Models;
using FormManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Queries
{
    public class GetUsersQuery : RequestQuery<Pagination<User>> {}

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Pagination<User>>
    {
        private readonly IUserRepository repository;

        public GetUsersQueryHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Pagination<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await repository.QueryUsersAsync(request);
        }
    }
}
