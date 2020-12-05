using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Forms.Commands
{
    public class DeleteFormCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteFormCommand(Guid id)
        {
            this.Id = id;
        }
    }

    public class DeleteFormCommandHandler : IRequestHandler<DeleteFormCommand, bool>
    {
        private readonly IAppDbContext context;

        public DeleteFormCommandHandler(IAppDbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Handle(DeleteFormCommand request, CancellationToken cancellationToken)
        {
            Form form = context.Forms.FirstOrDefault(x => x.Id == request.Id);
            if (form == null)
            {
                throw new NotFoundException($"Form with {request.Id} was not found");
            }
            context.Forms.Remove(form);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
