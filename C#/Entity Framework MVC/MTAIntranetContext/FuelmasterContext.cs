using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MTAIntranet.Shared
{
    // SQL 02
    public partial class FuelmasterContext : DbContext
    {
        public FuelmasterContext()
        {
        }

        public FuelmasterContext(DbContextOptions<FuelmasterContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<EqMain> EqMains { get; set; } = null!;


        //public virtual DbSet<MaxQueue>? MaxQueues { get; set; }

        [DisplayName("Main Transactions")]
        public virtual DbSet<MainTrans>? MainTrans { get; set; }

        public virtual DbSet<User>? User { get; set; }
        public virtual DbSet<Site>? Site { get; set; }
        public virtual DbSet<ProductConfig>? ProductConfig { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MaxQueue>(entity =>
            //{
            //    entity.HasNoKey();

            //    entity.ToTable("MAXQ_ErrorHandler", "maxqueue");
            //});

            modelBuilder.Entity<MainTrans>(
                mt =>
                {
                    mt.HasNoKey();
                });
            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
