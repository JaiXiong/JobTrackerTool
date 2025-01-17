using AutoMapper;
using JobData.Dtos;
using JobData.Entities;
using JobTracker.Business.Business;
using JobTracker.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;
using Utils.Encryption;

namespace JobTracker.API.tool.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class JobTrackerController : ControllerBase
    {
        private readonly ILogger<JobTrackerController> _logger;
        private readonly IJobTrackerToolService _jobTrackerToolService;
        private readonly Encryption _encyption;
        private readonly IJobTrackerToolBusiness _jobTrackerToolBusiness;
        private readonly IMapper _mapper;
        //private readonly JobTrackerToolService _jobTrackerDBcontext;

        public JobTrackerController(ILogger<JobTrackerController> logger, IJobTrackerToolService jobTrackerToolService, Encryption encyption, IJobTrackerToolBusiness jobTrackerToolBusiness, IMapper mapper)
        {
            _logger = logger;
            _jobTrackerToolService = jobTrackerToolService;
            _encyption = encyption;
            _jobTrackerToolBusiness = jobTrackerToolBusiness;
            _mapper = mapper;
            //_jobTrackerDBcontext = jobTrackerDBcontext;
        }

        [HttpGet("employerprofile/{jobprofileid}/{employerprofileid}", Name = "GetEmployerName")]
        public async Task<IActionResult> GetEmployerProfile(Guid jobProfileId, Guid employerProfileId)
        {
            try
            {
                var employerProfile = await _jobTrackerToolService.GetEmployerProfile(jobProfileId, employerProfileId);
                return Ok(employerProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                throw new ArgumentException("An error occurred while getting the employer profile.");
            }
        }

        [HttpGet("employerprofiles/{jobprofileid}", Name = "GetEmployerNames")]
        public async Task<IActionResult> GetEmployerProfiles(Guid jobProfileId)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId);
                return Ok(employerProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles.");
                throw new ArgumentException("An error occurred while getting the employer profiles.");
            }

        }

        [HttpGet("employerprofiles", Name = "GetAllEmployerNames")]
        public async Task<IActionResult> GetAllEmployerProfiles()
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetAllEmployerProfiles();
                return Ok(employerProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles.");
                throw new ArgumentException("An error occurred while getting the employer profiles.");
            }

        }

        [HttpGet("jobprofile/{jobprofileid}", Name = "GetJobProfile")]
        public async Task<IActionResult> GetJobProfile(Guid jobprofileid)
        {
            try
            {
                return Ok(await _jobTrackerToolService.GetJobProfile(jobprofileid));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the job profile.");
                throw new ArgumentException("An error occurred while getting the job profile.");
            }
        }

        [HttpGet("jobprofiles/{userProfileId}", Name = "GetJobProfiles")]
        public async Task<IActionResult> GetJobProfiles(Guid userProfileId)
        {
            try
            {
                var jobProfiles = await _jobTrackerToolService.GetJobProfiles(userProfileId);
                return Ok(jobProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                throw new ArgumentException("An error occurred while getting the employer profile.");
            }
        }

        [HttpGet("jobprofiles", Name = "GetAllJobProfiles")]
        public async Task<IActionResult> GetAllJobProfiles()
        {
            try
            {
                var jobProfiles = await _jobTrackerToolService.GetAllJobProfiles();
                return Ok(jobProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                throw new ArgumentException("An error occurred while getting the employer profile.");
            }
        }

        [HttpGet("employerpagingdata/{jobProfileId}/{pageIndex}/{pageSize}", Name = "GetPaging")]
        public async Task<IActionResult> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerPagingData(jobProfileId, pageIndex, pageSize);
                var totalCount = await _jobTrackerToolService.GetTotalEmployerCount(jobProfileId);

                var response = new PagingResponse<EmployerProfile>(employerProfiles, totalCount);

                return Ok(response);
                //return employerProfiles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles paging.");
                throw new ArgumentException("An error occurred while paging the employer profiles.");
            }
        }

        [HttpGet("jobaction", Name = "GetAllJobAction")]
        public async Task<IActionResult> GetAllJobAction()
        {
            try
            {
                var jobActions = await _jobTrackerToolService.GetAllJobActions();
                return Ok(jobActions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the job action.");
                throw new ArgumentException("An error occurred while getting the job action.");
            }
        }

        [HttpGet("jobaction/{employerProfileId}", Name = "GetJobAction")]
        public async Task<IActionResult> GetJobAction(Guid employerProfileId)
        {
            try
            {
                var jobAction = await _jobTrackerToolService.GetJobAction(employerProfileId);
                return Ok(jobAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the action result.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("detail", Name = "GetAllDetail")]
        public async Task<IActionResult> GetAllDetail()
        {
            try
            {
                var details = await _jobTrackerToolService.GetAllDetails();
                return Ok(details);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the detail.");
                throw new ArgumentException("An error occurred while getting the detail.");
            }
        }

        [HttpGet("detail/{employerProfileId}", Name = "GetDetail")]
        public async Task<IActionResult> GetDetail(Guid employerProfileId)
        {
            try
            {
                var detail = await _jobTrackerToolService.GetDetail(employerProfileId);
                return Ok(detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("userprofile", Name = "CreateUserProfile")]
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

        [HttpPost("jobaction/{employerProfileId}", Name = "AddJobAction")]
        public async Task<IActionResult> AddJobAction(Guid employerProfileId, [FromBody] JobAction jobAction)
        {
            try
            {
                await _jobTrackerToolService.AddJobAction(employerProfileId, jobAction);

                return CreatedAtAction(nameof(AddJobAction), new { id = jobAction.Id }, jobAction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the action result.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detail/{employerProfileId}", Name = "CreateDetail")]
        public async Task<IActionResult> CreateDetail(Guid employerProfileId, [FromBody] Detail detail)
        {
            try
            {
                await _jobTrackerToolService.AddDetail(employerProfileId, detail);

                return CreatedAtAction(nameof(CreateDetail), new { id = detail.Id }, detail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("employerprofile", Name = "UpdateEmployerProfile")]
        public async Task<IActionResult> UpdateEmployerProfile([FromBody] EmployerProfile employerProfile)
        {
            if (employerProfile == null)
            {
                return BadRequest("EmployerProfile is null.");
            }

            try
            {
                await _jobTrackerToolService.UpdateEmployerProfile(employerProfile);
                //return CreatedAtAction(nameof(UpdateEmployerProfile), new { id = employerProfile.Id }, employerProfile);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("jobprofile", Name = "UpdateJobProfile")]
        public async Task<IActionResult> UpdateJobProfile([FromBody] JobProfile jobProfile)
        {
            if (jobProfile == null)
            {
                return BadRequest("JobProfile is null.");
            }
            try
            {
                await _jobTrackerToolService.UpdateJobProfile(jobProfile);
                //return CreatedAtAction(nameof(UpdateJobProfile), new { id = jobProfile.Id }, jobProfile);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the job profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("jobprofile/{jobProfileId}", Name = "DeleteJobProfile")]
        public async Task<IActionResult> DeleteJobProfile(Guid jobProfileId)
        {
            try
            {
                await _jobTrackerToolService.DeleteJobProfile(jobProfileId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the job profile.");
                throw new ArgumentException("An error occurred while deleting the job profile.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the job profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("employerprofile/{employerProfileId}", Name = "DeleteEmployerProfile")]
        public async Task<IActionResult> DeleteEmployerProfile(Guid employerProfileId)
        {
            try
            {
                await _jobTrackerToolService.DeleteEmployerProfile(employerProfileId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employer profile.");
                throw new ArgumentException("An error occurred while deleting the employer profile.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("downloadcsv/{jobProfileId}", Name ="DownloadCsv")]
        public async Task<IActionResult> DownloadCsv(Guid jobProfileId)
        {
            var csv = new StringBuilder();
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId);

                if (Request.Headers.TryGetValue("include", out var includeHeader) && includeHeader == "true")
                {
                    csv = _jobTrackerToolBusiness.CsvCreateSelected(jobProfileId, employerProfiles);
                }
                else
                {
                    csv = _jobTrackerToolBusiness.CsvCreateAll(jobProfileId, employerProfiles);
                }

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var date = DateTime.Now;
                var result = new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = $"{jobProfileId}_employerProfiles_{date:yyyyMMdd}.csv"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the employer profiles.");
                throw new ArgumentException("An error occurred while downloading the employer profile.");
            }
        }

        [HttpGet("downloadpdf/{jobProfileId}", Name = "DownloadPdf")]
        public async Task<IActionResult> DownloadPdf(Guid jobProfileId)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId);

                var pdf = _jobTrackerToolBusiness.PdfCreate(jobProfileId, employerProfiles);

                var date = DateTime.Now;
                var result = new FileContentResult(pdf, "application/pdf")
                {
                    FileDownloadName = $"{jobProfileId}_employerProfiles_{date::yyyyMMdd}.csv"
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the employer profiles.");
                throw new ArgumentException("An error occurred while downloading the employer profile.");
            }
        }

        [HttpPost("upload", Name ="Upload")]
        public async Task<IActionResult> Upload(IFormFile uploadFile)
        {
            if (uploadFile == null || uploadFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                // Process the file here

                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the file.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}