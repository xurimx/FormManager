using FormManager.Infrastructure.Data;
using FormManager.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using FormManager.Application.Common.Interfaces;
using FormManager.Infrastructure.Services;
using System;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace FormManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                //                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            });

            services.AddScoped<IAppDbContext>(cfg => cfg.GetRequiredService<AppDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddAuthentication()
                .AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = configuration["Tokens:Issuer"],
                        ValidAudience = configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<ISmtpConfigurationService, SmtpConfigurationService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}
