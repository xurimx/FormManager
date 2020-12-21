using FormManager.Api;
using FormManager.Application.Common.Interfaces;
using FormManager.Application.Config.ViewModels;
using FormManager.Infrastructure.Data;
using FormManager.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Application.IntegrationTests
{
    [SetUpFixture]
    public class Testing
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        private static string _currentUserId;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();

            var services = new ServiceCollection();

            var startup = new Startup(_configuration);

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w => w.ApplicationName == "FormManager.Api" && w.EnvironmentName == "Development"));
            services.AddLogging();

            startup.ConfigureServices(services);

            var currentUserService = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(ICurrentUserService));
            services.Remove(currentUserService);

            services.AddTransient(provider =>
                Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            EnsureDatabase();

            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
        }

        private static void EnsureDatabase()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<AppDbContext>();

            context.Database.Migrate();
        }

        public static async Task ResetState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        }

        public static async Task SetupSmtp()
        {
            using var scope = _scopeFactory.CreateScope();
            var smtpConfig = scope.ServiceProvider.GetRequiredService<ISmtpConfigurationService>();
            await smtpConfig.UpdateConfiguration(new SmtpConfiguration
            {
                Host = "smtp-relay.sendinblue.com",
                Port = "587",
                From = "xurimx@gmail.com",
                To = "umorinax@gmail.com",
                Username = "xurimx@gmail.com",
                Password = "VXI0PgYhjF2QtE5p"
            });
        }

        public static async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetService<AppDbContext>();
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
        }

        public static async Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetService<AppDbContext>();
            return await ctx.FindAsync<TEntity>(id);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator.Send(request);
        }

        public static async Task<string> RunAsDefaultUserAsync()
        {
            return await RunAsUserAsync("test@local", "Testing1234!", new string[] { "user" });
        }

        public static async Task<string> RunAsAdministratorAsync()
        {
            return await RunAsUserAsync("administrator@local", "Administrator1234!", new[] { "admin" });
        }

        public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser { UserName = userName, Email = userName };

            var result = await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRolesAsync(user, roles);
            }

            if (result.Succeeded)
            {
                _currentUserId = user.Id;

                return _currentUserId;
            }

            var errors = string.Join(Environment.NewLine, result.Errors);

            throw new Exception($"Unable to create {userName}.");
        }
    }
}
