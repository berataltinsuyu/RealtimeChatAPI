using System.Net;
using System.Text.Json;

namespace RealtimeChatAPI.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        var statusCode = GetStatusCode(exception.Message);

        var response = new
        {
            message = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }

    private static int GetStatusCode(string message)
    {
        if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return (int)HttpStatusCode.NotFound;
        }

        if (message.Contains("not member", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("uye degil", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("üye değil", StringComparison.OrdinalIgnoreCase))
        {
            return (int)HttpStatusCode.Forbidden;
        }

        if (message.Contains("already", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("zaten", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("invalid", StringComparison.OrdinalIgnoreCase) ||
            message.Contains("cannot be empty", StringComparison.OrdinalIgnoreCase))
        {
            return (int)HttpStatusCode.BadRequest;
        }

        return (int)HttpStatusCode.InternalServerError;
    }
}