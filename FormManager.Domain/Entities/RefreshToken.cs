using System;
using System.Collections.Generic;
using System.Text;

namespace FormManager.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expiration { get; set; }
        public string UserId { get; set; }
        public bool isRevoked { get; set; }
    }
}
