using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Abstractions.Services;
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
        services.AddScoped<ISecondLargestNumberService, SecondLargestNumberService>();

        // Geo Services registrations
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IBorderService, BorderService>();
        services.AddScoped<ICountryBorderService, CountryBorderService>();

        return services;
    }
}