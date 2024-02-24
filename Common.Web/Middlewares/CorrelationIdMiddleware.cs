using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Common.Web.Middlewares;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Extract or generate a Correlation ID
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString();

        // Add the Correlation ID to the response headers for client transparency
        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        // Add the Correlation ID to the Serilog LogContext
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            // Continue processing the request
            await _next(context);
        }
    }
}