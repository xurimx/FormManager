using FormManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FormManager.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
        public string UserId => httpContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public IDictionary<string, string[]> Claims => throw new NotImplementedException();
    }
}
