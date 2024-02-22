// MemoryCachingExtensions.cs

using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.MemoryCaching.Extensions;

public static class MemoryCachingExtensions
{
    public static IServiceCollection AddMemoryCachingServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        return services;
    }
}