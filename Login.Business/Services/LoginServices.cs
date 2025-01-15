using JobData.Entities;
using JobTracker.API.Tool.DbData;
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
            _resourceManager = resourceManager;
            _dbContext = context;
            _configuration = configuration;
            _encryption = encryption;
            _loginBusiness = loginBusiness;
        }
        public async Task<string> LoginAuth(string email, string pw)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException(_resourceManager.GetString("UserEmailError"));
            }

            if (string.IsNullOrEmpty(pw))
            {
                throw new ArgumentException(_resourceManager.GetString("PasswordError"));
            }
            var delimiter = new char[] { '@' };
            var username = email.Split(delimiter)[0];
            var hashedPassword = _encryption.HashPassword(pw);
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == username);
            //var pass = await _dbContext.UserProfiles.FirstOrDefaultAsync(p => p.Password == _encyption.HashPassword(pw));

            if (user == null || !_encryption.VerifyPassword(pw, hashedPassword))
            {
                throw new ArgumentException(_resourceManager.GetString("Login Creditials Failed"));
            }

            return user.Id.ToString();
        }

        public string GenerateToken(string username)
        {
            var secretKey = _configuration["JWT_SECRET_KEY"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("JWT_SECRET_KEY is not configured.");
            }
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            //var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT_SECRET_KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            var secretKey = _configuration["JWT_SECRET_KEY"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("JWT_SECRET_KEY is not configured.");
            }
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            //var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT_SECRET_KEY"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:RefreshExpiresInMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> RefreshToken(string token)
        {
            var tokenInfo = _loginBusiness.GetTokenInfo(token);
            if (tokenInfo == null)
            {
                throw new ArgumentException("Invalid token");
            }

            var exp = tokenInfo.Claims.FirstOrDefault(c => c.Type == "exp").Value;
            var user = tokenInfo.Claims.FirstOrDefault(c => c.Type == "sub").Value ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var email = tokenInfo.Claims.FirstOrDefault(c => c.Type == "email").Value ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            var userExist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == user && u.Email == email);

            if (userExist == null) 
            {
                throw new ArgumentException("Invalid token");
            }

            if (exp == null)
            {
                throw new ArgumentException("Invalid token");
            }

            return GenerateRefreshToken(user);
        }

        public async Task<UserProfile> GetCurrentUser()
        {
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException(_resourceManager.GetString("UserNotExist"));
            }

            return user;
        }

        public async Task Register(string email, string pw)
        {
            var exist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);

            if (exist != null)
            {
                throw new ArgumentException(_resourceManager.GetString("UserExist"));
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

        //public string LoginEncrypt(string item)
        //{
        //    //TODO
        //    var encryptedItem = item;
        //    return encryptedItem;
        //}

        //public string LoginDecrpyt(string item)
        //{
        //    //TODO
        //    var decryptedItem = item;

        //    return decryptedItem;
        //}
    }
}
