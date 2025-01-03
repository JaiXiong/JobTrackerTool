using Microsoft.EntityFrameworkCore;
using JobData.Entities;

namespace JobTracker.API.Tool.DbData
{
    public interface IJobProfileContext
    {
        DbSet<EmployerProfile> Employers { get; set; }
        DbSet<JobProfile> JobProfiles { get; set; }
        DbSet<JobAction> JobActions { get; set; }
        DbSet<Detail> Details { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}