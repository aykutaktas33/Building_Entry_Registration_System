using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence
{
    public static class DependencyInjection
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var environment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var dbName = $"BuildingEntryDb_{environment}";

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(dbName));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
    }
}
