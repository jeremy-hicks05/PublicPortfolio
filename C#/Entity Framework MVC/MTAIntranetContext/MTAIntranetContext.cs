namespace MTAIntranet.Shared
{
    using Microsoft.EntityFrameworkCore;
    public partial class MTAIntranetContext : DbContext
    {
        // FLTAS003
        public MTAIntranetContext()
        {
        }

        public MTAIntranetContext(DbContextOptions<MTAIntranetContext> options)
            :base(options)
        {
        }

        // properties
        public virtual DbSet<Pulloff>? Pulloffs { get; set; }
        public virtual DbSet<MasterRoute>? MasterRoutes { get; set; }

        public virtual DbSet<TraineeEval>? TraineeEvals { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TraineeEval>(entity =>
            {

                entity.ToTable("TestDailyTraineeEval", "dbo");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}