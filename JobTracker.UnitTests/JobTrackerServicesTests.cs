using JobData.Common;
using JobData.Entities;
using JobTracker.API.Tool.DbData;
using JobTracker.Business.Business;
using JobTracker.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Resources;
using System.Text;
using Utils.CustomExceptions;
using Utils.Operations;

namespace JobTracker.UnitTests
{
    public class JobTrackerServicesTests
    {
        private readonly JobTrackerToolService _jobTrackerService;
        // Change the type of _mockDbContext to Mock<IJobProfileContext> to match the initialization
        private readonly Mock<IJobTrackerContext> _mockDbContext;
        private readonly Mock<ILogger<JobTrackerToolService>> _mockLogger;
        private readonly Mock<IMemoryCache> _mockCache;
        private readonly Mock<ICacheEntry> _mockCacheEntry;
        private readonly Mock<ResourceManager> _mockResourceManager;
        private readonly JobTrackerToolBusiness _jobTrackerBusiness;

        // Update the constructor to use the correct type
        public JobTrackerServicesTests()
        {
            _mockDbContext = new Mock<IJobTrackerContext>();
            _mockLogger = new Mock<ILogger<JobTrackerToolService>>();
            _mockCache = new Mock<IMemoryCache>();
            _mockCacheEntry = new Mock<ICacheEntry>();
            _mockResourceManager = new Mock<ResourceManager>("JobTracker.Business.JobTrackerBusinessErrors", typeof(JobTrackerToolService).Assembly);

            // Setup cache mock
            _mockCache.Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(_mockCacheEntry.Object);
            _mockCacheEntry.Setup(x => x.Dispose());

            // Initialize service with mocked dependencies
            _jobTrackerService = new JobTrackerToolService(_mockDbContext.Object, _mockLogger.Object, _mockCache.Object);

            // Initialize business class for testing business operations
            _jobTrackerBusiness = new JobTrackerToolBusiness(_mockLogger.Object);
        }

        [Fact]
        public async Task AddJobProfile_ShouldReturnSuccess_WhenProfileDoesNotExist()
        {
            // Arrange
            var jobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                ProfileName = "New Job Profile",
                UserProfileId = Guid.NewGuid()
            };

            //_mockDbContext.Setup(db => db.JobProfiles.AnyAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<JobProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(false);

            var jobProfiles = new List<JobProfile>().AsQueryable();
            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(jobProfiles);

            // Act
            var result = await _jobTrackerService.AddJobProfile(jobProfile);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Adding job profile successfully", result.Message);
            _mockDbContext.Verify(db => db.JobProfiles.Add(It.IsAny<JobProfile>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddJobProfile_ShouldReturnFailure_WhenProfileExists()
        {
            // Arrange
            var jobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                ProfileName = "Existing Job Profile",
                UserProfileId = Guid.NewGuid()
            };

            //_mockDbContext.Setup(db => db.JobProfiles.AnyAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<JobProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(true);

            var existingJobProfiles = new List<JobProfile>
            {
                new JobProfile { Id = jobProfile.Id, ProfileName = jobProfile.ProfileName, UserProfileId = jobProfile.UserProfileId }
            }.AsQueryable();

            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(existingJobProfiles);

            _mockResourceManager.Setup(r => r.GetString("JobProfileExist"))
                .Returns("Job Profile already exists");

            // Act
            var result = await _jobTrackerService.AddJobProfile(jobProfile);

            // Assert
            Assert.False(result.Success);
            _mockDbContext.Verify(db => db.JobProfiles.Add(It.IsAny<JobProfile>()), Times.Never);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetJobProfile_ShouldReturnProfile_WhenProfileExists()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var jobProfile = new JobProfile
            {
                Id = jobProfileId,
                ProfileName = "Test Job Profile",
                UserProfileId = Guid.NewGuid(),
                Employers = new List<EmployerProfile>()
            };

            var jobProfiles = new List<JobProfile> { jobProfile }.AsQueryable();

            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(jobProfiles);

            // Act
            var result = await _jobTrackerService.GetJobProfile(jobProfileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(jobProfileId, result.Id);
            Assert.Equal("Test Job Profile", result.ProfileName);
        }

        [Fact]
        public async Task GetJobProfile_ShouldThrowException_WhenProfileDoesNotExist()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var emptyJobProfiles = new List<JobProfile>().AsQueryable();

            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(emptyJobProfiles);

            _mockResourceManager.Setup(r => r.GetString("JobProfileNotExist"))
                .Returns("Job Profile does not exist");

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() => _jobTrackerService.GetJobProfile(jobProfileId));
        }

