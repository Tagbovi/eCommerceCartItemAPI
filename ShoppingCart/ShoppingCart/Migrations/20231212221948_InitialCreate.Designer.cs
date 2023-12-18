﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShoppingCart.DBContext;

#nullable disable

namespace ShoppingCart.Migrations
{
    [DbContext(typeof(CartDBContext))]
    [Migration("20231212221948_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShoppingCart.Models.Cart", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ProductId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<long>("Quantity")
                        .HasColumnType("bigint");

                    b.HasKey("ProductId");

                    b.ToTable("Cartdb");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            Name = "Adidas Shoe",
                            Price = 200.0,
                            Quantity = 1L
                        });
                });

            modelBuilder.Entity("ShoppingCart.Models.CartAndPriceItemTotal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TotalCartItems")
                        .HasColumnType("integer");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("CartPriceTotaldb");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            TotalCartItems = 0,
                            TotalPrice = 0.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
