using Microsoft.EntityFrameworkCore;

namespace JobTracker.Business
{
    public class JobTrackerToolService : DbContext
    {
        public JobTrackerToolService()
        {

        }

        public DbSet<JobProfile> JobProfiles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use a connection string to connect to your database
            optionsBuilder.UseSqlServer("YourConnectionStringHere");
        }

        public string GetEmployerName(Guid id)
        {
            var jobProfile = JobProfiles.FirstOrDefault(x => x.Id == id);
            return jobProfile?.Employer;
        }
    }
}
