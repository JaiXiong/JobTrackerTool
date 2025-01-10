using IdentityModel.Client;
using Login.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Login.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly LoginServices _loginServices;

        public LoginController(ILogger<LoginController> logger, LoginServices loginServices)
        {
            _logger = logger;
            _loginServices = loginServices;
        }

        [HttpPost("loginauth", Name = "Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            //TODO
            //for now we have no login since this is low priority
            //ideally we need to encrypt and salt the username and password using some engine etc. store it in db
            //then when we check we need to decrypt and unsalt to get these values again.
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                throw new ArgumentNullException("Username or password invalid");
            }

            try
            {
                
                //var delimiter = new char[] { '@' };
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
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("registeruser", Name = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            //TODO
            if (string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password))
            {
                throw new ArgumentNullException("Username or password invalid");
            }

            try
            {
                await _loginServices.Register(registerRequest.Email, registerRequest.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpGet("currentuser", Name = "CurrentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            //TODO
            try
            {
                var user = await _loginServices.GetCurrentUser();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("refreshtoken", Name = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            //TODO
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return BadRequest("Invalid token");
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                var refreshToken =  await _loginServices.RefreshToken(token);
                return Ok(new { refreshToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
