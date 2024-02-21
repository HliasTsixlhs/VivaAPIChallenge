﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Viva.Geo.API.DataAccess.Context;

#nullable disable

namespace Viva.Geo.API.DataAccess.Migrations
{
    [DbContext(typeof(GeoContext))]
    [Migration("20240221142229_initmigration")]
    partial class initmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Border", b =>
                {
                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("BorderCountryId")
                        .HasColumnType("int");

                    b.HasKey("CountryId", "BorderCountryId");

                    b.HasIndex("BorderCountryId");

                    b.ToTable("Borders", (string)null);
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("Capital")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CommonName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("Border", b =>
                {
                    b.HasOne("Viva.Geo.API.DataAccess.DataAccessModels.Country", "BorderCountry")
                        .WithMany()
                        .HasForeignKey("BorderCountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Viva.Geo.API.DataAccess.DataAccessModels.Country", "Country")
                        .WithMany("Borders")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BorderCountry");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Country", b =>
                {
                    b.Navigation("Borders");
                });
#pragma warning restore 612, 618
        }
    }
}
