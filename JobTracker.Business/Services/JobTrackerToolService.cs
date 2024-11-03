using JobData.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext
    {
        public DbSet<JobProfile> JobProfiles { get; set; } // Define DbSet property
        public DbSet<EmployerProfile> EmployerProfiles { get; set; } // Define DbSet property
        public DbSet<JobAction> JobActions { get; set; } // Define DbSet property
        public void AddJobProfile(JobProfile jobProfile)
        {
            jobProfile.Id = Guid.NewGuid();
            jobProfile.Date = DateOnly.FromDateTime(DateTime.Now);
            //jobProfile.WorkAction = new JobAction();
            //jobProfile.Employer = new EmployerProfile();
            JobProfiles.Add(jobProfile);
            SaveChanges();
        }

        public void AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId)
        {
            employerProfile.Id = Guid.NewGuid();
            employerProfile.JobProfileId = jobProfileId;
            EmployerProfiles.Add(employerProfile);
            SaveChanges();
        }

        public void AddWorkaction(JobAction workAction)
        {
            workAction.Id = Guid.NewGuid();
            workAction.Action = "Action";
            workAction.Method = "Method";
            workAction.ActionResult = "ActionResult";
            SaveChanges();
        }
        public string GetEmployerName(Guid id)
        {
            var name = "";
            return name;
        }
    }
}
