using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
//using MTAIntranetAngular.API.Data.Models;

namespace MTAIntranetAngular.API.Data.Models;

public partial class MtaticketsContext : DbContext
{
    public MtaticketsContext()
    {
    }

    public MtaticketsContext(DbContextOptions<MtaticketsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApprovalState> ApprovalStates { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Impact> Impacts { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketSubType> TicketSubTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:MTADevConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApprovalState>(entity =>
        {
            entity.HasKey(e => e.ApprovalStateId).HasName("PK__Approval__BE49849A4BBCDE95");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BC67E2300");
        });

        modelBuilder.Entity<Impact>(entity =>
        {
            entity.HasKey(e => e.ImpactId).HasName("PK__Impact__2297C5DD82CEA044");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Ticket__712CC627CA4BA2BF");

            entity.Property(e => e.ApprovedBy).HasDefaultValueSql("('NA')");
            entity.Property(e => e.ReasonForRejection).HasDefaultValueSql("('NA')");

            entity.HasOne(d => d.ApprovalState).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__Approval__3AB788A8");

            entity.HasOne(d => d.Category).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__Category__36E6F7C4");

            entity.HasOne(d => d.Impact).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__ImpactID__38CF4036");

            entity.HasOne(d => d.SubType).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ticket__SubTypeI__37DB1BFD");
        });

        modelBuilder.Entity<TicketSubType>(entity =>
        {
            entity.HasKey(e => e.TicketSubTypeId).HasName("PK__TicketSu__D4EA1061D2F99C5A");

            entity.HasOne(d => d.Category).WithMany(p => p.TicketSubTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TicketSub__Categ__3039FA35");
        });

        //base.OnModelCreating(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
