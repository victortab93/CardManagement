using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CardManagementApi.Persistence
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Number)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.Fee).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                //entity.HasOne(d => d.Card)
                //    .WithMany(p => p.Payments)
                //    .HasForeignKey(d => d.CardId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_PaymentCard");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
