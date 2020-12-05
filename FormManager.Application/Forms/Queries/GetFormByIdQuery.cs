using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Forms.Queries
{
    public class GetFormsQuery : IRequest<List<Form>>
    {
    }

    public class GetFormsQueryHandler : IRequestHandler<GetFormsQuery, List<Form>>
    {
        private readonly IAppDbContext context;

        public GetFormsQueryHandler(IAppDbContext context)
        {
            this.context = context;
        }
        public Task<List<Form>> Handle(GetFormsQuery request, CancellationToken cancellationToken)
        {
            return context.Forms.ToListAsync();
        }
    }
}
