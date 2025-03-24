using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ResourceMonitor.Models;

namespace ResourceMonitor.Data;

public partial class MtaresourceMonitoringContext : DbContext
{
    public MtaresourceMonitoringContext()
    {
    }

    public MtaresourceMonitoringContext(DbContextOptions<MtaresourceMonitoringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Sqltable> Sqltables { get; set; }

    public virtual DbSet<Website> Websites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=mtadev;Database=MTAResourceMonitoring;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Process__3214EC27A6324F82");

            entity.ToTable("Process");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FriendlyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");
            entity.Property(e => e.LastEmailSent).HasColumnType("datetime");
            entity.Property(e => e.LastHealthy).HasColumnType("datetime");
            entity.Property(e => e.PreviousState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ProcessName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Recipients)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Server__3214EC274F3944ED");

            entity.ToTable("Server");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FriendlyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");
            entity.Property(e => e.LastEmailSent).HasColumnType("datetime");
            entity.Property(e => e.LastHealthy).HasColumnType("datetime");
            entity.Property(e => e.PreviousState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Recipients)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service__3214EC2748BE5BDC");

            entity.ToTable("Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FriendlyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");
            entity.Property(e => e.LastEmailSent).HasColumnType("datetime");
            entity.Property(e => e.LastHealthy).HasColumnType("datetime");
            entity.Property(e => e.PreviousState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Recipients)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Sqltable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SQLTable__3214EC27115E99BB");

            entity.ToTable("SQLTable");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DatabaseName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FriendlyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");
            entity.Property(e => e.LastEmailSent).HasColumnType("datetime");
            entity.Property(e => e.LastHealthy).HasColumnType("datetime");
            entity.Property(e => e.PreviousState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Recipients)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Website>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Website__3214EC27F01CC2F8");

            entity.ToTable("Website");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CurrentState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FriendlyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastCheck).HasColumnType("datetime");
            entity.Property(e => e.LastEmailSent).HasColumnType("datetime");
            entity.Property(e => e.LastHealthy).HasColumnType("datetime");
            entity.Property(e => e.PreviousState)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Recipients)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ServerName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WebsiteName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
