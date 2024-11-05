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
        private readonly JobTrackerToolService _jobTrackerDBcontext;

        public JobTrackerController(ILogger<JobTrackerController> logger, JobTrackerToolService jobTrackerToolService, JobTrackerToolService jobTrackerDBcontext)
        {
            _logger = logger;
            _jobTrackerToolService = jobTrackerToolService;
            _jobTrackerDBcontext = jobTrackerDBcontext;
        }

        [HttpGet("employer/{id}", Name = "GetEmployerName")]
        public string GetEmployerName(Guid id)
        {
            var name = _jobTrackerToolService.GetEmployerName(id);
            return name;
        }

        [HttpGet("employers/{id}", Name = "GetEmployerNames")]
        public IEnumerable<JobProfile> GetEmployerNames(Guid id)
        {
            var jobProfiles = new List<JobProfile>();
            return jobProfiles;
        }

        [HttpPost("jobprofile", Name = "CreateJobProfile")]
        public async Task<IActionResult> CreateJobProfile([FromBody] JobProfile jobProfile)
        {
            if (jobProfile == null)
            {
                return BadRequest("JobProfile is null.");
            }

            await _jobTrackerToolService.AddJobProfile(jobProfile);
            //await _jobTrackerToolService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployerName), new { id = jobProfile.Id }, jobProfile);
        }

        [HttpPost("actionresult", Name = "AddActionResult")]
        public async Task<IActionResult> ActionResult([FromBody] JobProfile jobProfile)
        {
            if (jobProfile == null)
            {
                return BadRequest("JobProfile is null.");
            }

            _jobTrackerToolService.AddJobProfile(jobProfile);
            await _jobTrackerToolService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployerName), new { id = jobProfile.Id }, jobProfile);
        }
    }
}