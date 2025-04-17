using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using Utils.Operations;
using Microsoft.Extensions.Logging;
using Utils.CustomExceptions;
using JobData.Common;
using Microsoft.Extensions.Caching.Memory;

namespace JobTracker.Business.Services
{
    public class JobTrackerToolService : DbContext, IJobTrackerToolService
    {
        private readonly JobProfileContext _dbContext;
        private readonly ILogger<JobTrackerToolService> _logger;
        private readonly IMemoryCache _cache;
        private ResxFormat _resx;
        ResourceManager _resourceManager;
        public JobTrackerToolService(JobProfileContext context, ILogger<JobTrackerToolService> logger, IMemoryCache cache)
        {
            _dbContext = context;
            _logger = logger;
            _cache = cache;
            _resourceManager = new ResourceManager("JobTracker.Business.JobTrackerBusinessErrors", typeof(JobTrackerToolService).Assembly);
            _resx = new ResxFormat(_resourceManager);
        }
        public async Task<OperationResult> AddJobProfile(JobProfile jobProfile)
        {
            var jobProfileExist = await _dbContext.JobProfiles.AnyAsync(c => c.ProfileName == jobProfile.ProfileName);

            if (jobProfileExist)
            {
                return OperationResult.CreateFailure(_resx.Create("JobProfileExist"));
            }

            jobProfile.Id = Guid.NewGuid();
            jobProfile.Date = DateTime.Now;
            jobProfile.LatestUpdate = DateTime.Now;

            _dbContext.JobProfiles.Add(jobProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Adding job profile successfully.");
        }
        public async Task<OperationResult> AddWorkAction(JobAction workAction)
        {
            try
            {
                var employerProfile = await _dbContext.Employers
                    .Include(c => c.Result)
                    .FirstOrDefaultAsync(c => c.Id == workAction.EmployerProfileId);

                if (employerProfile == null)
                {
                    return OperationResult.CreateFailure(_resx.Create("EmployerProfileNotExist"));
                }

                workAction.Id = Guid.NewGuid();
                workAction.Action = "Action";
                workAction.Method = "Method";

                _dbContext.JobActions.Add(workAction);
                employerProfile.Result = workAction;
                _dbContext.Employers.Update(employerProfile);

                await _dbContext.SaveChangesAsync(); 

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
                    return OperationResult.CreateFailure(_resx.Create("UserExist"));
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
        public async Task<OperationResult> AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId)
        {
            try
            {
                var exist = await _dbContext.Employers.AnyAsync(c => c.Id == employerProfile.Id);

                if (exist)
                {
                    return OperationResult.CreateFailure(_resx.Create("EmployerProfileExist"));
                }

                employerProfile.Id = Guid.NewGuid();
                employerProfile.JobProfileId = jobProfileId;
                _dbContext.Employers.Add(employerProfile);
                await _dbContext.SaveChangesAsync(); // Fix: Changed SaveChanges to SaveChangesAsync

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
            var exist = await _dbContext.JobProfiles.AnyAsync(c => c.Id == employerProfile.Id);

            if (exist)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerProfileExist"));
            }

            //employerProfile.Id = Guid.NewGuid();
            employerProfile.Id = employerProfile.Id != Guid.Empty ? employerProfile.Id : Guid.NewGuid();
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

        public async Task<OperationResult> AddJobAction(Guid employerProfileId, JobAction jobAction)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == jobAction.EmployerProfileId);

            if (employerProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerProfileNotExist"));
            }

            jobAction.Id = Guid.NewGuid();
            _dbContext.JobActions.Add(jobAction);
            employerProfile.Result = jobAction;
            _dbContext.Employers.Update(employerProfile);

            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Added employer action successfully.");
        }

        public async Task<OperationResult> AddDetail(Guid employerProfileId, Detail detail)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == detail.EmployerProfileId);

            if (employerProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerProfileNotExist"));
            }

            detail.Id = Guid.NewGuid();

            _dbContext.Details.Add(detail);
            employerProfile.Detail = detail;
            _dbContext.Employers.Update(employerProfile);

            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Added employer detail successfully.");
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

        public async Task<EmployerProfile> GetEmployerProfile(Guid employerProfileId, DownloadOptions downloadOptions)
        {
            EmployerProfile? employerProfile;

            if (downloadOptions.Include == DownloadType.Include)
            {
                employerProfile = await _dbContext.Employers
                    .Include(c => c.Result)
                    .Include(c => c.Detail)
                    .FirstOrDefaultAsync(c => c.Id == employerProfileId);
            }
            else
            {
                employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);
            }

            if (employerProfile == null)
            {
                throw new BusinessException(_resx.Create("EmployerProfileNotExist"));
            }

            return employerProfile;
        }

