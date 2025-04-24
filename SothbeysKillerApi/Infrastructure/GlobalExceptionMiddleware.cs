using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using SothbeysKillerApi.Exceptions;

namespace SothbeysKillerApi.Infrastructure
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found: {Description}", ex.Description);
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (UserUnautorizedException ex)
            {
                _logger.LogWarning(ex, "Unauthorized: {Description}", ex.Description);
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (UserValidationExceprion ex) // Виправлено помилку в назві
            {
                _logger.LogWarning(ex, "Validation error: {Description}", ex.Description);
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = exception switch
            {
                UserNotFoundException ex => new
                {
                    StatusCode = (int)statusCode,
                    ex.Field,
                    ex.Description
                },
                UserUnautorizedException ex => new
                {
                    StatusCode = (int)statusCode,
                    ex.Field,
                    ex.Description
                },
                UserValidationExceprion ex => new
                {
                    StatusCode = (int)statusCode,
                    ex.Field,
                    ex.Description
                },
                _ => new
                {
                    StatusCode = (int)statusCode,
                    Message = exception.Message,
                    Details = exception.InnerException?.Message
                }
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}