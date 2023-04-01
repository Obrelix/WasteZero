﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WasteZero.Data;

#nullable disable

namespace WasteZero.Migrations
{
    [DbContext(typeof(WasteZeroDbContext))]
    [Migration("20230401153310_M6")]
    partial class M6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("WasteZero.Models.ConsumedDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ParentAddedDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProductID")
                        .HasColumnType("TEXT");

                    b.Property<float?>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ProductID");

                    b.ToTable("ConsumedDetails", (string)null);
                });

            modelBuilder.Entity("WasteZero.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool?>("IsGlutenFree")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProductTypeID")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ProductTypeID");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("WasteZero.Models.ProductDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ExpirationDate")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProductID")
                        .HasColumnType("TEXT");

                    b.Property<float?>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductDetails", (string)null);
                });

            modelBuilder.Entity("WasteZero.Models.ProductType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasColumnName("ID")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Code")
                        .HasMaxLength(5)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes", (string)null);
                });

            modelBuilder.Entity("WasteZero.Models.ConsumedDetail", b =>
                {
                    b.HasOne("WasteZero.Models.Product", "Product")
                        .WithMany("ConsumedDetails")
                        .HasForeignKey("ProductID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WasteZero.Models.Product", b =>
                {
                    b.HasOne("WasteZero.Models.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeID");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("WasteZero.Models.ProductDetail", b =>
                {
                    b.HasOne("WasteZero.Models.Product", "Product")
                        .WithMany("Details")
                        .HasForeignKey("ProductID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("WasteZero.Models.Product", b =>
                {
                    b.Navigation("ConsumedDetails");

                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
