using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShopFloor.EFModels
{
    public partial class ShopFloorDBContext : DbContext
    {
        public ShopFloorDBContext()
        {
        }

        public ShopFloorDBContext(DbContextOptions<ShopFloorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionConfig> ActionConfigs { get; set; } = null!;
        public virtual DbSet<ProcessStep> ProcessSteps { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=ShopFloorDB;Trusted_Connection=True; trustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionConfig>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.ToTable("Action_config");

                entity.Property(e => e.Code)
                    .HasColumnType("numeric(18, 0)")
                    .HasColumnName("code");

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .HasColumnName("action");
            });

            modelBuilder.Entity<ProcessStep>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.ProcessName).HasMaxLength(50);

                entity.Property(e => e.Target).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagName);

                entity.ToTable("Tag");

                entity.Property(e => e.TagName)
                    .HasMaxLength(50)
                    .HasColumnName("Tag_Name");

                entity.Property(e => e.TagType).HasColumnName("Tag_Type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
