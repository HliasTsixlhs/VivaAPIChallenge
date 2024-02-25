
# Swagger Documentation for Viva.Geo.API
![SwaggerUI.PNG](Images%2FSwagger%2FSwaggerUI.PNG)
## Overview

Swagger is an essential tool for documenting and testing RESTful APIs. In Viva.Geo.API, we have implemented Swagger using the `Swashbuckle.AspNetCore` package, which seamlessly integrates with our ASP.NET Core application to generate Swagger JSON and present a beautiful, interactive documentation UI.

## Why Use Swagger?

Swagger offers numerous advantages for both API developers and consumers:

- **Interactive Documentation**: Provides a web-based UI where users can see all available endpoints, their expected parameters, and the results.
- **Testing Platform**: Enables users to send live requests to the API directly from the documentation interface.
- **Code-Generation**: Supports generating client libraries in multiple languages from the Swagger definition.
- **Standards-Based**: Uses the OpenAPI Specification, which is widely recognized and adopted.
- **Ease of Use**: Simplifies the process of writing and maintaining API documentation.

## Swagger UI and JSON

Viva.Geo.API exposes two Swagger-related URLs:

- **Swagger UI**: `http://localhost:5266/index.html` - A web page that renders the API documentation and provides an interactive interface for sending test requests.
- **Swagger JSON**: `http://localhost:5266/swagger/v1/swagger.json` - The raw Swagger definition file that describes the API's endpoints, parameters, and other details.
![swaggerAPIContracts.PNG](Images%2FSwagger%2FswaggerAPIContracts.PNG)
## Implementation

### Registration

Swagger is set up in the application's startup configuration as follows:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Viva Geo API",
        Version = "v1",
        Description = "API for Geo operations"
    });
});
```

### Usage

The Swagger UI is made available by adding middleware in the application's request pipeline:

```csharp
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Viva Geo API V1");
    options.RoutePrefix = string.Empty; // Serves Swagger UI at the application's root URL.
});
```

## Conclusion

By integrating Swagger with Viva.Geo.API, we provide a user-friendly interface for exploring the API, alongside a powerful platform for testing and interacting with the available endpoints. This greatly enhances the developer experience and encourages adoption and utilization of the API.
