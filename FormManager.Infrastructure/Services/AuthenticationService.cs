using FormManager.Application.Common.Exceptions;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Users.Responses;
using FormManager.Domain.Entities;
using FormManager.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        //Todo: slap an interface to UserManager
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IAppDbContext context;
        private readonly IUserRepository repository;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IConfiguration config, IAppDbContext context, IUserRepository repository)
        {
            this.userManager = userManager;
            this.config = config;
            this.context = context;
            this.repository = repository;
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

        public async Task<TokenResponse> CreateToken(User user)
        {
            JwtSecurityToken token = GenerateJwtToken(user);
            RefreshToken refreshToken = await GenerateRefreshTokenAsync(user.Id);
            var results = new TokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken.Token
            };
            return results;
        }

        public async Task<TokenResponse> RefreshToken(string token, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new AuthenticateException("Refresh token or Access token is not valid. Please log in manually!");

            ClaimsPrincipal claimsPrincipal = GetPrincipalFromExpiredToken(token);

            Claim claim = claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            User user = await repository.GetUserById(claim.Value);
            if (user != null)
            {
                RefreshToken refreshTokenDb = await context.RefreshTokens
                    .Where(x => x.UserId == user.Id && x.Token == refreshToken && x.isRevoked == false && x.Expiration > DateTime.UtcNow)
                    .FirstOrDefaultAsync();
                if (refreshTokenDb != null)
                {
                    JwtSecurityToken jwtToken = GenerateJwtToken(user);
                    return new TokenResponse
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        Expiration = jwtToken.ValidTo,
                        RefreshToken = refreshTokenDb.Token                        
                    };
                }
                else
                {
                    throw new AuthenticateException("Refresh token or Access token is not valid. Please log in manually!");
                }                
            }
            throw new AuthenticateException("Refresh token or Access token is not valid. Please log in manually!");
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config["Tokens:Issuer"],
                ValidAudience = config["Tokens:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"])),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new AuthenticateException("Invalid token", 400);

                return principal;
            }
            catch (Exception e)
            {
                throw new AuthenticateException("Refresh token or Access token is not valid. Please log in manually!");
            }
        }


        private async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            List<RefreshToken> tokens = context.RefreshTokens.Where(x => x.UserId == userId && x.isRevoked == false).ToList();
            foreach (var item in tokens)
            {
                item.isRevoked = true;
                context.RefreshTokens.Update(item);
            }
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                RefreshToken refreshToken = new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expiration = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    isRevoked = false,
                    UserId = userId                    
                };
                context.RefreshTokens.Add(refreshToken);
                await context.SaveChangesAsync();
                return refreshToken;
            }
        }

        private JwtSecurityToken GenerateJwtToken(User user)
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
            return token;
        }
    }
}

