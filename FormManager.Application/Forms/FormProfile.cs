using AutoMapper;
using FormManager.Application.Forms.Commands;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Forms
{
    public class FormProfile : Profile
    {
        public FormProfile()
        {
            CreateMap<CreateFormCommand, Form>();
        }
    }
}
