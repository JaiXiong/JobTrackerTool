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
            var exist = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfile.Id);
            if (exist != null)
            {
                throw new ArgumentException(_resourceManager.GetString("JobProfileExist"));
            }

            jobProfile.Id = jobProfile.Id == Guid.Empty ? Guid.NewGuid() : jobProfile.Id;
            jobProfile.Date = DateTime.Now;
            jobProfile.LastestUpdate = DateTime.Now;
            _dbContext.JobProfiles.Add(jobProfile);
            await _dbContext.SaveChangesAsync();
        }

        public void AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId)
        {
            var exist = _dbContext.Employers.FirstOrDefault(c => c.Id == employerProfile.Id);
            if (exist != null)
            {
                throw new ArgumentException(_resourceManager.GetString("EmployerProfileExist"));
            }

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

            userProfile.Id = userProfile.Id == Guid.Empty ? Guid.NewGuid() : userProfile.Id;
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

        public async Task AddEmployerProfile(EmployerProfile employerProfile)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == employerProfile.JobProfileId);
            //if (jobProfile != null)
            //{
            //    throw new ArgumentException(_resourceManager.GetString("EmployerProfileExist"));
            //}
            if (jobProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }

            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }

            employerProfile.Id = employerProfile.Id == Guid.Empty ? Guid.NewGuid() : employerProfile.Id;
            employerProfile.JobProfileId = jobProfile.Id == Guid.Empty ? Guid.NewGuid() : jobProfile.Id;
            _dbContext.Employers.Add(employerProfile);
            await _dbContext.SaveChangesAsync();

        }

        public async Task AddActionResult(JobAction jobAction)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == jobAction.EmployerProfileId);
            //if (employerProfile  != null) 
            //{
            //    throw new ArgumentException(_resourceManager.GetString("ActionResultExist"));
            //}
            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }

            jobAction.Id = jobAction.Id == Guid.Empty ? Guid.NewGuid() : jobAction.Id;
            jobAction.EmployerProfileId = jobAction.EmployerProfileId == Guid.Empty ? Guid.NewGuid() : jobAction.EmployerProfileId;
            _dbContext.JobActions.Add(jobAction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddDetail(Detail detail)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == detail.EmployerProfileId);
            //if (employerProfile != null) {
            //    throw new ArgumentException(_resourceManager.GetString("DetailExist"));
            //}
            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }

            detail.Id = detail.Id == Guid.Empty ? Guid.NewGuid() : detail.Id;
            detail.EmployerProfileId = detail.EmployerProfileId == Guid.Empty ? Guid.NewGuid() : detail.EmployerProfileId;
            _dbContext.Details.Add(detail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<JobProfile> GetJobProfile(Guid jobProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfileId);
            if (jobProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            return jobProfile;
        }

        public async Task<IEnumerable<JobProfile>> GetJobProfiles(Guid userProfileId)
        {
            var jobProfiles = await _dbContext.JobProfiles.Where(c => c.UserProfileId == userProfileId).ToListAsync();
            if (jobProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            return jobProfiles;
        }

        public async Task<EmployerProfile> GetEmployer(Guid jobProfileId, Guid employerProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfileId);
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);

            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            return employerProfile;
        }

        public async Task<IEnumerable<EmployerProfile>> GetEmployers(Guid jobProfileId)
        {
            var employerProfiles = await _dbContext.Employers.Where(c => c.JobProfileId == jobProfileId).ToListAsync();
            if (employerProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            return employerProfiles;
        }

        public async Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            //var employerProfiles = await _dbContext.Employers.Where(c => c.JobProfileId == userProfileId)
            //.Skip(pageIndex * pageSize)
            //.Take(pageSize).ToListAsync();
            var employerProfiles = await _dbContext.Employers.Where(c => c.JobProfileId == jobProfileId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToListAsync();
            if (employerProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            return employerProfiles;
        }
        public string GetEmployerName(Guid id)
        {
            var name = "";
            return name;
        }
    }
}
