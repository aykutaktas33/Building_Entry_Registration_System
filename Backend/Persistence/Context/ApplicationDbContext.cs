using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure global query filter for User entity
            modelBuilder.Entity<Team>().HasData(
              new Team { Id = "1", Name = "Sales", Description = "Sales Team", IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
              new Team { Id = "2", Name = "Marketing", Description = "Marketing Team", IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
              new Team { Id = "3", Name = "Engineering", Description = "Software Engineer Team", IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
              new Team { Id = "4", Name = "Human Resources", Description = "Human Resources Team", IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
              new Team { Id = "5", Name = "Finance", Description = "Finance & Account Team", IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow }
            );

            modelBuilder.Entity<Entrance>().HasData(
                new Entrance { Id = "ENTRANCE-001", Name = "Main Entrance", IsActive = true, IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new Entrance { Id = "ENTRANCE-002", Name = "Side Entrance", IsActive = true, IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow },
                new Entrance { Id = "ENTRANCE-003", Name = "Urgent Exit", IsActive = false, IsDelete = false, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow }
            );

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Entrance> Entrances { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentDate = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is BaseEntity addedEntity)
                        {
                            addedEntity.CreateDate = currentDate;
                            addedEntity.UpdateDate = currentDate;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity modifiedEntity)
                        {
                            modifiedEntity.UpdateDate = currentDate;
                        }
                        break;

                    case EntityState.Deleted:

                        if (entry.Entity is BaseEntity deletedEntity)
                        {
                            entry.State = EntityState.Modified;

                            deletedEntity.DeleteDate = currentDate;
                            deletedEntity.IsDelete = true;
                        }
                        else
                            entry.State = EntityState.Deleted;
                        break;

                    // Handle other states if needed

                    default:
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
