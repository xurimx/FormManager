using FormManager.Application.Users.Responses;
using FormManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(string username, string email, string password, string role);
        Task<User> FindByUsernameAsync(string username);
    }
}
