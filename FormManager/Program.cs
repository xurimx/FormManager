using FormManager.Infrastructure.Data;
using FormManager.Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FormManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDefaultAccountAsync(host).Wait();
            host.Run();
        }

        private static async Task CreateDefaultAccountAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                ctx.Database.Migrate();
                //bool v = ctx.Database.EnsureCreated();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                ApplicationUser admin = await userManager.FindByNameAsync("admin");
                IdentityRole adminRole = await roleManager.FindByNameAsync("admin");
                if (adminRole == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                }
                IdentityRole userRole = await roleManager.FindByNameAsync("user");
                if (userRole == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("user"));
                }
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@test.com",
                    };
                    IdentityResult result = await userManager.CreateAsync(admin, "P@ssw0rd");
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException("Error creating default user");
                    }
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
