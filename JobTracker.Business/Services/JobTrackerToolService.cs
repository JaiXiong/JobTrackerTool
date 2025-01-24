using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Versioning;
using System.Resources;
using Utils.Operations;
using Org.BouncyCastle.Security;
using iTextSharp.text.log;
using Microsoft.Extensions.Logging;
using static iTextSharp.text.pdf.PdfSigLockDictionary;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Utils.CustomExceptions;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext, IJobTrackerToolService
    {
        private readonly JobProfileContext _dbContext;
        private readonly ILogger<JobTrackerToolService> _logger;
        private ResxFormat _resx;
        ResourceManager _resourceManager;
        public JobTrackerToolService(JobProfileContext context, ILogger<JobTrackerToolService> logger)
        {
            _dbContext = context;
            _logger = logger;
            _resourceManager = new ResourceManager("JobTracker.Business.JobTrackerBusinessErrors", typeof(JobTrackerToolService).Assembly);
            _resx = new ResxFormat(_resourceManager);
        }
        public async Task<OperationResult> AddJobProfile(JobProfile jobProfile)
        {
            try
            {
                var user = await _dbContext.UserProfiles.AnyAsync(c => c.Id == jobProfile.UserProfileId);

                if (!user)
                {
                    return OperationResult.CreateFailure(string.Format(_resourceManager.GetString("AddProfileError")));
                }

                var jobProfileExist = await _dbContext.JobProfiles.AnyAsync(c => c.ProfileName == jobProfile.ProfileName);

                if (jobProfileExist)
                {
                    return OperationResult.CreateSuccess(string.Format(_resourceManager.GetString("JobProfileExist"), jobProfile.ProfileName));
                }

                jobProfile.Id = Guid.NewGuid();
                jobProfile.Date = DateTime.Now;
                jobProfile.LatestUpdate = DateTime.Now;

                _dbContext.JobProfiles.Add(jobProfile);
                await _dbContext.SaveChangesAsync();

                return OperationResult.CreateSuccess("Adding job profile successfully."); //{ Success = true, Message = "Adding job profile successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding job profile");
                return OperationResult.CreateFailure("An error occured while adding job profile"); //{ Success = false, Message = "An error occured while adding job profile" };
            }
        }
        public async Task<OperationResult> AddWorkAction(JobAction workAction)
        {
            try
            {
                workAction.Id = Guid.NewGuid();
                workAction.Action = "Action";
                workAction.Method = "Method";
                workAction.ActionResult = "ActionResult";
                _dbContext.SaveChanges();

                return OperationResult.CreateSuccess("Added work action successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding workaction");
                return OperationResult.CreateFailure("An error occured while adding workaction");
            }
        }

        public async Task<OperationResult> AddUserProfile(UserProfile userProfile)
        {
            try
            {
                var exist = await _dbContext.UserProfiles.AnyAsync(c => c.Name == userProfile.Name);

                if (exist)
                {
                    return OperationResult.CreateFailure((string.Format(_resourceManager.GetString("UserExist"), userProfile.Name)));
                }

                userProfile.Id = Guid.NewGuid();
                userProfile.Date = DateTime.Now;
                userProfile.LatestUpdate = DateTime.Now;

                _dbContext.UserProfiles.Add(userProfile);
                await _dbContext.SaveChangesAsync();

                return OperationResult.CreateSuccess("Added user successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding user profile.");
                return OperationResult.CreateFailure("An error occured while adding user profile.");
            }
        }
        //public void ValidateNewUser(UserProfile userProfile)
        //{
        //    if (userProfile == null)
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserProfileNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.Name))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserNameNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.Email))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserEmailNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.Phone))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserPhoneNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.Address))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserAddressNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.City))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserCityNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.State))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserStateNull"));
        //    }
        //    if (string.IsNullOrEmpty(userProfile.Zip))
        //    {
        //        throw new ArgumentNullException(_resourceManager.GetString("UserZipNull"));
        //    }
        //}
        public async Task<OperationResult> AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId)
        {
            try
            {
                var exist = await _dbContext.Employers.AnyAsync(c => c.Id == employerProfile.Id);

                if (exist)
                {
                    return OperationResult.CreateFailure(string.Format(_resourceManager.GetString("EmployerProfileExist"), employerProfile.Name));
                }

                employerProfile.Id = Guid.NewGuid();
                employerProfile.JobProfileId = jobProfileId;
                _dbContext.Employers.Add(employerProfile);
                _dbContext.SaveChanges();

                return OperationResult.CreateSuccess("Added employer profile successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding employer profile");
                return OperationResult.CreateFailure("An error occurred while adding employer profile");
            }
        }
        public async Task<OperationResult> AddEmployerProfile(EmployerProfile employerProfile)
        {
            try
            {
                var jobProfile = await _dbContext.JobProfiles.AnyAsync(c => c.Id == employerProfile.JobProfileId);

                if (!jobProfile)
                {
                    return OperationResult.CreateFailure(string.Format(_resourceManager.GetString("JobProfileNotExist"), employerProfile.Name));
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

                return OperationResult.CreateSuccess("Added employer profile successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred adding employer profile");
                return OperationResult.CreateFailure("An error occurred adding employer profile");
            }

        }

        public async Task<OperationResult> AddJobAction(Guid employerProfileId, JobAction jobAction)
        {
            try
            {
                var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == jobAction.EmployerProfileId);

                if (employerProfile == null)
                {
                    return OperationResult.CreateFailure(string.Format(_resourceManager.GetString("EmployerProfileNotExist"), employerProfileId));
                }

                jobAction.Id = Guid.NewGuid();
                _dbContext.JobActions.Add(jobAction);
                employerProfile.Result = jobAction;
                _dbContext.Employers.Update(employerProfile);

                await _dbContext.SaveChangesAsync();

                return OperationResult.CreateSuccess("Added employer action successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred adding employer action.");
                return OperationResult.CreateFailure("An error occurred adding job action.");
            }
        }

        public async Task<OperationResult> AddDetail(Guid employerProfileId, Detail detail)
        {
            try
            {
                var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == detail.EmployerProfileId);

                if (employerProfile == null)
                {
                    return OperationResult.CreateFailure(string.Format(_resourceManager.GetString("EmployerProfileNotExist"), employerProfileId));
                }

                detail.Id = Guid.NewGuid();

                _dbContext.Details.Add(detail);
                employerProfile.Detail = detail;
                _dbContext.Employers.Update(employerProfile);

                await _dbContext.SaveChangesAsync();

                return OperationResult.CreateSuccess("Added employer detail successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred adding employer detail.");
                return OperationResult.CreateFailure("An error occurred adding employer detail.");
            }
        }

        public async Task<JobProfile> GetJobProfile(Guid jobProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles
                            .Include(c => c.Employers)
                            .FirstOrDefaultAsync(c => c.Id == jobProfileId);

            if (jobProfile == null)
            {
                throw new BusinessException(_resx.Create("JobProfileNotExist"));
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
                throw new BusinessException(_resx.Create("JobProfilesNotExist"));
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
                throw new BusinessException(_resx.Create("JobProfilesNotExist"));
            }

            return jobProfiles;
        }

        public async Task<EmployerProfile> GetEmployerProfile(Guid employerProfileId)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);

            if (employerProfile == null)
            {
                throw new BusinessException(_resx.Create("EmployerProfileNotExist"));
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
                throw new BusinessException(_resx.Create("EmployerProfilesNotExist"));
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
                throw new BusinessException(_resx.Create("EmployerProfilesNotExist"));
            }

            return employerProfiles;
        }

        public async Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            var employerProfiles = await _dbContext.Employers.Where(c => c.JobProfileId == jobProfileId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToListAsync();

            if (employerProfiles == null)
            {
                throw new BusinessException(_resx.Create("EmployerPagingError"));
            }

            return employerProfiles;
        }

        public async Task<int> GetTotalEmployerCount(Guid jobProfileId)
        {
            return await _dbContext.Employers.Where(c => c.JobProfileId == jobProfileId).CountAsync();
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
                throw new BusinessException(_resx.Create("EmployerActionsNotExist"));
            }

            return jobActions;
        }

        public async Task<Detail> GetDetail(Guid employerProfileId)
        {
            var detail = await _dbContext.Details.FirstOrDefaultAsync(c => c.EmployerProfileId == employerProfileId);

            if (detail == null)
            {
                throw new BusinessException(_resx.Create("EmployerDetailsNotExist"));
            }

            return detail;
        }

        public async Task<IEnumerable<Detail>> GetAllDetails()
        {
            var details = await _dbContext.Details.ToListAsync();

            if (details == null)
            {
                throw new BusinessException(_resx.Create("EmployerDetailsNotExist"));
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
