﻿// <auto-generated />
using System;
using ASPAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ASPAPI.Migrations
{
    [DbContext(typeof(TestDBContext))]
    [Migration("20230211103236_StoreProductId")]
    partial class StoreProductId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductCount")
                        .HasColumnType("integer");

                    b.Property<int?>("StoreProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("StoreProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.StoreProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("StoreCount")
                        .HasColumnType("integer");

                    b.Property<int>("StoreId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Order", b =>
                {
                    b.HasOne("ASPAPI.Models.DbEntities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.OrderItem", b =>
                {
                    b.HasOne("ASPAPI.Models.DbEntities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASPAPI.Models.DbEntities.StoreProduct", "StoreProduct")
                        .WithMany("OrderItems")
                        .HasForeignKey("StoreProductId");

                    b.Navigation("Order");

                    b.Navigation("StoreProduct");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.StoreProduct", b =>
                {
                    b.HasOne("ASPAPI.Models.DbEntities.Product", "Product")
                        .WithMany("StoreProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ASPAPI.Models.DbEntities.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.User", b =>
                {
                    b.HasOne("ASPAPI.Models.DbEntities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Product", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.Store", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.StoreProduct", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("ASPAPI.Models.DbEntities.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
