using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using FormManager.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<User> CreateUserAsync(string username, string email, string password, string role)
        {
            ApplicationUser appUser = await userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return null;
            }
            appUser = new ApplicationUser
            {
                UserName = username,
                Email = email
            };
            IdentityResult identityResult = await userManager.CreateAsync(appUser, password);
            if (!identityResult.Succeeded)
            {
                return null;
            }
            await userManager.AddToRoleAsync(appUser, role);
            return await GetRolesAndClaimsAsync(appUser);
        }

        public async Task<User> FindByUsernameAsync(string username)
        {
            ApplicationUser appUser = await userManager.FindByNameAsync(username);
            if (appUser == null)
            {
                return null;
            }            
            return await GetRolesAndClaimsAsync(appUser);
        }

        private async Task<User> GetRolesAndClaimsAsync(ApplicationUser appUser)
        {
            IList<string> roles = await userManager.GetRolesAsync(appUser);
            IList<Claim> claims = await userManager.GetClaimsAsync(appUser);
            User user = new User
            {
                Id = appUser.Id,
                Username = appUser.UserName,
                Email = appUser.Email,
                Roles = roles.ToArray(),
                Claims = claims.ToArray()
            };
            return user;
        }
    }
}
