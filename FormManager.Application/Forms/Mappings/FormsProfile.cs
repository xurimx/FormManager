using AutoMapper;
using FormManager.Application.Forms.Mappings.ViewModels;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Forms.Mappings
{
    public class FormsProfile : Profile
    {
        public FormsProfile()
        {
            CreateMap<Form, FormVM>()
                .ForPath(x => x.Sender.Id, cfg => cfg.MapFrom(source => source.SenderId))
                .ForMember(x => x.Appointment, cfg => cfg.MapFrom(source => source.Appointment.ToString("U")))
                .ForMember(x => x.Date, cfg => cfg.MapFrom(source => source.Date.ToString("U")));
        }
    }
}
