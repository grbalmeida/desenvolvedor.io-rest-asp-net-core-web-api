﻿// <auto-generated />
using System;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevIO.Data.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20191231021020_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyFullMvcApp.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Complement")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<Guid>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("MyFullMvcApp.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(1000)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<decimal>("Price");

                    b.Property<Guid>("SupplierId");

                    b.HasKey("Id");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MyFullMvcApp.Models.Supplier", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("varchar(14)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.Property<int>("SupplierType");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("MyFullMvcApp.Models.Address", b =>
                {
                    b.HasOne("MyFullMvcApp.Models.Supplier", "Supplier")
                        .WithOne("Address")
                        .HasForeignKey("MyFullMvcApp.Models.Address", "SupplierId");
                });

            modelBuilder.Entity("MyFullMvcApp.Models.Product", b =>
                {
                    b.HasOne("MyFullMvcApp.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId");
                });
#pragma warning restore 612, 618
        }
    }
}
