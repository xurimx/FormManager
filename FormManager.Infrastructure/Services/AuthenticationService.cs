using FormManager.Application.Common.Interfaces;
using FormManager.Application.Users.Responses;
using FormManager.Domain.Entities;
using FormManager.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }
        public async Task<bool> CheckPassword(string username, string password)
        {
            ApplicationUser appUser = await userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return false;
            }
            return await userManager.CheckPasswordAsync(appUser, password);
        }

        public Task<TokenResponse> CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                config["Tokens:Issuer"],
                config["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials);

            var results = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };

            return Task.FromResult(results);
        }
    }
}

