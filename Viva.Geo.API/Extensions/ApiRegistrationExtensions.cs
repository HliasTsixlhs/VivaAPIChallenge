using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
using Viva.Geo.API.Core.Exceptions;
using Viva.Geo.API.Core.Repositories;
using Viva.Geo.API.Core.Services;
using Viva.Geo.API.DataAccess.Context;
using Viva.Geo.API.Options;

namespace Viva.Geo.API.Extensions;

public static class ApiRegistrationExtensions
{
    public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection("DatabaseOptions"));

        services.AddDbContext<GeoContext>(options =>
        {
            var dbOptions = configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>();
            options.UseSqlServer(dbOptions.ConnectionString,
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        });

        return services;
    }

    /// <summary>
    /// Adds and configures HttpClient services using HttpClientFactory.
    /// HttpClientFactory offers several benefits like managing the lifetimes of HttpClient instances,
    /// improving DNS (Domain Name System) updates, and promoting reusable HttpClient instances.
    /// This method configures a named HttpClient and can also register typed clients for specific services.
    /// </summary>
    /// <returns>The IServiceCollection for chaining.</returns>
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        // Configure HttpClient for rest countries API
        services.AddHttpClient("restCountriesApiClient", client =>
        {
            client.BaseAddress = new Uri("https://restcountries.com/v3.1/all");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Geo Repositories registrations
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IBorderRepository, BorderRepository>();
        services.AddScoped<ICountryBorderRepository, CountryBorderRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IArrayProcessingService, ArrayProcessingService>();

        // Geo Services registrations
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IBorderService, BorderService>();
        services.AddScoped<ICountryBorderService, CountryBorderService>();

        return services;
    }

    /// <summary>
    /// Configures ProblemDetails middleware to handle exceptions and failed requests with custom error responses.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="environment">The hosting environment, used to tailor error responses based on the application's running environment.</param>
    /// <returns>The updated IServiceCollection with ProblemDetails middleware configured.</returns>
    /// <remarks>
    /// This method enhances the APIs error handling capabilities by setting up ProblemDetails middleware. It enables the API to return standardized, machine-readable error details, beneficial for RESTful APIs.
    /// In a development environment, detailed error information is provided for debugging purposes. In a production environment, a generic error message is returned to avoid exposing sensitive details.
    /// Custom exception handling can be added to provide specific error information for known exception types.
    /// </remarks>
    public static IServiceCollection AddProblemDetailsHandling(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                var exception = ctx.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

                // Setting a generic error message for production environment
                if (!environment.IsDevelopment() && ctx.ProblemDetails.Status == 500)
                {
                    ctx.ProblemDetails.Title = "An error occurred";
                    ctx.ProblemDetails.Detail =
                        "An error occurred in our API. Please contact support. Note: Use the trace id when contracting us.";
                    return;
                }

                // Customizing based on specific exception types
                switch (exception)
                {
                    case InsufficientUniqueElementsException ex:
                        ctx.ProblemDetails.Status = StatusCodes.Status400BadRequest;
                        ctx.ProblemDetails.Title = "Insufficient Unique Elements";
                        ctx.ProblemDetails.Detail = ex.Message;
                        break;
                    // Add more cases for other custom exceptions if necessary
                }
            };
        });

        return services;
    }
}