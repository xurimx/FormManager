using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Application.Users.Responses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string RefreshToken { get; set; }
    }
}
