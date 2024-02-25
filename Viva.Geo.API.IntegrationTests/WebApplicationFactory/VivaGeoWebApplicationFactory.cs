using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Viva.Geo.API.DataAccess.Context;
using Viva.Geo.API.Options;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Viva.Geo.API.IntegrationTests.WebApplicationFactory
{
    internal class VivaGeoWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, conf) =>
            {
                var projectDir = Directory.GetCurrentDirectory();
                var configPath = Path.Combine(projectDir, "appsettings.test.json");
                conf.AddJsonFile(configPath, optional: false, reloadOnChange: true);
            });

            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<GeoContext>));

                var databaseOptions = GetDatabaseOptions();
                services.AddDbContext<GeoContext>(options =>
                    options.UseSqlServer(databaseOptions.ConnectionString));

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<GeoContext>();
                dbContext.Database.Migrate(); // Apply migrations (Create database)
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    using var scope = Services.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<GeoContext>();
                    dbContext.Database.EnsureDeleted();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error disposing the WebApplicationFactory: {ex.Message}");
                }
            }

            base.Dispose(disposing);
        }


        private static DatabaseOptions GetDatabaseOptions()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            var databaseOptions = new DatabaseOptions();
            configuration.GetSection("DatabaseOptions").Bind(databaseOptions);

            return databaseOptions;
        }
    }
}