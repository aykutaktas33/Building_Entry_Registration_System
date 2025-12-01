using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Visitor> Visitors { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<Entrance> Entrances { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
