using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmail(Form from)
        {
            Console.WriteLine("SendEmail method not implemented");
            //Todo connect to smtp to send email here
            return Task.CompletedTask;
        }
    }
}
