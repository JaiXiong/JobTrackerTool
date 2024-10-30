using JobTrackerToolBusiness;
using Microsoft.AspNetCore.Mvc;

namespace JobTrackerTool.Controllers
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
        public JobProfile GetEmployerName(Guid id)
        {
            //var jobProfiles = jobProfiles.FirstOrDefault(x => x.Id == id);
            var jobProfiles = _jobTrackerToolService.JobProfiles.FirstOrDefault(x => x.Id == id);

            return 
        }

        [HttpGet(Name = "GetEmployerName")]
        public IEnumerable<JobProfile> GetEmployerNames(Guid id)
        {
            var jobProfiles = new List<JobProfile>();

            return
        }
    }
}
