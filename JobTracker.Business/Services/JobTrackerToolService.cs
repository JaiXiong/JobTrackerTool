using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext
    {
        private readonly JobProfileContext _dbContext;
        public JobTrackerToolService(JobProfileContext context)
        {
            _dbContext = context;
        }
        public async Task AddJobProfile(JobProfile jobProfile)
        {
            jobProfile.Id = Guid.NewGuid();
            jobProfile.Date = DateTime.Now;
            jobProfile.LastestUpdate = DateTime.Now;
            _dbContext.JobProfiles.Add(jobProfile);
            await _dbContext.SaveChangesAsync();
        }

        public void AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId)
        {
            employerProfile.Id = Guid.NewGuid();
            employerProfile.JobProfileId = jobProfileId;
            _dbContext.Employers.Add(employerProfile);
            _dbContext.SaveChanges();
        }

        public void AddWorkaction(JobAction workAction)
        {
            workAction.Id = Guid.NewGuid();
            workAction.Action = "Action";
            workAction.Method = "Method";
            workAction.ActionResult = "ActionResult";
            _dbContext.SaveChanges();
        }

        public async Task AddUserDetail(Guid jobprofileid)
        {
            var jobProfile = _dbContext.JobProfiles.FirstOrDefault(c => c.Id == jobprofileid);

            if (jobProfile == null)
            {
                throw Exceptio;
            }
            jobProfile.User.Id = Guid.NewGuid();

            await _dbContext.SaveChangesAsync();
        }
        public string GetEmployerName(Guid id)
        {
            var name = "";
            return name;
        }
    }
}