        [Fact]
        public async Task GetJobProfiles_ShouldReturnProfiles_WhenProfilesExistForUser()
        {
            // Arrange
            var userProfileId = Guid.NewGuid();
            var jobProfiles = new List<JobProfile>
            {
                new JobProfile { Id = Guid.NewGuid(), ProfileName = "Profile 1", UserProfileId = userProfileId },
                new JobProfile { Id = Guid.NewGuid(), ProfileName = "Profile 2", UserProfileId = userProfileId }
            }.AsQueryable();

            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(jobProfiles);

            // Act
            var result = await _jobTrackerService.GetJobProfiles(userProfileId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, profile => Assert.Equal(userProfileId, profile.UserProfileId));
        }

        [Fact]
        public async Task AddEmployerProfile_ShouldReturnSuccess_WhenProfileDoesNotExist()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = jobProfileId,
                Name = "Test Employer",
                Email = "employer@test.com"
            };

            //_mockDbContext.Setup(db => db.Employers.AnyAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<EmployerProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(false);

            // Create an empty collection since we want to indicate no employer exists yet
            var employers = new List<EmployerProfile>().AsQueryable();

            // Setup the DbSet using ReturnsDbSet
            _mockDbContext.Setup(db => db.Employers).ReturnsDbSet(employers);

            // Setup JobProfiles for the check in the AddEmployerProfile method
            var jobProfiles = new List<JobProfile>().AsQueryable();
            _mockDbContext.Setup(db => db.JobProfiles)
                .ReturnsDbSet(jobProfiles);

            // Act
            var result = await _jobTrackerService.AddEmployerProfile(employerProfile);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Added employer profile successfully", result.Message);
            _mockDbContext.Verify(db => db.Employers.Add(It.IsAny<EmployerProfile>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddEmployerProfile_WithJobProfileId_ShouldReturnSuccess_WhenProfileDoesNotExist()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = jobProfileId,
                Name = "Test Employer",
                Email = "employer@test.com"
            };

            //_mockDbContext.Setup(db => db.Employers.AnyAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<EmployerProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(false);

            var employers = new List<EmployerProfile>().AsQueryable();
            _mockDbContext.Setup(db => db.Employers).ReturnsDbSet(employers);

            var jobProfiles = new List<JobProfile>
            {
                new JobProfile { Id = jobProfileId, ProfileName = "Test Job Profile", UserProfileId = Guid.NewGuid() }
            }.AsQueryable();
            _mockDbContext.Setup(db => db.JobProfiles).ReturnsDbSet(jobProfiles);

            // Act
            var result = await _jobTrackerService.AddEmployerProfile(employerProfile, jobProfileId);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Added employer profile successfully", result.Message);
            _mockDbContext.Verify(db => db.Employers.Add(It.IsAny<EmployerProfile>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployerProfiles_ShouldReturnProfiles_WhenProfilesExistForJobProfile()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile { Id = Guid.NewGuid(), JobProfileId = jobProfileId, Name = "Employer 1" },
                new EmployerProfile { Id = Guid.NewGuid(), JobProfileId = jobProfileId, Name = "Employer 2" }
            }.AsQueryable();

            var downloadOptions = new DownloadOptions { Include = DownloadType.Include };

            _mockDbContext.Setup(db => db.Employers)
                .ReturnsDbSet(employerProfiles);

            // Act
            var result = await _jobTrackerService.GetEmployerProfiles(jobProfileId, downloadOptions);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, profile => Assert.Equal(jobProfileId, profile.JobProfileId));
        }

