using System;
using System.Collections.Generic;
using GymTrainingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace GymTrainingSystem.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GymClient> GymClients { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<MemeberFee> MemeberFees { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-EE3AP9K;Database=GymSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GymClient>(entity =>
        {
            entity.HasKey(e => e.ClientId);

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.GymLogo).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Fees).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.Gender).HasMaxLength(250);
            entity.Property(e => e.JoiningDate).HasColumnType("datetime");
            entity.Property(e => e.MemberShipEnd).HasColumnType("datetime");
            entity.Property(e => e.MemberShipStart).HasColumnType("datetime");
            entity.Property(e => e.MemberShipType).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Members)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Members_GymClients");
        });

        modelBuilder.Entity<MemeberFee>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Fees).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .HasColumnName("status");
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.MemeberFees)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_MemeberFees_GymClients");

            entity.HasOne(d => d.Member).WithMany(p => p.MemeberFees)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MemeberFees_Members");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(250);
            entity.Property(e => e.TimeStamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.Users)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Users_GymClients");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
