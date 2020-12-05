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
    public class GetFormByIdQuery : IRequest<Form>
    {
        public GetFormByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
        public Guid Id { get; set; }
    }

    public class GetFormByIdQueryHandler : IRequestHandler<GetFormByIdQuery, Form>
    {
        private readonly IAppDbContext context;

        public GetFormByIdQueryHandler(IAppDbContext context)
        {
            this.context = context;
        }
        public async Task<Form> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Forms.FirstOrDefaultAsync(x => x.Id == request.Id);
        }
    }
}
