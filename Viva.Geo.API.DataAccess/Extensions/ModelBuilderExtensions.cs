using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.DataAccess.Extensions;

public static class ModelBuilderExtensions
{
    public static void RegisterCountryEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries");
            entity.HasKey(e => e.CountryId);
            entity.Property(e => e.CommonName).IsRequired();
            entity.Property(e => e.Capital);
        });
    }

    public static void RegisterBorderEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Border>(entity =>
        {
            entity.ToTable("Borders");
            entity.HasKey(e => e.BorderId);
            entity.Property(e => e.BorderCode).IsRequired();
        });
    }

    public static void RegisterCountryBorderEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CountryBorder>()
            .HasKey(cb => new { cb.CountryId, cb.BorderId });

        modelBuilder.Entity<CountryBorder>()
            .HasOne(cb => cb.Country)
            .WithMany(c => c.CountryBorders)
            .HasForeignKey(cb => cb.CountryId);

        modelBuilder.Entity<CountryBorder>()
            .HasOne(cb => cb.Border)
            .WithMany(b => b.CountryBorders)
            .HasForeignKey(cb => cb.BorderId);
    }
}