        [Fact]
        public async Task UpdateEmployerProfile_ShouldReturnSuccess_WhenProfileExists()
        {
            // Arrange
            var employerProfileId = Guid.NewGuid();
            var existingProfile = new EmployerProfile
            {
                Id = employerProfileId,
                JobProfileId = Guid.NewGuid(),
                Name = "Old Name",
                Email = "old@email.com"
            };

            var updatedProfile = new EmployerProfile
            {
                Id = employerProfileId,
                JobProfileId = existingProfile.JobProfileId,
                Name = "New Name",
                Email = "new@email.com"
            };

            //_mockDbContext.Setup(db => db.Employers.FirstOrDefaultAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<EmployerProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(existingProfile);

            var employerProfiles = new List<EmployerProfile> { existingProfile }.AsQueryable();
            _mockDbContext.Setup(db => db.Employers)
                .ReturnsDbSet(employerProfiles);

            // Act
            var result = await _jobTrackerService.UpdateEmployerProfile(updatedProfile);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Employer profile updated successfully", result.Message);
            Assert.Equal("New Name", existingProfile.Name);
            Assert.Equal("new@email.com", existingProfile.Email);
            _mockDbContext.Verify(db => db.Employers.Update(It.IsAny<EmployerProfile>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployerProfile_ShouldReturnSuccess_WhenProfileExists()
        {
            // Arrange
            var employerProfileId = Guid.NewGuid();
            var employerProfile = new EmployerProfile
            {
                Id = employerProfileId,
                JobProfileId = Guid.NewGuid(),
                Name = "Profile to Delete"
            };

            //_mockDbContext.Setup(db => db.Employers.FirstOrDefaultAsync(
            //    It.IsAny<System.Linq.Expressions.Expression<Func<EmployerProfile, bool>>>(),
            //    It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(employerProfile);

            var employerProfiles = new List<EmployerProfile> { employerProfile }.AsQueryable();
            _mockDbContext.Setup(db => db.Employers)
                .ReturnsDbSet(employerProfiles);

            // Act
            var result = await _jobTrackerService.DeleteEmployerProfile(employerProfileId);

            // Assert
            Assert.True(result.Success);
            Assert.Contains("Deleted employer profile successfully", result.Message);
            _mockDbContext.Verify(db => db.Employers.Remove(It.IsAny<EmployerProfile>()), Times.Once);
            _mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetEmployerPagingData_ShouldReturnPaginatedData_WhenDataExists()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            int pageIndex = 0;
            int pageSize = 2;

            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile { Id = Guid.NewGuid(), JobProfileId = jobProfileId, Name = "Employer 1" },
                new EmployerProfile { Id = Guid.NewGuid(), JobProfileId = jobProfileId, Name = "Employer 2" }
            }.AsQueryable();

            _mockDbContext.Setup(db => db.Employers)
                .ReturnsDbSet(employerProfiles);

            object expectedValue = employerProfiles.ToList();
            _mockCache.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
                .Returns(false);

            // Act
            var result = await _jobTrackerService.GetEmployerPagingData(jobProfileId, pageIndex, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void DownloadCsv_ShouldReturnCsvString_WithoutIncludingDetails()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                    Id = Guid.NewGuid(),
                    JobProfileId = jobProfileId,
                    Name = "Test Employer",
                    Email = "test@example.com",
                    Phone = "123-456-7890"
                }
            };
            var downloadOptions = new DownloadOptions { Include = DownloadType.None };

            // Act
            var result = _jobTrackerBusiness.DownloadCsv(jobProfileId, employerProfiles, downloadOptions);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Job Profile", result.ToString());
            Assert.Contains("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website", result.ToString());
            Assert.Contains("test@example.com", result.ToString());
        }

        [Fact]
        public void DownloadCsv_ShouldReturnCsvString_WithIncludingDetails()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                    Id = Guid.NewGuid(),
                    JobProfileId = jobProfileId,
                    Name = "Test Employer",
                    Email = "test@example.com",
                    Phone = "123-456-7890",
                    Result = new JobAction
                    {
                        Action = "Applied",
                        ActionResult = "Interview"
                    },
                    Detail = new Detail
                    {
                        Updates = "Following up next week",
                        Comments = "Initial contact made"
                    }
                }
            };
            var downloadOptions = new DownloadOptions { Include = DownloadType.Include };

            // Act
            var result = _jobTrackerBusiness.DownloadCsv(jobProfileId, employerProfiles, downloadOptions);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Job Profile", result.ToString());
            Assert.Contains("Id,Name,Title,Address,City,State,Zip,Phone,Email,Website,Action,ActionResult", result.ToString());
            Assert.Contains("test@example.com", result.ToString());
            Assert.Contains("Applied", result.ToString());
            Assert.Contains("Interview", result.ToString());
        }

        [Fact]
        public void DownloadPdf_ShouldReturnPdfBytes_WhenEmployerProfilesExist()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                    Id = Guid.NewGuid(),
                    JobProfileId = jobProfileId,
                    Name = "Test Employer",
                    Email = "test@example.com"
                }
            };
            var downloadOptions = new DownloadOptions { Include = DownloadType.None };

            // Act
            var result = _jobTrackerBusiness.DownloadPdf(jobProfileId, employerProfiles, downloadOptions);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void DownloadPdf_ShouldThrowArgumentException_WhenEmployerProfilesEmpty()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>();
            var downloadOptions = new DownloadOptions { Include = DownloadType.None };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _jobTrackerBusiness.DownloadPdf(jobProfileId, employerProfiles, downloadOptions));
        }

        [Fact]
        public void CreateZipFile_ShouldReturnZipBytes_WhenEmployerProfilesExist()
        {
            // Arrange
            var jobProfileId = Guid.NewGuid();
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                    Id = Guid.NewGuid(),
                    JobProfileId = jobProfileId,
                    Name = "Test Employer",
                    Email = "test@example.com"
                }
            };
            var downloadOptions = new DownloadOptions { Include = DownloadType.None };

            // Act
            var result = _jobTrackerBusiness.CreateZipFile(jobProfileId, employerProfiles, downloadOptions);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void ExportEmployerProfilesToExcel_ShouldReturnExcelBytes_WhenEmployerProfilesExist()
        {
            // Arrange
            var employerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                    Id = Guid.NewGuid(),
                    JobProfileId = Guid.NewGuid(),
                    Name = "Test Employer",
                    Email = "test@example.com",
                    Phone = "123-456-7890"
                }
            };

            // Act
            var result = _jobTrackerBusiness.ExportEmployerProfilesToExcel(employerProfiles);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Length > 0);
        }
    }
}