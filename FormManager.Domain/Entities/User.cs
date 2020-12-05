using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace FormManager.Domain.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string[] Roles { get; set; }
        public Dictionary<string, string[]> Claims { get; set; }
    }
}
