using JobData.Entities;
using JobTracker.Business.Services;
using Moq;
using Microsoft.Extensions.Logging;
using JobTracker.API.Tool.DbData;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Utils.Encryption;
using AutoMapper;
using JobTracker.Business.Business;
using Utils.Operations;
using JobData.Common;

namespace JobTracker.API.tool.Controllers.Tests
{
    [TestClass()]
    public class JobTrackerControllerTests
    {
        private readonly Mock<ILogger<JobTrackerController>> _mockLogger;
        private readonly Mock<IJobProfileContext> _mockProfileContext;
        private readonly Mock<ResourceManager> _mockResourceManager;
        private readonly Mock<Encryption> _mockEncryption;
        private readonly Mock<IJobTrackerToolService> _mockServices;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJobTrackerToolBusiness> _mockJobTrackerToolBusiness;
        private readonly JobTrackerController _controller;

        public JobTrackerControllerTests()
        {
            _mockLogger = new Mock<ILogger<JobTrackerController>>();
            _mockProfileContext = new Mock<IJobProfileContext>();
            _mockResourceManager = new Mock<ResourceManager>();
            _mockEncryption = new Mock<Encryption>();
            _mockServices = new Mock<IJobTrackerToolService>();
            _mockMapper = new Mock<IMapper>();
            _mockJobTrackerToolBusiness = new Mock<IJobTrackerToolBusiness>();
            _controller = new JobTrackerController(_mockLogger.Object, _mockServices.Object, _mockEncryption.Object, _mockJobTrackerToolBusiness.Object, _mockMapper.Object);
        }

        #region Job Profile Tests
        [TestMethod()]
        public async Task GetJobProfileTest()
        {
            //Arrange
            var expectedJobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.NewGuid(),
                ProfileName = "Test Profile",
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };

            _mockServices.Setup(s => s.GetJobProfile(expectedJobProfile.Id))
                .ReturnsAsync(expectedJobProfile);

            //Act
            var result = await _controller.GetJobProfile(expectedJobProfile.Id) as OkObjectResult;

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(expectedJobProfile.Id, result.Id);
            //Assert.AreEqual(expectedJobProfile.ProfileName, result.ProfileName);
            Assert.AreEqual(200, result.StatusCode);
            var jobProfile = result.Value as JobProfile;
            Assert.IsNotNull(jobProfile);
            Assert.AreEqual(expectedJobProfile.Id, jobProfile.Id);
            Assert.AreEqual(expectedJobProfile.ProfileName, jobProfile.ProfileName);
        }

        [TestMethod()]
        public async Task GetJobProfileTestFails()
        {
            var jobProfileId = Guid.NewGuid();

            _mockServices.Setup(s => s.GetJobProfile(jobProfileId))
                .ThrowsAsync(new ArgumentException("Job Profile not found"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.GetJobProfile(jobProfileId));
        }

        [TestMethod()]
        public async Task CreateJobProfile()
        {
            var expectedJobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.NewGuid(),
                ProfileName = "Test Profile",
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };

            _mockServices.Setup(s => s.AddJobProfile(expectedJobProfile))
                .ReturnsAsync(OperationResult.CreateSuccess("Job profile created successfully."));

            var result = await _controller.CreateJobProfile(expectedJobProfile) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            var jobProfile = result.Value as JobProfile;
            Assert.IsNotNull(jobProfile);
            Assert.AreEqual(expectedJobProfile.Id, jobProfile.Id);
            Assert.AreEqual(expectedJobProfile.ProfileName, jobProfile.ProfileName);
            _mockServices.Verify(s => s.AddJobProfile(expectedJobProfile), Times.Once());
        }

        [TestMethod()]
        public async Task CreateJobProfileFails()
        {
            var expectedJobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.NewGuid(),
                ProfileName = "Test Profile",
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };

            _mockServices.Setup(s => s.AddJobProfile(expectedJobProfile))
                .ThrowsAsync(new ArgumentException("Adding new Job Profile Failed"));

