using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.Core.Abstractions.Services;
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


    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ISecondLargestNumberService, SecondLargestNumberService>();

        return services;
    }
}