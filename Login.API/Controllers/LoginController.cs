using IdentityModel.Client;
using JobData.Entities;
using Login.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Utils.CustomExceptions;

namespace Login.API.Controllers
{
    /// <summary>
    /// Controller for handling login and user registration.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly LoginServices _loginServices;
        private readonly EmailServices _emailServices;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="loginServices">The login services instance.</param>
        /// <param name="emailServices"></param>
        /// <param name="configuration">The configuration instance.</param>
        public LoginController(ILogger<LoginController> logger, LoginServices loginServices, EmailServices emailServices, IConfiguration configuration)
        {
            _logger = logger;
            _loginServices = loginServices;
            _emailServices = emailServices;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and generates tokens.
        /// </summary>
        /// <param name="loginRequest">The login request containing email and password.</param>
        /// <returns>A response containing the user ID, username, access token, and refresh token.</returns>
        [HttpPost("loginauth", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                throw new ArgumentNullException("Username or password invalid");
            }

            try
            {
                var username = loginRequest.Email;
                var password = loginRequest.Password;

                var userId = await _loginServices.LoginAuth(username, password);

                if (userId != null)
                {
                    var access_token = _loginServices.GenerateToken(username);
                    var refresh_token = _loginServices.GenerateRefreshToken(username);
                    return Ok(new { id = userId, name = username, access_token, refresh_token });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registerRequest">The registration request containing email and password.</param>
        /// <returns>A response indicating the result of the registration operation.</returns>
        [HttpPost("registeruser", Name = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password))
            {
                throw new ArgumentNullException("Username or password invalid");
            }

            try
            {
                //await _emailServices.VerifyEmail(registerRequest.Email);
                var result = await _emailServices.VerifyEmail(registerRequest.Email);

                if (result.Success)
                {
                    // Send verification email
                    await _emailServices.SendEmail(registerRequest.Email, "Email Verification", "Please verify your email by clicking the link provided.");
                    //await _loginServices.Register(registerRequest.Email, registerRequest.Password);
                }
                else
                {
                    return BadRequest(new { result.Message, result.Errors });
                }

                //await _loginServices.Register(registerRequest.Email, registerRequest.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets the current authenticated user.
        /// </summary>
        /// <returns>The current authenticated user.</returns>
        [Authorize]
        [HttpGet("currentuser", Name = "CurrentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            try
            {
                var user = await _loginServices.GetCurrentUser();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the current user.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <returns>A response containing the new refresh token.</returns>
        [Authorize]
        [HttpPost("refreshtoken", Name = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return BadRequest("Invalid token");
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var refreshToken = await _loginServices.RefreshToken(token);
                return Ok(new { refreshToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing the token.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Gets the connection string for the database.
        /// </summary>
        /// <returns>A response containing the connection string.</returns>
        //[HttpGet("database", Name = "DB")]
        //public async Task<IActionResult> GetConnectionString()
        //{
        //    try
        //    {
        //        //var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        //        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //        return Ok(new { connectionString });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while getting the connection string.");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}
