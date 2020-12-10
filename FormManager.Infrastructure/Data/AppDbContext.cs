using FormManager.Application.Common.Interfaces;
using FormManager.Domain.Entities;
using FormManager.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FormManager.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
    {
        public DbSet<Form> Forms { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
            
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Config>().HasKey(c => c.Key);
            base.OnModelCreating(builder);
        }
    }
}
