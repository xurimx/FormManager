using FormManager.Application.Common.Interfaces;
using FormManager.Application.Common.Models;
using FormManager.Application.Users.Queries;
using FormManager.Domain.Entities;
using FormManager.Infrastructure.Data;
using FormManager.Infrastructure.Models;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;

        public UserRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        public async Task<User> CreateUserAsync(string username, string email, string password, string role)
        {
            ApplicationUser appUser = await userManager.FindByNameAsync(username);
            if (appUser != null)
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

        public async Task<User> GetUserById(string id)
        {
            ApplicationUser appUser = await userManager.FindByIdAsync(id);
            return await GetRolesAndClaimsAsync(appUser);
        }

        public async Task<Pagination<User>> QueryUsersAsync(GetUsersQuery request)
        {
            //Todo generalization of this block
            int page = request.Page ?? 1;
            int limit = request.Limit ?? 10;
            int total = await context.Users.CountAsync();

            var query = context.Users.AsQueryable();
            if (!string.IsNullOrEmpty(request.SearchInput))
            {
                if (request.SearchColumns.Length != 0)
                {
                    var predicate = PredicateBuilder.New<ApplicationUser>();
                    foreach (var column in request.SearchColumns)
                    {
                        ParameterExpression param = Expression.Parameter(typeof(ApplicationUser), "user");
                        MemberExpression property = Expression.Property(param, column);
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        ConstantExpression inputValue = Expression.Constant(request.SearchInput, typeof(string));
                        MethodCallExpression containsMethodExpression = Expression.Call(property, method, inputValue);
                        predicate = predicate.Or(Expression.Lambda<Func<ApplicationUser, bool>>(containsMethodExpression, param));
                    }
                    query = query.Where(predicate);
                }
                else
                {
                    query = query.Where(x =>
                            x.UserName.Contains(request.SearchInput) ||
                            x.Email.Contains(request.SearchInput) ||
                            x.PhoneNumber.Contains(request.SearchInput));
                }
            }
            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                ParameterExpression param = Expression.Parameter(typeof(ApplicationUser), "x");
                MemberExpression property = Expression.Property(param, request.OrderBy);
                var lambda = Expression.Lambda(property, param);

                var orderBy = Expression.Call(typeof(Queryable),
                                request.OrderDirection != "desc" ? "OrderBy" : "OrderByDescending",
                                new Type[] { typeof(ApplicationUser), property.Type },
                                query.Expression,
                                Expression.Quote(lambda));

                query = query.Provider.CreateQuery<ApplicationUser>(orderBy);
            }
            int filtered = query.Count();
            List<ApplicationUser> items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            List<User> mappedUsers = new List<User>();

            foreach (var item in items)
            {
                mappedUsers.Add(await GetRolesAndClaimsAsync(item));
            }

            Pagination<User> pagination = new Pagination<User>
            {
                TotalItems = await context.Users.CountAsync(),
                FilteredItems = filtered,
                Page = page,
                Items = mappedUsers,
                TotalPages = (int)Math.Ceiling((double)filtered / limit)
            };

            pagination.BuildNavigation(request);

            return pagination;
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
                Claims = claims.GroupBy(x => x.Type.ToString())
                               .Select(g => (key: g.Key, values: g.Select(k => k.Value).ToArray()))
                               .ToDictionary(k => k.key, k => k.values)
            };
            return user;
        }
    }
}
