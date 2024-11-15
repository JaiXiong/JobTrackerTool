using JobData.Entities;
using JobTracker.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.tool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobTrackerController : ControllerBase
    {
        private readonly ILogger<JobTrackerController> _logger;
        private readonly JobTrackerToolService _jobTrackerToolService;
        //private readonly JobTrackerToolService _jobTrackerDBcontext;

        public JobTrackerController(ILogger<JobTrackerController> logger, JobTrackerToolService jobTrackerToolService, JobTrackerToolService jobTrackerDBcontext)
        {
            _logger = logger;
            _jobTrackerToolService = jobTrackerToolService;
            //_jobTrackerDBcontext = jobTrackerDBcontext;
        }

        [HttpGet("employerprofile/{jobprofileid}/{employerprofileid}", Name = "GetEmployerName")]
        public async Task<EmployerProfile> GetEmployerProfile(Guid jobProfileId, Guid employerProfileId)
        {
            try
            {
                var employerProfile = await _jobTrackerToolService.GetEmployer(jobProfileId, employerProfileId);
                return employerProfile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                throw new ArgumentException("An error occurred while getting the employer profile.");
            }
        }

        [HttpGet("employerprofiles/{jobprofileid}", Name = "GetEmployerNames")]
        public async Task<IEnumerable<EmployerProfile>> GetEmployerProfiles(Guid jobProfileId)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployers(jobProfileId);
                return employerProfiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles.");
                throw new ArgumentException("An error occurred while getting the employer profiles.");
            }

        }

        [HttpGet("jobprofile/{jobprofileid}", Name = "GetJobProfile")]
        public async Task<JobProfile> GetJobProfile(Guid jobprofileid)
        {
            try
            {
                return await _jobTrackerToolService.GetJobProfile(jobprofileid); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the job profile.");
                throw new ArgumentException("An error occurred while getting the job profile.");
            }
        }

        [HttpGet("jobprofiles/{userProfileId}", Name = "GetJobProfiles")]
        public async Task<IEnumerable<JobProfile>> GetJobProfiles(Guid userProfileId)
        {
            try
            {
                var jobProfiles = await _jobTrackerToolService.GetJobProfiles(userProfileId);
                return jobProfiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                throw new ArgumentException("An error occurred while getting the employer profile.");
            }
        }

        [HttpGet("employerpagingdata/{jobProfileId}/{pageIndex}/{pageSize}", Name = "GetPaging")]
        public async Task<IEnumerable<EmployerProfile>> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerPagingData(jobProfileId, pageIndex, pageSize);
                return employerProfiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles paging.");
                throw new ArgumentException("An error occurred while paging the employer profiles.");
            }
        }

        [HttpPost("userprofile",Name="CreateUserProfile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfile userProfile)
        {
            _jobTrackerToolService.ValidateNewUser(userProfile);

            try
            {
                await _jobTrackerToolService.AddUserProfile(userProfile);

                return CreatedAtAction(nameof(CreateUserProfile), new { id = userProfile.Id }, userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("jobprofile", Name = "CreateJobProfile")]
        public async Task<IActionResult> CreateJobProfile([FromBody] JobProfile jobProfile)
        {
            //if (jobProfile == null)
            //{
            //    return BadRequest("JobProfile is null.");
            //}

            try
            {
                await _jobTrackerToolService.AddJobProfile(jobProfile);

                return CreatedAtAction(nameof(CreateJobProfile), new { id = jobProfile.Id }, jobProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the job profile.");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpPost("employerprofile", Name = "CreateEmployerProfile")]
        public async Task<IActionResult> CreateEmployerProfile([FromBody] EmployerProfile employerProfile)
        {
            if (employerProfile == null)
            {
                return BadRequest("EmployerProfile is null.");
            }

            try
            {
                await _jobTrackerToolService.AddEmployerProfile(employerProfile);

                return CreatedAtAction(nameof(CreateEmployerProfile), new { id = employerProfile.Id }, employerProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("actionresult", Name = "CreateActionResult")]
        public async Task<IActionResult> CreateActionResult([FromBody] JobAction actionResult)
        {
            try
            {
                await _jobTrackerToolService.AddActionResult(actionResult);

                return CreatedAtAction(nameof(CreateActionResult), new { id = actionResult.Id }, actionResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the action result.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detail", Name = "CreateDetail")]
        public async Task<IActionResult> CreateDetail([FromBody] Detail detail)
        {
            try
            {
                await _jobTrackerToolService.AddDetail(detail);

                return CreatedAtAction(nameof(CreateDetail), new { id = detail.Id }, detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the detail.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}