# Common.Web Project Documentation

## Overview

The Common.Web project in Viva.Geo.API includes a crucial middleware component: the CorrelationId middleware. This
middleware is essential for enhancing the API's logging and monitoring capabilities.(More useful in a distributed
architecture system like microservices). It's designed to be lightweight with minimal performance overhead.

## CorrelationId Middleware

The CorrelationId middleware adds a unique identifier, known as the Correlation ID, to each request. This ID is then
pushed to Serilog's LogContext, enabling improved tracking and monitoring of requests.

### Implementation

The middleware utilizes the `X-Correlation-ID` HTTP header to implement this feature. Here's a brief overview of the
implementation:

```csharp
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
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault() ??
                            Guid.NewGuid().ToString();

        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
```

### Benefits

- **Traceability**: Enhances the ability to trace requests across different components of the system.
- **Logging and Monitoring**: Improves logging by attaching a unique identifier to each log entry.
- **Compatibility with Monitoring Tools**: Facilitates integration with systems like Prometheus, OpenTelemetry, and
  Datadog.
- **Distributed Systems**: Particularly beneficial in distributed architectures, aiding in tracking and troubleshooting.

### Performance

The performance impact of this middleware is minimal, making it a cost-effective solution for enhancing the traceability
and monitoring of the API.

## Conclusion

Incorporating the CorrelationId middleware into the Common.Web project is a strategic decision for improving the API's
operability in distributed environments. Its integration with Serilog enhances the logging capabilities, providing a
robust solution for monitoring and debugging.