        public async Task<IEnumerable<EmployerProfile>> GetEmployerProfiles(Guid jobProfileId, DownloadOptions downloadOptions)
        {
            IEnumerable<EmployerProfile> employerProfiles;

            if (downloadOptions.Include == DownloadType.Include)
            {
                employerProfiles = await _dbContext.Employers
                    .Include(c => c.Result)
                    .Include(c => c.Detail)
                    .Where(c => c.JobProfileId == jobProfileId)
                    .ToListAsync();
            }
            else
            {
                employerProfiles = await _dbContext.Employers
                    .Where(c => c.JobProfileId == jobProfileId)
                    .ToListAsync();
            }

            if (employerProfiles == null || !employerProfiles.Any())
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

        //public async Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        //{
        //    var employerProfiles = await _dbContext.Employers.Where(c => c.JobProfileId == jobProfileId)
        //        .Skip(pageIndex * pageSize)
        //        .Take(pageSize).ToListAsync();

        //    if (employerProfiles == null)
        //    {
        //        throw new BusinessException(_resx.Create("EmployerPagingError"));
        //    }

        //    return employerProfiles;
        //}

        public async Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            // Generate cache keys for current, previous, and next pages
            var currentPageKey = $"EmployerPagingData_{jobProfileId}_Page_{pageIndex}_Size_{pageSize}";
            var previousPageKey = $"EmployerPagingData_{jobProfileId}_Page_{pageIndex - 1}_Size_{pageSize}";
            var nextPageKey = $"EmployerPagingData_{jobProfileId}_Page_{pageIndex + 1}_Size_{pageSize}";

            if (!_cache.TryGetValue(currentPageKey, out IEnumerable<EmployerProfile>? currentPageData))
            {
                currentPageData = await _dbContext.Employers
                    .Where(c => c.JobProfileId == jobProfileId)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (currentPageData == null || !currentPageData.Any())
                {
                    throw new BusinessException(_resx.Create("EmployerPagingError"));
                }

                // Cache the current page data with a TTL of 5 minutes
                _cache.Set(currentPageKey, currentPageData, TimeSpan.FromMinutes(5));
            }

            // Preload and cache the previous page data
            if (pageIndex > 0 && !_cache.TryGetValue(previousPageKey, out _))
            {
                var previousPageData = await _dbContext.Employers
                    .Where(c => c.JobProfileId == jobProfileId)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (previousPageData != null && previousPageData.Any())
                {
                    _cache.Set(previousPageKey, previousPageData, TimeSpan.FromMinutes(5));
                }
            }

            // Preload and cache the next page data
            if (!_cache.TryGetValue(nextPageKey, out _))
            {
                var nextPageData = await _dbContext.Employers
                    .Where(c => c.JobProfileId == jobProfileId)
                    .Skip((pageIndex + 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                if (nextPageData != null && nextPageData.Any())
                {
                    _cache.Set(nextPageKey, nextPageData, TimeSpan.FromMinutes(5));
                }
            }

            return currentPageData;
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

        public async Task<OperationResult> UpdateEmployerProfile(EmployerProfile updatedProfile)
        {
            var existingProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);

            if (existingProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerProfileNotExist"));
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

            return OperationResult.CreateSuccess("Employer profile updated successfully");
        }

        public async Task<OperationResult> UpdateJobProfile(JobProfile updatedProfile)
        {
            var existingProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);

            if (existingProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("JobProfileNotExist"));
            }

            existingProfile.ProfileName = updatedProfile.ProfileName;
            existingProfile.LatestUpdate = DateTime.Now;
            _dbContext.JobProfiles.Update(existingProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Job profile updated successfully");
        }

        public async Task<OperationResult> UpdateJobAction(JobAction updatedProfile)
        {
            var existingProfile = await _dbContext.JobActions.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);

            if (existingProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("JobActionNotExist"));
            }
            existingProfile.Action = updatedProfile.Action;
            existingProfile.Method = updatedProfile.Method;
            existingProfile.ActionResult = updatedProfile.ActionResult;
            existingProfile.LatestUpdate = DateTime.Now;

            _dbContext.JobActions.Update(existingProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Updated job action successfully.");
        }

        public async Task<OperationResult> UpdateDetail(Detail updatedProfile)
        {
            var existingProfile = await _dbContext.Details.FirstOrDefaultAsync(c => c.Id == updatedProfile.Id);

            if (existingProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerDetailsNotExist"));
            }
            existingProfile.Comments = updatedProfile.Comments;
            existingProfile.Updates = updatedProfile.Updates;
            existingProfile.LatestUpdate = DateTime.Now;

            _dbContext.Details.Update(existingProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Updated employer details successfully.");
        }

        public async Task<OperationResult> DeleteJobProfile(Guid jobProfileId)
        {
            var jobProfile = await _dbContext.JobProfiles.FirstOrDefaultAsync(c => c.Id == jobProfileId);

            if (jobProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("JobProfileNotExist"));
            }

            _dbContext.JobProfiles.Remove(jobProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Deleted job profile successfully");
        }

        public async Task<OperationResult> DeleteEmployerProfile(Guid employerProfileId)
        {
            var employerProfile = await _dbContext.Employers.FirstOrDefaultAsync(c => c.Id == employerProfileId);

            if (employerProfile == null)
            {
                return OperationResult.CreateFailure(_resx.Create("EmployerProfileNotExist"));
            }

            _dbContext.Employers.Remove(employerProfile);
            await _dbContext.SaveChangesAsync();

            return OperationResult.CreateSuccess("Deleted employer profile successfully.");
        }

        public async Task<OperationResult> UploadEmployerProfiles(List<EmployerProfile> employerProfiles, Guid jobProfileId)
        {
            if (employerProfiles == null)
            {
                return OperationResult.CreateFailure(_resx.Create("UploadNotExist"));
            }

            var exist = await _dbContext.JobProfiles.AnyAsync(c => c.Id == jobProfileId);

            if (!exist)
            {
                return OperationResult.CreateFailure(_resx.Create("JobProfileNotExist"));
            }

            foreach(var profile in employerProfiles)
            {
                await AddEmployerProfile(profile);
            }

            return OperationResult.CreateSuccess("Upload employer profiles successfully.");
        }

        public Task<OperationResult> ExportXMLEmployerProfiles()
        {
            throw new NotImplementedException();
        }
    }
}
