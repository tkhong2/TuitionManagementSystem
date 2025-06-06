using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TuitionManagement.Models;

public partial class HpsvContext : DbContext
{
    public HpsvContext()
    {
    }

    public HpsvContext(DbContextOptions<HpsvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Tuition> Tuitions { get; set; }
    public object TuiTions { get; internal set; }
    public virtual DbSet<User> Users { get; set; }
    public object Tuition { get; internal set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=KHACHONG\\SQLEXPRESS;Database=HPSV;Integrated Security=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC0767B04D63");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Class).HasMaxLength(100);
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC071C20FA5C");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasMaxLength(100);
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TuitionId).HasMaxLength(100);

            entity.HasOne(d => d.Student).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Stude__4E88ABD4");

            entity.HasOne(d => d.Tuition).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TuitionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Tuiti__4F7CD00D");
        });

        modelBuilder.Entity<Tuition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tuitions__3214EC07AC2AA6F8");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.AmountDue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Semester).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StudentId).HasMaxLength(100);

            entity.HasOne(d => d.Student).WithMany(p => p.Tuitions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tuitions__Studen__4BAC3F29");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC077F2DB23D");

            entity.Property(e => e.Id).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
