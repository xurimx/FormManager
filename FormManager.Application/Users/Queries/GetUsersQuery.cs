using FormManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Users.Queries
{
    public class GetUsersQuery : IRequest<List<User>>
    {
    }
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<User>>
    {
        public Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
