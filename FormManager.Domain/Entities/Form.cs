using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Domain.Entities
{
    public class Form
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Company { get; set; }
        public DateTime Appointment { get; set; }
        public DateTime Date { get; set; }
        public string SenderId { get; set; }
    }
}
