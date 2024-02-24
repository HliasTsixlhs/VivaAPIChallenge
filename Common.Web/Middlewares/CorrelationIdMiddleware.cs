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

    /// <summary>
    /// Invokes the middleware to process the HTTP context.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    /// <returns>A Task that completes when the middleware has completed processing.</returns>
    /// <remarks>
    /// This middleware extracts or generates a unique Correlation ID for each request.
    /// The Correlation ID is used to track and correlate logs and activities across
    /// distributed systems and services, aiding in tracing and monitoring.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        // Extract or generate a Correlation ID
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault() ??
                            Guid.NewGuid().ToString();

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