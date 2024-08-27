using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RepairTracker.DBModels
{

    public partial class GameRepairContext : DbContext
    {
        public GameRepairContext()
        {
        }

        public GameRepairContext(DbContextOptions<GameRepairContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Owner> Owners { get; set; }

        public virtual DbSet<Part> Parts { get; set; }

        public virtual DbSet<Repair> Repairs { get; set; }

        public virtual DbSet<RepairNote> RepairNotes { get; set; }

        public virtual DbSet<RepairPart> RepairParts { get; set; }

        public virtual DbSet<Technician> Technicians { get; set; }

        public virtual DbSet<WallTime> WallTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("server=DEVX1\\SQLEXPRESS01;database=RepairTracker;Trusted_Connection=True;Trust Server Certificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).HasColumnName("GameId");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.Property(e => e.OwnerId).HasColumnName("OwnerId");
            });

            modelBuilder.Entity<Part>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_Parts_CategoryID");

                entity.Property(e => e.PartId).HasColumnName("PartId");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryId");
                entity.Property(e => e.Cost).HasColumnType("decimal(7, 2)");
                entity.Property(e => e.Sale).HasColumnType("decimal(7, 2)");

                entity.HasOne(d => d.Category).WithMany(p => p.Parts).HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Repair>(entity =>
            {
                entity.HasIndex(e => e.GameId, "IX_Repairs_GameID");

                entity.HasIndex(e => e.TechnicianId, "IX_Repairs_TechnicianID");

                entity.Property(e => e.RepairId).HasColumnName("RepairId");
                entity.Property(e => e.GameId).HasColumnName("GameId");
                entity.Property(e => e.OwnerId).HasColumnName("OwnerId");
                entity.Property(e => e.TechnicianId).HasColumnName("TechnicianId");

                entity.HasOne(d => d.Game).WithMany(p => p.Repairs).HasForeignKey(d => d.GameId);

                entity.HasOne(d => d.Owner).WithMany(p => p.Repairs)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Repairs_Owners");

                entity.HasOne(d => d.Technician).WithMany(p => p.Repairs).HasForeignKey(d => d.TechnicianId);
            });

            modelBuilder.Entity<RepairNote>(entity =>
            {
                entity.HasIndex(e => e.RepairId, "IX_RepairNotes_RepairID");

                entity.Property(e => e.RepairNoteId).HasColumnName("RepairNoteID");
                entity.Property(e => e.RepairId).HasColumnName("RepairId");
                entity.Property(e => e.TechnicianId).HasColumnName("TechnicianId");

                entity.HasOne(d => d.Repair).WithMany(p => p.RepairNotes).HasForeignKey(d => d.RepairId);

                entity.HasOne(d => d.Technician).WithMany(p => p.RepairNotes)
                    .HasForeignKey(d => d.TechnicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RepairNotes_Technicians");
            });

            modelBuilder.Entity<RepairPart>(entity =>
            {
                entity.HasIndex(e => e.PartId, "IX_RepairParts_PartID");

                entity.HasIndex(e => e.RepairId, "IX_RepairParts_RepairID");

                entity.Property(e => e.RepairPartId).HasColumnName("RepairPartID");
                entity.Property(e => e.PartId).HasColumnName("PartId");
                entity.Property(e => e.RepairId).HasColumnName("RepairId");

                entity.HasOne(d => d.Part).WithMany(p => p.RepairParts).HasForeignKey(d => d.PartId);

                entity.HasOne(d => d.Repair).WithMany(p => p.RepairParts).HasForeignKey(d => d.RepairId);
            });

            modelBuilder.Entity<Technician>(entity =>
            {
                entity.Property(e => e.TechnicianId).HasColumnName("TechnicianId");
                entity.Property(e => e.HourlyRate).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<WallTime>(entity =>
            {
                entity.HasIndex(e => e.RepairId, "IX_WallTimes_RepairID");

                entity.Property(e => e.WallTimeId).HasColumnName("WallTimeID");
                entity.Property(e => e.RepairId).HasColumnName("RepairId");
                entity.Property(e => e.TechnicianId).HasColumnName("TechnicianId");

                entity.HasOne(d => d.Repair).WithMany(p => p.WallTimes).HasForeignKey(d => d.RepairId);

                entity.HasOne(d => d.Technician).WithMany(p => p.WallTimes)
                    .HasForeignKey(d => d.TechnicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WallTimes_Technicians");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}