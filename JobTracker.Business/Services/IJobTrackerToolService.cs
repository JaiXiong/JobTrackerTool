using JobData.Entities;
using Utils.Operations;

public interface IJobTrackerToolService
{
    Task<OperationResult> AddJobProfile(JobProfile jobProfile);
    Task<OperationResult> AddWorkAction(JobAction action);
    Task<OperationResult> AddUserProfile(UserProfile profile);
    Task<OperationResult> AddEmployerProfile(EmployerProfile employerProfile, Guid jobProfileId);
    Task<OperationResult> AddEmployerProfile(EmployerProfile employerProfile);
    Task<OperationResult> AddJobAction(Guid employerProfileId, JobAction action);
    Task<OperationResult> AddDetail(Guid employerProfileId, Detail detail);
    Task<JobProfile> GetJobProfile(Guid jobProfileId);
    Task<IEnumerable<JobProfile>> GetJobProfiles(Guid userProfileId);
    Task<IEnumerable<JobProfile>> GetAllJobProfiles();
    Task<EmployerProfile> GetEmployerProfile(Guid employerProfileId);
    Task<IEnumerable<EmployerProfile>> GetEmployerProfiles(Guid jobProfileId);
    Task<IEnumerable<EmployerProfile>> GetAllEmployerProfiles();
    Task<int> GetTotalEmployerCount(Guid jobProfileId);
    Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize);
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
