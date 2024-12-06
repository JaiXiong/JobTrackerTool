using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobTracker.API.tool.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobData.Entities;
using JobTracker.Business.Services;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using System.Resources;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace JobTracker.API.tool.Controllers.Tests
{
    [TestClass()]
    public class JobTrackerControllerTests
    {
        private readonly Mock<ILogger<JobTrackerController>> _mockLogger;
        private readonly Mock<JobProfileContext> _mockProfileContext;
        private readonly Mock<ResourceManager> _mockResourceManager;
        private readonly Mock<IJobTrackerToolService> _mockServices;
        private readonly JobTrackerController _controller;

        public JobTrackerControllerTests()
        {
            _mockLogger = new Mock<ILogger<JobTrackerController>>();
            _mockProfileContext = new Mock<JobProfileContext>(new DbContextOptions<JobProfileContext>());
            _mockResourceManager = new Mock<ResourceManager>();
            _mockServices = new Mock<IJobTrackerToolService>();
            _controller = new JobTrackerController(_mockLogger.Object, _mockServices.Object);
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
            var result = await _controller.GetJobProfile(expectedJobProfile.Id);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedJobProfile.Id, result.Id);
            Assert.AreEqual(expectedJobProfile.ProfileName, result.ProfileName);
        }

        [TestMethod()]
        public async Task GetJobProfileTestFails()
        {
            //Arrange
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
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateJobProfile(expectedJobProfile);

            Assert.IsNotNull(result);
            _mockServices.Verify(s =>  s.AddJobProfile(expectedJobProfile), Times.Once());
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
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateJobProfile(expectedJobProfile);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.AreEqual(nameof(_controller.UpdateJobProfile), createdAtActionResult.ActionName);
            Assert.AreEqual(expectedJobProfile.Id, ((JobProfile)createdAtActionResult.Value).Id);
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

            _mockServices.Setup(s => s.GetEmployerProfile(expectedEmployerProfile.JobProfileId, expectedEmployerProfile.Id))
                .ReturnsAsync(expectedEmployerProfile);

            var result = await _controller.GetEmployerProfile(expectedEmployerProfile.JobProfileId, expectedEmployerProfile.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(result, expectedEmployerProfile);
            Assert.AreEqual(result.Id, expectedEmployerProfile.Id);
        }
        #endregion
    }
}