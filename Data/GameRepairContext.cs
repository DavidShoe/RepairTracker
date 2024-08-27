﻿//using RepairTracker.DBModels;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Reflection.Emit;

//namespace RepairTracker.Data
//{
//    public class GameRepairContext : DbContext
//    {
//        public GameRepairContext()
//                   {
//        }

//        public GameRepairContext(DbContextOptions<GameRepairContext> options)
//            : base(options)
//        {
//        }
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//            => optionsBuilder.UseSqlServer("server=DEVX1\\SQLEXPRESS01;database=RepairTracker; Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");


//        public DbSet<Owner> Owners { get; set; }
//        public DbSet<Technician> Technicians { get; set; }
//        public DbSet<Part> Parts { get; set; }
//        public DbSet<Game> Games { get; set; }
//        public DbSet<Repair> Repairs { get; set; }
//        public DbSet<RepairPart> RepairParts { get; set; }
//        public DbSet<Category> Categories { get; set; }
//        public DbSet<RepairNote> RepairNotes { get; set; }
//        public DbSet<WallTime> WallTimes { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Owner>()
//                .HasMany(o => o.Games);

//            modelBuilder.Entity<Technician>()
//                .HasMany(t => t.Repairs)
//                .WithOne(r => r.Technician)
//                .HasForeignKey(r => r.TechnicianId);

//            modelBuilder.Entity<Game>()
//                .HasMany(g => g.Repairs)
//                .WithOne(r => r.Game)
//                .HasForeignKey(r => r.GameId);

//            modelBuilder.Entity<Repair>()
//                .HasMany(r => r.RepairParts)
//                .WithOne(rp => rp.Repair)
//                .HasForeignKey(rp => rp.RepairId);

//            modelBuilder.Entity<Part>()
//                .HasMany(p => p.RepairParts)
//                .WithOne(rp => rp.Part)
//                .HasForeignKey(rp => rp.PartId);

//            modelBuilder.Entity<Category>()
//                .HasMany(c => c.Parts)
//                .WithOne(p => p.Category)
//                .HasForeignKey(p => p.CategoryId);

//            modelBuilder.Entity<WallTime>()
//                .HasOne(wt => wt.Repair)
//                .WithMany(r => r.WallTimes)
//                .HasForeignKey(wt => wt.RepairId);
//        }
//    }
//}
