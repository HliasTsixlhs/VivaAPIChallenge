using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Common.Logging.Serilog.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder UseCustomSerilog(this WebApplicationBuilder builder)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            // Additional configurations or enrichers can be added here
            .CreateLogger();

        // Set Serilog as the logging provider
        builder.Host.UseSerilog();

        return builder;
    }
}