            var result = await _controller.CreateJobProfile(expectedJobProfile);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value);
        }

        [TestMethod()]
        public async Task UpdateJobProfile()
        {
            var expectedJobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.NewGuid(),
                ProfileName = "Test Profile",
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };

            _mockServices.Setup(s => s.UpdateJobProfile(expectedJobProfile))
                .ReturnsAsync(OperationResult.CreateSuccess("Updated job profile successfully."));

            var result = await _controller.UpdateJobProfile(expectedJobProfile) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateJobProfileFails()
        {
            var expectedJobProfile = new JobProfile
            {
                Id = Guid.NewGuid(),
                UserProfileId = Guid.NewGuid(),
                ProfileName = "Test Profile",
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
            };

            _mockServices.Setup(s => s.UpdateJobProfile(expectedJobProfile))
                .ThrowsAsync(new ArgumentException("Update Job Profile Failed"));

            var result = await _controller.UpdateJobProfile(expectedJobProfile);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value);
        }

        [TestMethod()]
        public async Task DeleteJobProfile()
        {
            var jobProfileId = Guid.NewGuid();

            _mockServices.Setup(s => s.DeleteJobProfile(jobProfileId))
                .ReturnsAsync(OperationResult.CreateSuccess("Deleted job profile successfully."));

            var result = await _controller.DeleteJobProfile(jobProfileId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod()]
        public async Task DeleteJobProfileFails()
        {
            var jobProfileId = Guid.NewGuid();

            _mockServices.Setup(s => s.DeleteJobProfile(jobProfileId))
                .ThrowsAsync(new ArgumentException("Job Profile Delete Failed"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.DeleteJobProfile(jobProfileId));
        }
        #endregion

        #region Employer Profile Tests
        [TestMethod()]
        public async Task GetEmployerProfile()
        {
            var expectedEmployerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
            };

            _mockServices.Setup(s => s.GetEmployerProfile(expectedEmployerProfile.Id))
                .ReturnsAsync(expectedEmployerProfile);

            var result = await _controller.GetEmployerProfile(expectedEmployerProfile.Id) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var employerProfile = result.Value as EmployerProfile;
            Assert.IsNotNull(employerProfile);
            Assert.AreEqual(expectedEmployerProfile.Id, employerProfile.Id);
            Assert.AreEqual(expectedEmployerProfile.Name, employerProfile.Name);
        }

        [TestMethod()]
        public async Task GetEmployerProfileFails()
        {
            var expectedEmployerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
            };

            _mockServices.Setup(s => s.GetEmployerProfile(expectedEmployerProfile.Id))
                .ThrowsAsync(new ArgumentException("Failed to get Employer Profile"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.GetEmployerProfile(expectedEmployerProfile.Id));
        }

        [TestMethod()]
        public async Task GetEmployerProfiles()
        {
            var jobProfileId = Guid.NewGuid();
            var expectedEmployerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                },
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                }
            };

            _mockServices.Setup(s => s.GetEmployerProfiles(jobProfileId))
                .ReturnsAsync(expectedEmployerProfiles);

            var result = await _controller.GetEmployerProfiles(jobProfileId) as OkObjectResult;

            Assert.IsNotNull(result);
            var employerProfile = result.Value as IEnumerable<EmployerProfile>;
            Assert.IsNotNull(employerProfile);
            Assert.AreEqual(expectedEmployerProfiles.Count(), employerProfile.Count());
            CollectionAssert.AreEqual(expectedEmployerProfiles, employerProfile.ToList());
        }

        [TestMethod]
        public async Task GetEmployerProfilesFail()
        {
            var jobProfileId = Guid.NewGuid();
            var expectedEmployerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                },
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                }
            };

            _mockServices.Setup(s => s.GetEmployerProfiles(jobProfileId))
                .ThrowsAsync(new ArgumentException("Get All Employer Profiles Failed"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.GetEmployerProfiles(jobProfileId));
        }

        [TestMethod()]
        public async Task UpdateEmployerProfile()
        {
            var expectedEmployerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
            };

            _mockServices.Setup(s => s.UpdateEmployerProfile(expectedEmployerProfile))
                .ReturnsAsync(OperationResult.CreateSuccess("Updated employer profile successfully."));

            var result = await _controller.UpdateEmployerProfile(expectedEmployerProfile) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateEmployerProfileFails()
        {
            var expectedEmployerProfile = new EmployerProfile
            {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
            };

            _mockServices.Setup(s => s.UpdateEmployerProfile(expectedEmployerProfile))
                .ThrowsAsync(new ArgumentException("Update Employer Profile Failed"));

            var result = await _controller.UpdateEmployerProfile(expectedEmployerProfile);

            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = result as ObjectResult;
            Assert.AreEqual(500, objectResult.StatusCode);
            Assert.AreEqual("Internal server error", objectResult.Value);
        }

        [TestMethod()]
        public async Task GetEmployerPagingData()
        {
            var jobProfileId = Guid.NewGuid();
            var pageSize = 10;
            var pageIndex = 1;
            var expectedEmployerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                },
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                }
            };
            
            _mockServices.Setup(s => s.GetEmployerPagingData(jobProfileId, pageIndex, pageSize))
                .ReturnsAsync(expectedEmployerProfiles);

            var result = await _controller.GetEmployerPagingData(jobProfileId, pageIndex, pageSize) as OkObjectResult;

            Assert.IsNotNull(result);
            var employerPagingData = result.Value as PagingResponse<EmployerProfile>;
            Assert.IsNotNull(employerPagingData);
            Assert.AreEqual(expectedEmployerProfiles.Count, employerPagingData.Data.Count());
            CollectionAssert.AreEqual(expectedEmployerProfiles, employerPagingData.Data.ToList());
        }

        [TestMethod]
        public async Task GetEmployerPagingDataFails()
        {
            var jobProfileId = Guid.NewGuid();
            var pageSize = 10;
            var pageIndex = 1;
            var expectedEmployerProfiles = new List<EmployerProfile>
            {
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                },
                new EmployerProfile
                {
                Id = Guid.NewGuid(),
                JobProfileId = Guid.NewGuid(),
                Date = DateTime.Now,
                LatestUpdate = DateTime.Now,
                Name = "Test",
                Title = "Test",
                Address = "Test",
                City = "Test",
                State = "Test",
                Zip = "Test",
                Phone = "Test",
                Email = "Test",
                Website = "Test"
                }
            };

            _mockServices.Setup(s => s.GetEmployerPagingData(jobProfileId, pageIndex, pageSize))
                .ThrowsAsync(new ArgumentException("Employer Paging Failed"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.GetEmployerPagingData(jobProfileId, pageIndex, pageSize));
        }

        [TestMethod]
        public async Task DeleteEmployerProfile()
        {
            var employerProfileId = Guid.NewGuid();

            _mockServices.Setup(s => s.DeleteEmployerProfile(employerProfileId))
                .ReturnsAsync(OperationResult.CreateSuccess("Delete employer profile successfully."));

            var result = await _controller.DeleteEmployerProfile(employerProfileId) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod()]
        public async Task DeleteEmployerProfileFails()
        {
            var employerProfileId = Guid.NewGuid();

            _mockServices.Setup(s => s.DeleteEmployerProfile(employerProfileId))
                .ThrowsAsync(new ArgumentException("Employer Profile Delete Failed"));

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await _controller.DeleteEmployerProfile(employerProfileId));
        }
        #endregion
    }
}