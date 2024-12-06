using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;
using System.Resources;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext, IJobTrackerToolService
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
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync(c => c.Id == jobProfile.UserProfileId);

            if (user == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("UserProfileNull"));
            }

            var jobProfileExist = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.ProfileName == jobProfile.ProfileName);
            
            if (jobProfileExist != null)
            {
                throw new ArgumentException(_resourceManager.GetString("JobProfileExist"));
            }

            jobProfile.Id = Guid.NewGuid();
            jobProfile.Date = DateTime.Now;
            jobProfile.LatestUpdate = DateTime.Now;

            _dbContext.JobProfiles.Add(jobProfile);
            await _dbContext.SaveChangesAsync();
        }
        public void AddWorkAction(JobAction workAction)
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
            userProfile.LatestUpdate = DateTime.Now;

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
        public async Task AddEmployerProfile(EmployerProfile employerProfile)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == employerProfile.JobProfileId);
            
            if (jobProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }

            employerProfile.Id = Guid.NewGuid();
            employerProfile.Date = DateTime.Now;
            employerProfile.LatestUpdate = DateTime.Now;

            if (employerProfile.Result != null)
            {
                employerProfile.Result.Id = Guid.NewGuid();
                employerProfile.Result.EmployerProfileId = employerProfile.Id;
                employerProfile.Result.Date = DateTime.Now;
                employerProfile.Result.LatestUpdate = DateTime.Now;
            }

            if (employerProfile.Detail != null)
            {
                employerProfile.Detail.Id = Guid.NewGuid();
                employerProfile.Detail.EmployerProfileId = employerProfile.Id;
                employerProfile.Detail.Date = DateTime.Now;
                employerProfile.Detail.LatestUpdate = DateTime.Now;
            }

            _dbContext.Employers.Add(employerProfile);
            await _dbContext.SaveChangesAsync();

        }

        public async Task AddJobAction(Guid employerProfileId, JobAction jobAction)
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

            jobAction.Id = Guid.NewGuid();
            _dbContext.JobActions.Add(jobAction);
            employerProfile.Result = jobAction;
            _dbContext.Employers.Update(employerProfile);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddDetail(Guid employerProfileId, Detail detail)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == detail.EmployerProfileId);
            //if (employerProfile != null) {
            //    throw new ArgumentException(_resourceManager.GetString("DetailExist"));
            //}
            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }

            detail.Id = Guid.NewGuid();
            
            _dbContext.Details.Add(detail);
            employerProfile.Detail = detail;
            _dbContext.Employers.Update(employerProfile);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<JobProfile> GetJobProfile(Guid jobProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles
                .Include(c => c.Employers)
                .FirstOrDefaultAsync(c => c.Id == jobProfileId);
            if (jobProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            return jobProfile;
        }

        public async Task<IEnumerable<JobProfile>> GetJobProfiles(Guid userProfileId)
        {
            var jobProfiles = await _dbContext.JobProfiles
                .Include(c => c.Employers)
                .Where(c => c.UserProfileId == userProfileId)
                .ToListAsync();
            if (jobProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            return jobProfiles;
        }

        public async Task<IEnumerable<JobProfile>> GetAllJobProfiles()
        {
            var jobProfiles = await _dbContext.JobProfiles
                .Include(c => c.Employers)
                .ToListAsync();
            if (jobProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            return jobProfiles;
        }

        public async Task<EmployerProfile> GetEmployerProfile(Guid jobProfileId, Guid employerProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfileId);
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);

            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            return employerProfile;
        }

        public async Task<IEnumerable<EmployerProfile>> GetEmployerProfiles(Guid jobProfileId)
        {
            var employerProfiles = await _dbContext.Employers
                .Include(c => c.Result)
                .Include(c => c.Detail)
                .Where(c => c.JobProfileId == jobProfileId)
                .ToListAsync();

            if (employerProfiles == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            return employerProfiles;
        }

        public async Task<IEnumerable<EmployerProfile>> GetAllEmployerProfiles()
        {
            var employerProfiles = await _dbContext.Employers
                .Include(c => c.Result)
                .Include(c => c.Detail)
                .ToListAsync();
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

        public async Task<JobAction> GetJobAction(Guid employerProfileId)
        {
            var jobAction = await _dbContext.JobActions.FirstOrDefaultAsync(c => c.EmployerProfileId == employerProfileId);
            if (jobAction == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobActionNull"));
            }
            return jobAction;
        }

        public async Task<IEnumerable<JobAction>> GetAllJobActions()
        {
            var jobActions = await _dbContext.JobActions.ToListAsync();
            if (jobActions == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobActionNull"));
            }
            return jobActions;
        }

        public async Task<Detail> GetDetail(Guid employerProfileId)
        {
            var detail = await _dbContext.Details.FirstOrDefaultAsync(c => c.EmployerProfileId == employerProfileId);
            if (detail == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("DetailNull"));
            }
            return detail;
        }

        public async Task<IEnumerable<Detail>> GetAllDetails()
        {
            var details = await _dbContext.Details.ToListAsync();
            if (details == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("DetailNull"));
            }
            return details;
        }

        public async Task UpdateEmployerProfile(EmployerProfile updatedProfile)
        {
            var existingProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);
            if (existingProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }

            existingProfile.Name = updatedProfile.Name;
            existingProfile.Title = updatedProfile.Title;
            existingProfile.Address = updatedProfile.Address;
            existingProfile.City = updatedProfile.City;
            existingProfile.State = updatedProfile.State;
            existingProfile.Zip = updatedProfile.Zip;
            existingProfile.Phone = updatedProfile.Phone;
            existingProfile.Email = updatedProfile.Email;
            existingProfile.Website = updatedProfile.Website;
            existingProfile.LatestUpdate = DateTime.Now;

            if (existingProfile.Result != null)
            {
                existingProfile.Result.LatestUpdate = DateTime.Now;
                existingProfile.Result = updatedProfile.Result;
            }

            if (existingProfile.Detail != null)
            {
                existingProfile.Detail.LatestUpdate = DateTime.Now;
                existingProfile.Detail = updatedProfile.Detail;
            }

            _dbContext.Employers.Update(existingProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateJobProfile(JobProfile updatedProfile)
        {
            var existingProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);
            if (existingProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            existingProfile.ProfileName = updatedProfile.ProfileName;
            existingProfile.LatestUpdate = DateTime.Now;
            _dbContext.JobProfiles.Update(existingProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateJobAction(JobAction updatedProfile)
        {
            var existingProfile = await _dbContext.JobActions.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);
            if (existingProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobActionNull"));
            }
            existingProfile.Action = updatedProfile.Action;
            existingProfile.Method = updatedProfile.Method;
            existingProfile.ActionResult = updatedProfile.ActionResult;
            existingProfile.LatestUpdate = DateTime.Now;
            _dbContext.JobActions.Update(existingProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDetail(Detail updatedProfile)
        {
            var existingProfile = await _dbContext.Details.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);
            if (existingProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("DetailNull"));
            }
            existingProfile.Comments = updatedProfile.Comments;
            existingProfile.Updates = updatedProfile.Updates;
            existingProfile.LatestUpdate = DateTime.Now;
            _dbContext.Details.Update(existingProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteJobProfile(Guid jobProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfileId);
            if (jobProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("JobProfileNull"));
            }
            _dbContext.JobProfiles.Remove(jobProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployerProfile(Guid employerProfileId)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);
            if (employerProfile == null)
            {
                throw new ArgumentNullException(_resourceManager.GetString("EmployerProfileNull"));
            }
            _dbContext.Employers.Remove(employerProfile);
            await _dbContext.SaveChangesAsync();
        }
    }
}
