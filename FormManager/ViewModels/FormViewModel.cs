using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.ViewModels
{
    public class FormViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Company { get; set; }
        public DateTime Appointment { get; set; }
        public DateTime Date { get; set; }
        public string Sender { get; set; }
        public string SenderId { get; set; }
    }
}
