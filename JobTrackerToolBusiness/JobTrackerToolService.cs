using Microsoft.EntityFrameworkCore;
using JobEntities.Entities;

namespace JobTracker.Business
{
    public class JobTrackerToolService : DbContext
    {
        public JobTrackerToolService()
        {

        }

        public DbSet<JobEntities.Entities.JobProfile> JobProfiles { get; set; }
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

    //public class JobProfile
    //{
    //    public Guid Id { get; set; }
    //    public string Employer { get; set; }
    //    // Add other properties as needed
    //}
}
