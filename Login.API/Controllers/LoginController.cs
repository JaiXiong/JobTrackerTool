using Login.Business.Services;
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

        //[HttpPost("login", Name = "Login")]
        //public async Task<IActionResult> Login(string username, string password)
        //{
        //    //TODO
        //    //for now we have no login since this is low priority
        //    //ideally we need to encrypt and salt the username and password using some engine etc. store it in db
        //    //then when we check we need to decrypt and unsalt to get these values again.
        //    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        //    {
        //        throw new ArgumentNullException("Username or password invalid");
        //    }

        //    try
        //    {
        //        var userid = await _loginServices.LoginAuth(username, password);

        //        //return CreatedAtAction(nameof(Login), new { username = username, password = password });
        //        return Ok(new { id = userid, name = username});
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while creating the employer profile.");
        //        return StatusCode(500, "Internal server error");
        //    }

        //}

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
                
                var delimiter = new char[] { '@' };
                var username = _loginServices.LoginDecrpyt(loginRequest.Email.Split(delimiter)[0]);
                var password = _loginServices.LoginDecrpyt(loginRequest.Password);

                var userid = await _loginServices.LoginAuth(username, password);

                var token = _loginServices.GenerateToken(username, password);

                //return CreatedAtAction(nameof(Login), new { username = username, password = password });
                return Ok(new { id = userid, name = username, token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the employer profile.");
                return StatusCode(500, "Internal server error");
            }

        }


    }
}
