using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Login.Business.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using Utils.Encryption;

namespace Login.Business.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ResourceManager _resourceManager;
        private readonly IJobProfileContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly Encryption _encryption;
        private readonly LoginBusiness _loginBusiness;

        public LoginServices(ResourceManager resourceManager, IJobProfileContext context, IConfiguration configuration, Encryption encryption, LoginBusiness loginBusiness)
        {
            _resourceManager = new ResourceManager("Login.Business.LoginBusinessErrors", typeof(LoginServices).Assembly);
            _dbContext = context;
            _configuration = configuration;
            _encryption = encryption;
            _loginBusiness = loginBusiness;
        }
        public async Task<string> LoginAuth(string email, string pw)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pw))
            {
                var errorMessage = _resourceManager.GetString("JWTInvalid") ?? "JWT invalid error";
                throw new ArgumentException(string.Format(errorMessage));
            }

            var delimiter = new char[] { '@' };
            var username = email.Split(delimiter)[0];
            var hashedPassword = _encryption.HashPassword(pw);
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null || !_encryption.VerifyPassword(pw, hashedPassword))
            {
                var errorMessage = _resourceManager.GetString("LoginAttemptError") ?? "Login attempt error";
                throw new ArgumentException(string.Format(errorMessage));
            }

            return user.Id.ToString();
        }

        public string GenerateToken(string username)
        {
            var secretKey = _configuration["JWT_SECRET_KEY"];
            if (string.IsNullOrEmpty(secretKey))
            {
                var errorMessage = _resourceManager.GetString("JWTInvalid") ?? "JWT invalid error";
                throw new ArgumentException(string.Format(errorMessage));
            }
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiresInMinutes = _configuration["Jwt:ExpiresInMinutes"];
            if (string.IsNullOrEmpty(expiresInMinutes))
            {
                throw new ArgumentException("JWT expiration time is not configured properly.");
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(expiresInMinutes)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            var secretKey = _configuration["JWT_SECRET_KEY"];
            if (string.IsNullOrEmpty(secretKey))
            {
                var errorMessage = _resourceManager.GetString("JWTInvalid") ?? "JWT invalid error";
                throw new ArgumentException(string.Format(errorMessage));
            }
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var refreshExpiresInMinutes = _configuration["Jwt:RefreshExpiresInMinutes"];
            if (string.IsNullOrEmpty(refreshExpiresInMinutes))
            {
                throw new ArgumentException("JWT refresh expiration time is not configured properly.");
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(refreshExpiresInMinutes)),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> RefreshToken(string token)
        {
            var tokenInfo = _loginBusiness.GetTokenInfo(token);

            if (tokenInfo == null)
            {
                throw new ArgumentException(_resourceManager.GetString("TokenInvalid"));
            }
            var expClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "exp");
            var userClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "sub") ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var emailClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "email") ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (expClaim == null || userClaim == null || emailClaim == null)
            {
                throw new ArgumentException(_resourceManager.GetString("TokenInvalid"));
            }

            var exp = expClaim.Value;
            var user = userClaim.Value;
            var email = emailClaim.Value;

            var userExist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == user && u.Email == email);

            if (userExist == null || exp == null) 
            {
                throw new ArgumentException(_resourceManager.GetString("TokenInvalid"));
            }

            return GenerateRefreshToken(user);
        }

        public async Task<UserProfile> GetCurrentUser()
        {
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync();

            if (user == null)
            {
                var errorMessage = _resourceManager.GetString("UserNotExist") ?? "User does not exist";
                throw new ArgumentException(string.Format(errorMessage));
            }

            return user;
        }

        public async Task Register(string email, string pw)
        {
            var exist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);

            if (exist != null)
            {
                var errorMessage = _resourceManager.GetString("UserExist");
                if (errorMessage == null)
                {
                    throw new ArgumentException("User already exists", nameof(email));
                }
                throw new ArgumentException(string.Format(errorMessage, email), nameof(email));
            }

            var delimiter = new char[] { '@' };
            try
            {
                var username = email.Substring(0, email.IndexOf('@'));

                var user = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    LatestUpdate = DateTime.Now,
                    Name = email.Substring(0, email.IndexOf('@')),
                    Email = email,
                    Password = _encryption.HashPassword(pw)
                };

                await _dbContext.UserProfiles.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
