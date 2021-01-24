using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Forms.Mappings.ViewModels
{
    public class FormVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Company { get; set; }
        public string Appointment { get; set; }
        public string Date { get; set; }
        public Sender Sender { get; set; }
    }

    public class Sender
    {
        public string Id { get; set; }
        public string Username { get; set; }
    }
}
