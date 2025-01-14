using JobData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Business.Services
{
    public interface IJobTrackerToolService
    {
        Task AddJobProfile(JobProfile jobProfile);
        void AddWorkAction(JobAction action);
        Task AddUserProfile(UserProfile profile);
        void ValidateNewUser(UserProfile userProfile);
        void AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId);
        Task AddEmployerProfile(EmployerProfile employerProfile);
        Task AddJobAction(Guid employerProfileId, JobAction action);
        Task AddDetail(Guid employerProfileId, Detail detail);
        Task<JobProfile> GetJobProfile(Guid jobProfileId);
        Task<IEnumerable<JobProfile>> GetJobProfiles(Guid userProfileId);
        Task<IEnumerable<JobProfile>> GetAllJobProfiles();
        Task<EmployerProfile> GetEmployerProfile(Guid jobProfileId, Guid employerProfileId);
        Task<IEnumerable<EmployerProfile>> GetEmployerProfiles(Guid jobProfileId);
        Task<IEnumerable<EmployerProfile>> GetAllEmployerProfiles();
        Task<int> GetTotalEmployerCount(Guid jobProfileId);
        Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId,  int pageIndex, int pageSize);
        Task<JobAction> GetJobAction(Guid employerProfileId);
        Task<IEnumerable<JobAction>> GetAllJobActions();
        Task<Detail> GetDetail(Guid employerProfileId);
        Task<IEnumerable<Detail>> GetAllDetails();
        Task UpdateEmployerProfile(EmployerProfile updatedProfile);
        Task UpdateJobProfile(JobProfile updatedProfile);
        Task UpdateJobAction(JobAction updatedProfile);
        Task UpdateDetail(Detail updatedProfile);
        Task DeleteJobProfile(Guid jobProfileId);
        Task DeleteEmployerProfile(Guid employerProfileId);
    }
}
