using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.DataAccess.DataAccessModels;
using Viva.Geo.API.DataAccess.Extensions;

namespace Viva.Geo.API.DataAccess.Context;

public class GeoContext : DbContext
{
    public GeoContext(DbContextOptions<GeoContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<Border> Borders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Use the extension methods for configuration
        modelBuilder.RegisterCountryEntity();
        modelBuilder.RegisterBorderEntity();
        modelBuilder.RegisterCountryBorderEntity();
    }
}