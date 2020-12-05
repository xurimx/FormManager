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
    class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
    {
        public DbSet<Form> Forms { get; set; }
        public AppDbContext(DbContextOptions options) : base(options) { }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
