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
    [Migration("20240224141358_fixedStringLengths")]
    partial class fixedStringLengths
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Border", b =>
                {
                    b.Property<int>("BorderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BorderId"));

                    b.Property<string>("BorderCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BorderId");

                    b.ToTable("Borders", (string)null);
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountryId"));

                    b.Property<string>("Capital")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CommonName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.CountryBorder", b =>
                {
                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("BorderId")
                        .HasColumnType("int");

                    b.HasKey("CountryId", "BorderId");

                    b.HasIndex("BorderId");

                    b.ToTable("CountryBorder");
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.CountryBorder", b =>
                {
                    b.HasOne("Viva.Geo.API.DataAccess.DataAccessModels.Border", "Border")
                        .WithMany("CountryBorders")
                        .HasForeignKey("BorderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Viva.Geo.API.DataAccess.DataAccessModels.Country", "Country")
                        .WithMany("CountryBorders")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Border");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Border", b =>
                {
                    b.Navigation("CountryBorders");
                });

            modelBuilder.Entity("Viva.Geo.API.DataAccess.DataAccessModels.Country", b =>
                {
                    b.Navigation("CountryBorders");
                });
#pragma warning restore 612, 618
        }
    }
}
