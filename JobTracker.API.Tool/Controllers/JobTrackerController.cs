using AutoMapper;
using JobData.Common;
using JobData.Dtos;
using JobData.Entities;
using JobTracker.Business.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Utils.CustomExceptions;
using Utils.Encryption;

namespace JobTracker.API.tool.Controllers
{
    /// <summary>
    ///Controller for handling login and user registration.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="JobTrackerController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="jobTrackerToolService">The job tracker tool service instance.</param>
        /// <param name="encyption">The encryption instance.</param>
        /// <param name="jobTrackerToolBusiness">The job tracker tool business instance.</param>
        /// <param name="mapper">The mapper instance.</param>
        public JobTrackerController(ILogger<JobTrackerController> logger, IJobTrackerToolService jobTrackerToolService, Encryption encyption, IJobTrackerToolBusiness jobTrackerToolBusiness, IMapper mapper)
        {
            _logger = logger;
            _jobTrackerToolService = jobTrackerToolService;
            _encyption = encyption;
            _jobTrackerToolBusiness = jobTrackerToolBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the employer based on the id.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer.</param>
        /// <returns>A employer profile of given id.</returns>
        [HttpGet("employerprofile/{employerProfileid}", Name = "GetEmployer")]
        public async Task<IActionResult> GetEmployerProfile(Guid employerProfileId)
        {
            try
            {
                var downloadOptions = new DownloadOptions
                {
                    Include = Request.Query["Include"].ToString() == "true" ? DownloadType.Include : DownloadType.None,
                    Csv = Request.Query["Csv"].ToString() == "true" ? DownloadType.Csv : DownloadType.None,
                    Pdf = Request.Query["Pdf"].ToString() == "true" ? DownloadType.Pdf : DownloadType.None
                };

                var employerProfile = await _jobTrackerToolService.GetEmployerProfile(employerProfileId, downloadOptions);

                return Ok(employerProfile);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while creating the employer profile.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a list of employer based on the job id.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <returns>A list of all employer profile of given job id.</returns>
        [HttpGet("employerprofiles/{jobprofileid}", Name = "GetEmployers")]
        public async Task<IActionResult> GetEmployerProfiles(Guid jobProfileId)
        {
            try
            {
                var downloadOptions = new DownloadOptions
                {
                    Include = Request.Query["Include"].ToString() == "true" ? DownloadType.Include : DownloadType.None,
                    Csv = Request.Query["Csv"].ToString() == "true" ? DownloadType.Csv : DownloadType.None,
                    Pdf = Request.Query["Pdf"].ToString() == "true" ? DownloadType.Pdf : DownloadType.None
                };

                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId, downloadOptions);

                return Ok(employerProfiles);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting the employer profiles.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles.");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Gets list of all employer in database.
        /// </summary>
        /// <returns>A list of all employer profiles.</returns>
        [HttpGet("employerprofiles", Name = "GetAllEmployerNames")]
        public async Task<IActionResult> GetAllEmployerProfiles()
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetAllEmployerProfiles();

                return Ok(employerProfiles);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting all the employer profiles.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer profiles.");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Gets a job profile based on the job id.
        /// </summary>
        /// <param name="jobprofileid">The ID of the job profile.</param>
        /// <returns>A job profile of given job id.</returns>
        [HttpGet("jobprofile/{jobprofileid}", Name = "GetJobProfile")]
        public async Task<IActionResult> GetJobProfile(Guid jobprofileid)
        {
            try
            {
                var jobProfile = await _jobTrackerToolService.GetJobProfile(jobprofileid);

                return Ok(jobProfile);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting the job profile.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the job profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a user profile based on the user id.
        /// </summary>
        /// <param name="userProfileId">The ID of the user profile.</param>
        /// <returns>A user profile of given user id.</returns>
        [HttpGet("jobprofiles/{userProfileId}", Name = "GetJobProfiles")]
        public async Task<IActionResult> GetJobProfiles(Guid userProfileId)
        {
            try
            {
                var jobProfiles = await _jobTrackerToolService.GetJobProfiles(userProfileId);

                return Ok(jobProfiles);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting the job profiles.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the job profiles.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all job profiles in database.
        /// </summary>
        /// <returns>All job profiles.</returns>
        [HttpGet("jobprofiles", Name = "GetAllJobProfiles")]
        public async Task<IActionResult> GetAllJobProfiles()
        {
            try
            {
                var jobProfiles = await _jobTrackerToolService.GetAllJobProfiles();

                return Ok(jobProfiles);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting all the job profiles.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all the job profiles.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a paged list of employer profile based on the user job id.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <param name="pageIndex">Page user selects.</param>
        /// <param name="pageSize">Size user selects.</param>
        /// <returns>A paged list of employers.</returns>
        [HttpGet("employerpagingdata/{jobProfileId}/{pageIndex}/{pageSize}", Name = "GetPaging")]
        public async Task<IActionResult> GetEmployerPagingData(Guid jobProfileId, int pageIndex, int pageSize)
        {
            try
            {
                var employerProfiles = await _jobTrackerToolService.GetEmployerPagingData(jobProfileId, pageIndex, pageSize);
                var totalCount = await _jobTrackerToolService.GetTotalEmployerCount(jobProfileId);

                var response = new PagingResponse<EmployerProfile>(employerProfiles, totalCount);

                return Ok(response);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting employer paging data.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer paging.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all job actions.
        /// </summary>
        /// <returns>A list of all job actions.</returns>
        [HttpGet("jobaction", Name = "GetAllJobAction")]
        public async Task<IActionResult> GetAllJobAction()
        {
            try
            {
                var jobActions = await _jobTrackerToolService.GetAllJobActions();

                return Ok(jobActions);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting all the employer actions.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all the employer actions.");
                throw new ArgumentException("An error occurred while getting all the employer actions.");
            }
        }

        /// <summary>
        /// Gets a employer action based on the user employer id.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer profile.</param>
        /// <returns>A employer job action.</returns>
        [HttpGet("jobaction/{employerProfileId}", Name = "GetJobAction")]
        public async Task<IActionResult> GetJobAction(Guid employerProfileId)
        {
            try
            {
                var jobAction = await _jobTrackerToolService.GetJobAction(employerProfileId);

                return Ok(jobAction);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting the employer actions.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the employer actions.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets all details.
        /// </summary>
        /// <returns>A list of all details.</returns>
        [HttpGet("detail", Name = "GetAllDetail")]
        public async Task<IActionResult> GetAllDetail()
        {
            try
            {
                var details = await _jobTrackerToolService.GetAllDetails();

                return Ok(details);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting all the employer details.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all the detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets a detail based on the employer profile id.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer profile.</param>
        /// <returns>A detail of given employer profile id.</returns>
        [HttpGet("detail/{employerProfileId}", Name = "GetDetail")]
        public async Task<IActionResult> GetDetail(Guid employerProfileId)
        {
            try
            {
                var detail = await _jobTrackerToolService.GetDetail(employerProfileId);

                return Ok(detail);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while getting the employer details.");
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new user profile.
        /// </summary>
        /// <param name="userProfile">The user profile to create.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("userprofile", Name = "CreateUserProfile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfile userProfile)
        {
            try
            {
                var result = await _jobTrackerToolService.AddUserProfile(userProfile);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(CreateUserProfile), new { id = userProfile.Id }, userProfile);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new job profile.
        /// </summary>
        /// <param name="jobProfile">The job profile to create.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("jobprofile", Name = "CreateJobProfile")]
        public async Task<IActionResult> CreateJobProfile([FromBody] JobProfile jobProfile)
        {
            try
            {
                var result = await _jobTrackerToolService.AddJobProfile(jobProfile);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(CreateJobProfile), new { id = jobProfile.Id }, jobProfile);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the job profile.");
                return StatusCode(500, "Internal server error");
            }

        }

        /// <summary>
        /// Creates a new employer profile.
        /// </summary>
        /// <param name="employerProfile">The employer profile to create.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("employerprofile", Name = "CreateEmployerProfile")]
        public async Task<IActionResult> CreateEmployerProfile([FromBody] EmployerProfile employerProfile)
        {
            try
            {
                var result = await _jobTrackerToolService.AddEmployerProfile(employerProfile);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(CreateEmployerProfile), new { id = employerProfile.Id }, employerProfile);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Adds a job action for a specific employer profile.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer profile.</param>
        /// <param name="jobAction">The job action to add.</param>
        /// <returns>A response indicating the result of the add operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("jobaction/{employerProfileId}", Name = "AddJobAction")]
        public async Task<IActionResult> AddJobAction(Guid employerProfileId, [FromBody] JobAction jobAction)
        {
            //TODO:
            //eventually we need to rename this to make more sense
            //it's results but named jobaction
            try
            {
                var result = await _jobTrackerToolService.AddJobAction(employerProfileId, jobAction);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(AddJobAction), new { id = jobAction.Id }, jobAction);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the action result.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new detail for a specific employer profile.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer profile.</param>
        /// <param name="detail">The detail to create.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("detail/{employerProfileId}", Name = "CreateDetail")]
        public async Task<IActionResult> CreateDetail(Guid employerProfileId, [FromBody] Detail detail)
        {
            try
            {
                var result = await _jobTrackerToolService.AddDetail(employerProfileId, detail);

                if (result.Success)
                {
                    return CreatedAtAction(nameof(CreateDetail), new { id = detail.Id }, detail);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the detail.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing employer profile.
        /// </summary>
        /// <param name="employerProfile">The employer profile to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPut("employerprofile", Name = "UpdateEmployerProfile")]
        public async Task<IActionResult> UpdateEmployerProfile([FromBody] EmployerProfile employerProfile)
        {
            try
            {
                var result = await _jobTrackerToolService.UpdateEmployerProfile(employerProfile);

                if (result.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing job profile.
        /// </summary>
        /// <param name="jobProfile">The job profile to update.</param>
        /// <returns>A response indicating the result of the update operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPut("jobprofile", Name = "UpdateJobProfile")]
        public async Task<IActionResult> UpdateJobProfile([FromBody] JobProfile jobProfile)
        {
            try
            {
                var result = await _jobTrackerToolService.UpdateJobProfile(jobProfile);

                if (result.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the job profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a job profile based on the job profile ID.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpDelete("jobprofile/{jobProfileId}", Name = "DeleteJobProfile")]
        public async Task<IActionResult> DeleteJobProfile(Guid jobProfileId)
        {
            try
            {
                var result = await _jobTrackerToolService.DeleteJobProfile(jobProfileId);

                if (result.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the job profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an employer profile based on the employer profile ID.
        /// </summary>
        /// <param name="employerProfileId">The ID of the employer profile to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpDelete("employerprofile/{employerProfileId}", Name = "DeleteEmployerProfile")]
        public async Task<IActionResult> DeleteEmployerProfile(Guid employerProfileId)
        {
            try
            {
                var result = await _jobTrackerToolService.DeleteEmployerProfile(employerProfileId);

                if (result.Success)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Downloads all employer profiles for a specific job profile as a zip file.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <returns>A zip file containing all employer profiles for the specified job profile.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpGet("downloadall/{jobProfileId}", Name = "Download")]
        public async Task<IActionResult> DownloadAll(Guid jobProfileId)
        {
            byte[] filesTobeDownloaded;
            try
            {
                var downloadOptions = new DownloadOptions
                {
                    Include = Request.Query["include"].ToString() == "true" ? DownloadType.Include : DownloadType.None,
                    Pdf = Request.Query["pdf"].ToString() == "true" ? DownloadType.Pdf : DownloadType.None,
                    Csv = Request.Query["csv"].ToString() == "true" ? DownloadType.Csv : DownloadType.None
                };

                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId, downloadOptions);

                filesTobeDownloaded = _jobTrackerToolBusiness.CreateZipFile(jobProfileId, employerProfiles, downloadOptions);

                var date = DateTime.Now;
                var result = new FileContentResult(filesTobeDownloaded, "application/zip")
                {
                    FileDownloadName = $"{jobProfileId}_employerProfiles_{date:yyyyMMdd}.zip"
                };

                return result;
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while downloading employer zip");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the employer profiles.");
                throw new ArgumentException("An error occurred while downloading the employer profile.");
            }
        }

        /// <summary>
        /// Downloads all employer profiles for a specific job profile as a CSV file.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <returns>A CSV file containing all employer profiles for the specified job profile.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpGet("downloadcsv/{jobProfileId}", Name = "DownloadCsv")]
        public async Task<IActionResult> DownloadCsv(Guid jobProfileId)
        {
            var csv = new StringBuilder();
            try
            {
                var downloadOptions = new DownloadOptions
                {
                    Include = Request.Query["Include"].ToString() == "true" ? DownloadType.Include : DownloadType.None,
                    Csv = Request.Query["Csv"].ToString() == "true" ? DownloadType.Csv : DownloadType.None,
                    Pdf = Request.Query["Pdf"].ToString() == "true" ? DownloadType.Pdf : DownloadType.None
                };

                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId, downloadOptions);

                csv = _jobTrackerToolBusiness.DownloadCsv(jobProfileId, employerProfiles, downloadOptions);

                var bytes = Encoding.UTF8.GetBytes(csv.ToString());
                var date = DateTime.Now;
                var result = new FileContentResult(bytes, "text/csv")
                {
                    FileDownloadName = $"{jobProfileId}_employerProfiles_{date:yyyyMMdd}.csv"
                };

                return result;
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while downloading employer csv");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the employer profiles.");
                throw new ArgumentException("An error occurred while downloading the employer profile.");
            }
        }

        /// <summary>
        /// Downloads all employer profiles for a specific job profile as a PDF file.
        /// </summary>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <returns>A PDF file containing all employer profiles for the specified job profile.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpGet("downloadpdf/{jobProfileId}", Name = "DownloadPdf")]
        public async Task<IActionResult> DownloadPdf(Guid jobProfileId)
        {
            try
            {
                var downloadOptions = new DownloadOptions
                {
                    Include = Request.Query["Include"].ToString() == "true" ? DownloadType.Include : DownloadType.None,
                    Csv = Request.Query["Csv"].ToString() == "true" ? DownloadType.Csv : DownloadType.None,
                    Pdf = Request.Query["Pdf"].ToString() == "true" ? DownloadType.Pdf : DownloadType.None
                };

                var employerProfiles = await _jobTrackerToolService.GetEmployerProfiles(jobProfileId, downloadOptions);
                var pdf = _jobTrackerToolBusiness.DownloadPdf(jobProfileId, employerProfiles, downloadOptions);

                var date = DateTime.Now;
                var result = new FileContentResult(pdf, "application/pdf")
                {
                    FileDownloadName = $"{jobProfileId}_employerProfiles_{date:yyyyMMdd}.pdf"
                };

                return result;
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Business rule violation occurred while downloading employer pdf");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading the employer profiles.");
                throw new ArgumentException("An error occurred while downloading the employer profile.");
            }
        }

        /// <summary>
        /// Uploads employer profiles for a specific job profile.
        /// </summary>
        /// <param name="formData">The form data containing the file to upload.</param>
        /// <param name="jobProfileId">The ID of the job profile.</param>
        /// <returns>A response indicating the result of the upload operation.</returns>
        /// <exception cref="Exception">Thrown when an unexpected error occurs.</exception>
        [HttpPost("upload/{jobProfileId}", Name = "Upload")]
        public async Task<IActionResult> Upload([FromForm] IFormCollection formData, Guid jobProfileId)
        {
            try
            {
                var file = formData.Files[0];
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                var parsedEmployerData = _jobTrackerToolBusiness.UploadParsing(stream, jobProfileId);

                var result = await _jobTrackerToolService.UploadEmployerProfiles(parsedEmployerData, jobProfileId);

                if (result.Success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the file.");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

