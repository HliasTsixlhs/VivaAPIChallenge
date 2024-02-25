
# ProblemDetails Feature in Viva.Geo.API

## Overview

The Viva.Geo.API utilizes Microsoft's `ProblemDetails` feature, which is a standardized way to return error details from HTTP APIs. It helps us return structured and meaningful error responses that adhere to the [RFC 7807](https://tools.ietf.org/html/rfc7807) specification, making it universally understandable and actionable.

## Implementation

### Registration

`ProblemDetails` are configured in the application's services with custom logic for handling exceptions:

```csharp
public static IServiceCollection AddProblemDetailsHandling(
    this IServiceCollection services,
    IWebHostEnvironment environment)
{
    services.AddProblemDetails(options =>
    {
        // Customization logic for ProblemDetails goes here
    });

    return services;
}
```

### Customization

The `ProblemDetails` are customized to handle various exceptions gracefully:

- In a non-development environment, we avoid exposing stack traces for 500 Internal Server Errors.
- For known exceptions, we provide meaningful messages and appropriate HTTP status codes.
- Custom exceptions like `InsufficientUniqueElementsException` are specifically handled to provide relevant information.

### Error Handling Middleware

The application is configured to use middleware for global exception handling:

```csharp
app.UseExceptionHandler();
app.UseStatusCodePages();
```

This ensures that no exceptions are swallowed and that they are all processed to return structured `ProblemDetails`.

## Custom Exceptions

We have created custom exceptions, such as `InsufficientUniqueElementsException`, to encapsulate specific error scenarios. These exceptions are caught in our services, logged with a trace ID for support tracking, and re-thrown to be handled by our `ProblemDetails` setup.

## Note on Performance and Best Practices

While using `ProblemDetails` provides a standardized error response, it introduces performance considerations due to the global handling of exceptions. An alternative approach is demonstrated by Nick Chapsas in his video, which outlines performance optimizations for error handling in ASP.NET Core applications.

[Watch Nick Chapsas's video on Error Handling](https://www.youtube.com/watch?v=a1ye9eGTB98&t=647s)

## Conclusion

The integration of `ProblemDetails` into Viva.Geo.API's error handling strategy ensures that error responses are consistent, informative, and secure. It aligns with best practices and enhances the API's usability by providing clear and actionable feedback to clients in case of errors.
