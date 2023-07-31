﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MilkManagementSystem.Models;

#nullable disable

namespace MilkManagementSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230105122332_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MilkManagementSystem.Models.Packets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AddedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<double>("TotalQuantity")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Packets");
                });

            modelBuilder.Entity("MilkManagementSystem.Models.SoldPackets", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DropOff_Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int?>("PacketsId")
                        .HasColumnType("int");

                    b.Property<int>("PickedBy")
                        .HasColumnType("int");

                    b.Property<string>("Pickup_Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PricePerPacket")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SoldDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SoldDateString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SoldTo")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Total_Distance")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PacketsId");

                    b.HasIndex("PickedBy");

                    b.HasIndex("SoldTo");

                    b.ToTable("SoldPackets");
                });

            modelBuilder.Entity("MilkManagementSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contact_No")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsActive")
                        .HasColumnType("int");

                    b.Property<int>("IsRegular")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<double?>("Salary")
                        .HasColumnType("float");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MilkManagementSystem.Models.SoldPackets", b =>
                {
                    b.HasOne("MilkManagementSystem.Models.Packets", null)
                        .WithMany("SoldPackets")
                        .HasForeignKey("PacketsId");

                    b.HasOne("MilkManagementSystem.Models.User", "Rider")
                        .WithMany("Packets_Sold")
                        .HasForeignKey("PickedBy");

                    b.HasOne("MilkManagementSystem.Models.User", "Customer")
                        .WithMany("Packets_Bought")
                        .HasForeignKey("SoldTo");

                    b.Navigation("Customer");

                    b.Navigation("Rider");
                });

            modelBuilder.Entity("MilkManagementSystem.Models.Packets", b =>
                {
                    b.Navigation("SoldPackets");
                });

            modelBuilder.Entity("MilkManagementSystem.Models.User", b =>
                {
                    b.Navigation("Packets_Bought");

                    b.Navigation("Packets_Sold");
                });
#pragma warning restore 612, 618
        }
    }
}
