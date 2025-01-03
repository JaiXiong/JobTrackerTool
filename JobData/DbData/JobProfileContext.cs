using Microsoft.EntityFrameworkCore;
using JobData.Entities;

namespace JobTracker.API.Tool.DbData
{
    public class JobProfileContext: DbContext, IJobProfileContext
    {
        public JobProfileContext(DbContextOptions<JobProfileContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserProfile>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<JobProfile>()
                .HasIndex(j => j.ProfileName)
                .IsUnique();
        }
        public DbSet<EmployerProfile> Employers{ get; set; }
        public DbSet<JobProfile> JobProfiles { get; set; }
        public DbSet<JobAction> JobActions { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
