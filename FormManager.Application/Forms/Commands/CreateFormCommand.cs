using AutoMapper;
using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FormManager.Application.Forms.Commands
{
    public class CreateFormCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Company { get; set; }
        public DateTime Appointment { get; set; }
    }

    public class CreateFormCommandHandler : IRequestHandler<CreateFormCommand, Guid>
    {
        private readonly IAppDbContext context;
        private readonly IEmailSender sender;
        private readonly ICurrentUserService currentUser;

        public CreateFormCommandHandler(
            IAppDbContext context, 
            IEmailSender sender, 
            ICurrentUserService currentUser)
        {
            this.context = context;
            this.sender = sender;
            this.currentUser = currentUser;
        }
        public async Task<Guid> Handle(CreateFormCommand request, CancellationToken cancellationToken)
        {
            Form form = new Form
            {
                Name = request.Name,
                Company = request.Company,
                Email = request.Email,
                Telephone = request.Telephone,
                Appointment = request.Appointment,
                Date = DateTime.Now,
                SenderId = currentUser.UserId
            };
            context.Forms.Add(form);
            await sender.SendEmail(form);
            await context.SaveChangesAsync();
            return form.Id;
        }
    }
}
