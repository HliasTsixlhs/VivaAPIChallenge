using Microsoft.EntityFrameworkCore;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.DataAccess.Extensions
{
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
            modelBuilder.Entity<Border>()
                .HasKey(b => new {b.CountryId, b.BorderCountryId});

            modelBuilder.Entity<Border>()
                .HasOne(b => b.Country)
                .WithMany(c => c.Borders)
                .HasForeignKey(b => b.CountryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Border>()
                .HasOne(b => b.BorderCountry)
                .WithMany()
                .HasForeignKey(b => b.BorderCountryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Border>().ToTable("Borders");
        }
    }
}