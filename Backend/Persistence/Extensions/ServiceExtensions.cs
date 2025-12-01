using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Persistence.Extensions
{
    public static class ServiceExtensions
    {
        private const string apiName = "Building Entry Web API";

        private const int apiMajorVersion = 1;
        private const int apiMinorVersion = 0;

        private const string apiEmailContact = "aykut@aykutaktas.net";
        private const string apiUrl = "https://www.aykutaktas.net";

        private const string apiXml = "BuildingEntryApi.WebApi.xml";

        public static void AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                string xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + apiXml;

                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v" + apiMajorVersion, new OpenApiInfo
                {
                    Title = apiName,
                    Version = $"v{apiMajorVersion}.{apiMinorVersion}",
                    Description = $"{apiName} v{apiMajorVersion}.{apiMinorVersion} This Api will be responsible for overall data distribution and authorization.",
                    Contact = new OpenApiContact
                    {
                        Name = apiName,
                        Email = apiEmailContact,
                        Url = new Uri(apiUrl),
                    }
                });

                //c.SwaggerDoc("v" + apiMajorVersion + 1, new OpenApiInfo
                //{
                //    Title = apiName,
                //    Version = $"v{apiMajorVersion + 1}.{apiMinorVersion}",
                //    Description = $"{apiName} v{apiMajorVersion + 1}.{apiMinorVersion} This Api will be responsible for overall data distribution and authorization.",
                //    Contact = new OpenApiContact
                //    {
                //        Name = apiName,
                //        Email = apiEmailContact,
                //        Url = new Uri(apiUrl),
                //    }
                //});

                // Define the security scheme for JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void AddApiVersioningExtension(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(apiMajorVersion, apiMinorVersion);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;

                config.ApiVersionReader = ApiVersionReader.Combine(
                   new UrlSegmentApiVersionReader(),
                   new HeaderApiVersionReader("version:apiVersion")
                );
            });
        }

        public static void UseSwaggerDocumentation(this IApplicationBuilder app, bool development = false)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{(development ? "/swagger" : "../swagger")}/v{apiMajorVersion}/swagger.json", $"{apiName} v{apiMajorVersion}.{apiMinorVersion}");
                c.SwaggerEndpoint($"{(development ? "/swagger" : "../swagger")}/v{apiMajorVersion + 1}/swagger.json", $"{apiName} v{apiMajorVersion + 1}.{apiMinorVersion}");
                c.RoutePrefix = "swagger"; // veya string.Empty için root

                c.ConfigObject.AdditionalItems["cacheControl"] = "no-cache";
                c.ConfigObject.AdditionalItems["docExpansion"] = "none";
            });
        }

    }
}
