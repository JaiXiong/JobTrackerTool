using Microsoft.EntityFrameworkCore;
using JobData.Entities;

namespace JobTracker.API.Tool.DbData
{
    public class JobProfileContext: DbContext
    {
        public JobProfileContext(DbContextOptions<JobProfileContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<EmployerProfile> Employers{ get; set; }
        public DbSet<JobProfile> JobProfiles { get; set; }
        public DbSet<JobAction> JobActions { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
