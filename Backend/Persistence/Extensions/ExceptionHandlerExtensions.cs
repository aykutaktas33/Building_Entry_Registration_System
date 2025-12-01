using Microsoft.Extensions.DependencyInjection;
using Persistence.ExceptionHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static void AddExceptionHandler<T>(this IServiceCollection services) where T : class
        {
            services.AddTransient<T>();
        }
    }
}
