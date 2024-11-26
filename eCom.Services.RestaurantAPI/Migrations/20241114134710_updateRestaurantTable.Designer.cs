﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eCom.Services.RestaurantAPI.Data;

#nullable disable

namespace eCom.Services.RestaurantAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241114134710_updateRestaurantTable")]
    partial class updateRestaurantTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eCom.Services.RestaurantAPI.Models.Restaurant", b =>
                {
                    b.Property<int>("RestaurantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RestaurantId"));

                    b.Property<string>("RestaurantDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantLocation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RestaurantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("RestaurantRating")
                        .HasColumnType("float");

                    b.HasKey("RestaurantId");

                    b.ToTable("Restaurants");

                    b.HasData(
                        new
                        {
                            RestaurantId = 1,
                            RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            RestaurantLocation = "Da Nang",
                            RestaurantName = "Ramen Ichido",
                            RestaurantRating = 4.2999999999999998
                        },
                        new
                        {
                            RestaurantId = 2,
                            RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            RestaurantLocation = "Tokyo",
                            RestaurantName = "Udon Yukichi",
                            RestaurantRating = 4.5999999999999996
                        },
                        new
                        {
                            RestaurantId = 3,
                            RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            RestaurantLocation = "Fukuoka",
                            RestaurantName = "Gyudon",
                            RestaurantRating = 4.7000000000000002
                        },
                        new
                        {
                            RestaurantId = 4,
                            RestaurantDescription = " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.",
                            RestaurantLocation = "Ishikawa",
                            RestaurantName = "Tenpura",
                            RestaurantRating = 4.9000000000000004
                        });
                });
#pragma warning restore 612, 618
        }
    }
}