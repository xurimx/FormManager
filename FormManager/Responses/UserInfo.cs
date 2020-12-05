using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api.Responses
{
    public class UserInfo
    {
        public string Sub { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public string Exp { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public string[] Roles { get; set; }
    }
}
