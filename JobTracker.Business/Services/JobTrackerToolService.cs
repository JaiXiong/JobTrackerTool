using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;
using System.Resources;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext
    {
        private readonly JobProfileContext _dbContext;
        ResourceManager _resourceManager;
        public JobTrackerToolService(JobProfileContext context, ResourceManager resourceManager)
        {
            _dbContext = context;
            _resourceManager = resourceManager;
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

        public async Task AddUserProfile(UserProfile userProfile)
        {
            var exist = await _dbContext.UserProfiles.FirstOrDefaultAsync(c => c.Name == userProfile.Name);
            if (exist != null)
            {
                throw new ArgumentException(_resourceManager.GetString("UserExist"));
            }

            userProfile.Id = Guid.NewGuid();
            userProfile.Date = DateTime.Now;
            userProfile.LastestUpdate = DateTime.Now;

            _dbContext.UserProfiles.Add(userProfile);
            await _dbContext.SaveChangesAsync();
        }
        public void ValidateNewUser(UserProfile userProfile)
        {
            if (userProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserProfileNull"));
            }
            if (string.IsNullOrEmpty(userProfile.Name))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserNameNull"));
            }
            if (string.IsNullOrEmpty(userProfile.Email))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserEmailNull"));
            }
            if (string.IsNullOrEmpty(userProfile.Phone))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserPhoneNull"));
            }
            if (string.IsNullOrEmpty(userProfile.Address))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserAddressNull"));
            }
            if (string.IsNullOrEmpty(userProfile.City))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserCityNull"));
            }
            if (string.IsNullOrEmpty(userProfile.State))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserStateNull"));
            }
            if (string.IsNullOrEmpty(userProfile.Zip))
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserZipNull"));
            }
        }
        public string GetEmployerName(Guid id)
        {
            var name = "";
            return name;
        }
    }
}
