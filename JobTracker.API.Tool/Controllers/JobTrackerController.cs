using JobEntities.Entities;
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
    }
}
