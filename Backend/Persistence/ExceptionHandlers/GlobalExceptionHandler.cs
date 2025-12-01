using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Persistence.ExceptionHandlers
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _environment;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, errorResponse) = CreateErrorResponse(exception, context);

            context.Response.StatusCode = (int)statusCode;

            var errorResponseJson = JsonConvert.SerializeObject(errorResponse);
            return context.Response.WriteAsync(errorResponseJson);
        }

        private (HttpStatusCode statusCode, ErrorResponse errorResponse) CreateErrorResponse(Exception exception, HttpContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorResponse = new ErrorResponse();

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.StatusCode = (int)statusCode;
                    errorResponse.Message = "Validation failed";
                    errorResponse.Type = "Validation";
                    errorResponse.Errors = validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(x => x.ErrorMessage).ToArray()
                        );
                    break;

                case BusinessException businessException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.StatusCode = (int)statusCode;
                    errorResponse.Message = businessException.Message;
                    errorResponse.Type = "BusinessRule";
                    break;

                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorResponse.StatusCode = (int)statusCode;
                    errorResponse.Message = "Resource not found";
                    errorResponse.Type = "NotFound";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    errorResponse.StatusCode = (int)statusCode;
                    errorResponse.Message = "An internal server error occurred";
                    errorResponse.Type = "InternalError";

                    // Development ortamında detaylı error göster
                    if (_environment.IsDevelopment())
                    {
                        errorResponse.Details = exception.StackTrace;
                        errorResponse.InnerException = exception.InnerException?.Message;
                    }
                    break;
            }

            // Trace ID for correlation
            errorResponse.TraceId = context.TraceIdentifier;

            return (statusCode, errorResponse);
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string InnerException { get; set; } = string.Empty;
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}