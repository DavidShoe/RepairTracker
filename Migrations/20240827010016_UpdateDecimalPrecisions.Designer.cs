﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepairTracker.Data;

#nullable disable

namespace RepairTracker.Migrations
{
    [DbContext(typeof(GameRepairContext))]
    [Migration("20240827010016_UpdateDecimalPrecisions")]
    partial class UpdateDecimalPrecisions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GameRepairApp.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"));

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GameRepairApp.Models.Game", b =>
                {
                    b.Property<int>("GameID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GameID"));

                    b.Property<string>("GameName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OwnerID")
                        .HasColumnType("int");

                    b.HasKey("GameID");

                    b.HasIndex("OwnerID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GameRepairApp.Models.Owner", b =>
                {
                    b.Property<int>("OwnerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OwnerID"));

                    b.Property<string>("ContactInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OwnerID");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("GameRepairApp.Models.Part", b =>
                {
                    b.Property<int>("PartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PartID"));

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasPrecision(7, 2)
                        .HasColumnType("decimal(7,2)");

                    b.Property<string>("PartName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Sale")
                        .HasPrecision(7, 2)
                        .HasColumnType("decimal(7,2)");

                    b.Property<string>("Supplier")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PartID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Parts");
                });

            modelBuilder.Entity("GameRepairApp.Models.Repair", b =>
                {
                    b.Property<int>("RepairID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RepairID"));

                    b.Property<DateOnly?>("FinishedDate")
                        .HasColumnType("date");

                    b.Property<int>("GameID")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("ReceivedDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TechnicianID")
                        .HasColumnType("int");

                    b.HasKey("RepairID");

                    b.HasIndex("GameID");

                    b.HasIndex("TechnicianID");

                    b.ToTable("Repairs");
                });

            modelBuilder.Entity("GameRepairApp.Models.RepairNote", b =>
                {
                    b.Property<int>("RepairNoteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RepairNoteID"));

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NoteDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RepairID")
                        .HasColumnType("int");

                    b.HasKey("RepairNoteID");

                    b.HasIndex("RepairID");

                    b.ToTable("RepairNotes");
                });

            modelBuilder.Entity("GameRepairApp.Models.RepairPart", b =>
                {
                    b.Property<int>("RepairPartID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RepairPartID"));

                    b.Property<int>("PartID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RepairID")
                        .HasColumnType("int");

                    b.HasKey("RepairPartID");

                    b.HasIndex("PartID");

                    b.HasIndex("RepairID");

                    b.ToTable("RepairParts");
                });

            modelBuilder.Entity("GameRepairApp.Models.Technician", b =>
                {
                    b.Property<int>("TechnicianID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TechnicianID"));

                    b.Property<decimal>("HourlyRate")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("TechnicianName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TechnicianID");

                    b.ToTable("Technicians");
                });

            modelBuilder.Entity("GameRepairApp.Models.WallTime", b =>
                {
                    b.Property<int>("WallTimeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WallTimeID"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RepairID")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TechnicianID")
                        .HasColumnType("int");

                    b.HasKey("WallTimeID");

                    b.HasIndex("RepairID");

                    b.ToTable("WallTimes");
                });

            modelBuilder.Entity("GameRepairApp.Models.Game", b =>
                {
                    b.HasOne("GameRepairApp.Models.Owner", "Owner")
                        .WithMany("Games")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("GameRepairApp.Models.Part", b =>
                {
                    b.HasOne("GameRepairApp.Models.Category", "Category")
                        .WithMany("Parts")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("GameRepairApp.Models.Repair", b =>
                {
                    b.HasOne("GameRepairApp.Models.Game", "Game")
                        .WithMany("Repairs")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameRepairApp.Models.Technician", "Technician")
                        .WithMany("Repairs")
                        .HasForeignKey("TechnicianID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("Technician");
                });

            modelBuilder.Entity("GameRepairApp.Models.RepairNote", b =>
                {
                    b.HasOne("GameRepairApp.Models.Repair", "Repair")
                        .WithMany("RepairNotes")
                        .HasForeignKey("RepairID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repair");
                });

            modelBuilder.Entity("GameRepairApp.Models.RepairPart", b =>
                {
                    b.HasOne("GameRepairApp.Models.Part", "Part")
                        .WithMany("RepairParts")
                        .HasForeignKey("PartID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameRepairApp.Models.Repair", "Repair")
                        .WithMany("RepairParts")
                        .HasForeignKey("RepairID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Part");

                    b.Navigation("Repair");
                });

            modelBuilder.Entity("GameRepairApp.Models.WallTime", b =>
                {
                    b.HasOne("GameRepairApp.Models.Repair", "Repair")
                        .WithMany("WallTimes")
                        .HasForeignKey("RepairID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repair");
                });

            modelBuilder.Entity("GameRepairApp.Models.Category", b =>
                {
                    b.Navigation("Parts");
                });

            modelBuilder.Entity("GameRepairApp.Models.Game", b =>
                {
                    b.Navigation("Repairs");
                });

            modelBuilder.Entity("GameRepairApp.Models.Owner", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("GameRepairApp.Models.Part", b =>
                {
                    b.Navigation("RepairParts");
                });

            modelBuilder.Entity("GameRepairApp.Models.Repair", b =>
                {
                    b.Navigation("RepairNotes");

                    b.Navigation("RepairParts");

                    b.Navigation("WallTimes");
                });

            modelBuilder.Entity("GameRepairApp.Models.Technician", b =>
                {
                    b.Navigation("Repairs");
                });
#pragma warning restore 612, 618
        }
    }
}
