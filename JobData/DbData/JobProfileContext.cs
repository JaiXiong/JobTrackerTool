using Microsoft.EntityFrameworkCore;
using JobData.Entities;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace JobTracker.API.Tool.DbData
{
    public class JobProfileContext : DbContext, IJobProfileContext
    {
        private readonly IConfiguration _configuration;

        public JobProfileContext(DbContextOptions<JobProfileContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public JobProfileContext(DbContextOptions<JobProfileContext> options) : base(options)
        {
            _configuration = null!; // Explicitly mark as non-null since this constructor does not initialize it.
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration != null)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString, options =>
                {
                    options.EnableRetryOnFailure(
                        maxRetryCount: 2,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            }
        }

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

        public DbSet<EmployerProfile> Employers { get; set; }
        public DbSet<JobProfile> JobProfiles { get; set; }
        public DbSet<JobAction> JobActions { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public virtual async Task<UserProfile> GetUserProfileAsync(Expression<Func<UserProfile, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await UserProfiles.FirstOrDefaultAsync(predicate, cancellationToken) ?? throw new InvalidOperationException("UserProfile not found");
        }
    }
}
