using Application.Behaviors;
using Application.Features.VisitorFeatures.Session;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Visitor Session Service -> In-Memory Cache
            services.AddSingleton<IVisitorSessionService, VisitorSessionService>();

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Validation behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}

