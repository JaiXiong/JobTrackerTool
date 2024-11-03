using JobTracker.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.tool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobTrackerController : ControllerBase
    {

        private readonly ILogger<JobTrackerController> _logger;
        private readonly JobTrackerToolService _jobTrackerToolService;

        public JobTrackerController(ILogger<JobTrackerController> logger, JobTrackerToolService jobTrackerToolService)
        {
            _logger = logger;
            _jobTrackerToolService = jobTrackerToolService;
        }

        [HttpGet(Name = "GetEmployerName")]
        public string GetEmployerName(Guid id)
        {
            //var jobProfiles = jobProfiles.FirstOrDefault(x => x.Id == id);
            var name = _jobTrackerToolService.GetEmployerName(id);

            return name;
        }

        [HttpGet(Name = "GetEmployerName")]
        public IEnumerable<JobProfile> GetEmployerNames(Guid id)
        {
            var jobProfiles = new List<JobProfile>();

            return jobProfiles;
        }

        [HttpPost(Name = "CreateJobProfile")]
        //public async Task<IActionResult> CreateJobProfile([FromBody] JobProfile jobProfile)
        public async Task<IActionResult> CreateJobProfile()
        {
            JobProfile jobProfile = new JobProfile();
            _jobTrackerToolService.Add(jobProfile);

            await _jobTrackerToolService.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployerName), new { id = jobProfile.Id }, jobProfile);
        }
        [HttpPost(Name = "AddActionResult")]
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
