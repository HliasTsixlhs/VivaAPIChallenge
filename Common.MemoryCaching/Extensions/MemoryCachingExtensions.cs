using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.MemoryCaching.Extensions;

public static class MemoryCachingExtensions
{
    /// <summary>
    /// Adds memory caching services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The application's configuration.</param>
    /// <returns>The IServiceCollection.</returns>
    public static IServiceCollection AddMemoryCachingServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var memoryCacheOptions = new MemoryCacheOptions();
        configuration.Bind("MemoryCacheOptions", memoryCacheOptions);

        // Configure the memory cache with options from configuration
        services.AddMemoryCache(options =>
        {
            // Set the size limit of the memory cache
            options.SizeLimit = memoryCacheOptions.SizeLimit;

            // Set the frequency to scan for expired items
            options.ExpirationScanFrequency = memoryCacheOptions.ExpirationScanFrequency;

            // Set the percentage of cache to compact when the size limit is exceeded
            options.CompactionPercentage = memoryCacheOptions.CompactionPercentage;

            // Determines whether to track linked cache entries (used in scenarios like child actions in MVC)
            options.TrackLinkedCacheEntries = memoryCacheOptions.TrackLinkedCacheEntries;

            // Enables tracking of detailed memory cache statistics
            options.TrackStatistics = memoryCacheOptions.TrackStatistics;
        });

        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        return services;
    }
}