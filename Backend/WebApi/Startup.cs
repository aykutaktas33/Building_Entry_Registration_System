using Application;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Context;
using Persistence.ExceptionHandlers;
using Persistence.Extensions;
using System;
using System.Linq;

namespace BuildingEntryApi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Auth

            AuthExtensions.AddAuth(services, Configuration);

            #endregion

            #region Swagger & Api Versioning

            ServiceExtensions.AddSwaggerExtension(services);
            ServiceExtensions.AddApiVersioningExtension(services);

            #endregion

            services.AddApplication();
            services.AddPersistence(Configuration);
            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5000")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials()
                               .SetIsOriginAllowed(origin => true)
                               .WithMethods("GET", "POST", "PUT", "DELETE");
                    });
            });

            services.AddControllers();

            services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngularApp");
            app.UseRouting();

            #region Swagger

            ServiceExtensions.UseSwaggerDocumentation(app, env.IsDevelopment());

            #endregion

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
