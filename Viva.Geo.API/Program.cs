using Asp.Versioning;
using Common.HealthChecks.Extensions;
using Common.Logging.Serilog.Extensions;
using Common.MemoryCaching.Extensions;
using Common.Web.Middlewares;
using Microsoft.OpenApi.Models;
using Serilog;
using Viva.Geo.API.Extensions;
using Viva.Geo.API.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Initializes a custom Serilog logging configuration.
builder.UseCustomSerilog();
builder.Host.UseSerilog();
builder.Services.AddEventIdFactory();

// Configures AutoMapper for mapping objects within the application.
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Retrieves the version of the application from the assembly information.
var version = typeof(Program).Assembly.GetName().Version?.ToString();
var majorVersion = version?.Split('.')[0];
var minorVersion = version?.Split('.')[1];

// Sets up API versioning with default versions derived from the assembly version.
builder.Services.AddApiVersioning(options =>
{
    if (majorVersion != null && minorVersion != null)
        options.DefaultApiVersion = new ApiVersion(int.Parse(majorVersion), int.Parse(minorVersion));
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Adds health check services to the application.
builder.Services.AddCustomHealthChecks(builder.Configuration);

// Configures database context and other necessary services for the application.
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddClients();
builder.Services.AddRepositories();
builder.Services.AddServices();

// Configures memory caching services.
builder.Services.AddMemoryCachingServices(builder.Configuration);

// Configures Swagger/OpenAPI support for documenting the API.
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

// Adds controllers to the service collection.
builder.Services.AddControllers();
builder.Services.AddProblemDetailsHandling(builder.Environment);

var app = builder.Build();

// Configures global exception handling for the application.
app.UseExceptionHandler();
app.UseStatusCodePages();

// Enables detailed error pages in development environment.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Enforces HTTPS in production environments.
    app.UseHsts();
}

// Adds middleware to include a correlation ID in the response header for tracking requests.
app.UseMiddleware<CorrelationIdMiddleware>();

// Enables and configures Swagger UI.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Viva Geo API V1");
    options.RoutePrefix = string.Empty; // Serves Swagger UI at the application's root URL.
});

// Configures the application to use routing for request handling.
app.UseRouting();

// Adds custom health check endpoints to the application.
app.UseCustomHealthChecks();

// Authentication and Authorization middleware could be added here if needed.
// app.UseAuthentication();
// app.UseAuthorization();

// Maps controller actions to routes.
app.MapControllers();

// Runs the application.
app.Run();
