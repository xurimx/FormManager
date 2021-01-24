using AutoMapper;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Forms.Mappings.ViewModels;
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
    public class GetFormByIdQuery : IRequest<FormVM>
    {
        public GetFormByIdQuery(Guid Id)
        {
            this.Id = Id;
        }
        public Guid Id { get; set; }
    }

    public class GetFormByIdQueryHandler : IRequestHandler<GetFormByIdQuery, FormVM>
    {
        private readonly IAppDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;

        public GetFormByIdQueryHandler(IAppDbContext context, IMapper mapper, IUserRepository userRepository)
        {
            this.context = context;
            this.mapper = mapper;
            this.userRepository = userRepository;
        }
        public async Task<FormVM> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
        {
            Form form = await context.Forms.FirstOrDefaultAsync(x => x.Id == request.Id);
            FormVM formVM = mapper.Map<FormVM>(form);
            formVM.Sender.Username = (await userRepository.GetUserById(formVM.Sender.Id))?.Username;
            return formVM;
        }
    }
}
