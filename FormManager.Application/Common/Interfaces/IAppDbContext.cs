﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FormManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FormManager.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Form> Forms { get; set; }
        DbSet<Domain.Entities.Config> Configs { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync();

    }
}
