using FormManager.Application.Users.Responses;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> CreateToken(User user);
        Task<TokenResponse> RefreshToken(string token, string refreshToken);
        Task<bool> CheckPassword(string username, string password);
    }
}
