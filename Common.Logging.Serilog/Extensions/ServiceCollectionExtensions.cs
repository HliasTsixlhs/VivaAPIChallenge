using Common.Logging.Serilog.Factories;
using Common.Logging.Serilog.Factories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Logging.Serilog.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventIdFactory(this IServiceCollection services)
    {
        services.AddSingleton<IEventIdFactory, EventIdFactory>();
        return services;
    }
}