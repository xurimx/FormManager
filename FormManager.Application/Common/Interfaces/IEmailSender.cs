using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Common.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(Form from);
    }
}
