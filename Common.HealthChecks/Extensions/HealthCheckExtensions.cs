using Common.HealthChecks.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Configuration;

namespace Common.HealthChecks.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            var healthCheckUIOptions = new HealthCheckUIOptions();
            configuration.Bind("HealthCheckUI", healthCheckUIOptions);

            services.AddHealthChecks()
                .AddCheck("liveness", () => HealthCheckResult.Healthy());

            services.AddHealthChecksUI(setupSettings =>
                    {
                        setupSettings.AddHealthCheckEndpoint("Service's self Health.", "health");
                        setupSettings.SetEvaluationTimeInSeconds(healthCheckUIOptions.EvaluationTimeInSeconds);
                        setupSettings.MaximumHistoryEntriesPerEndpoint(healthCheckUIOptions.MaximumHistoryEntriesPerEndpoint);
                        setupSettings.SetApiMaxActiveRequests(healthCheckUIOptions.ApiMaxActiveRequests);
                        setupSettings.SetMinimumSecondsBetweenFailureNotifications(healthCheckUIOptions.MinimumSecondsBetweenFailureNotifications);
                    }
                )
                .AddInMemoryStorage();
            return services;
        }

        public static IApplicationBuilder UseCustomHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/self", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui";
                options.ApiPath = "/health-ui-api";
            });

            return app;
        }
    }
}