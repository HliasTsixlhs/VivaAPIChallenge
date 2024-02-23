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
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName()
            .CreateLogger();
        return builder;
    }
}