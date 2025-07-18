﻿using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Login.Business.Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using Utils.CustomExceptions;
using Utils.Encryption;
using Utils.Operations;

namespace Login.Business.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ResourceManager _resourceManager;
        private readonly IJobProfileContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly Encryption _encryption;
        private readonly LoginBusiness _loginBusiness;
        private readonly ILogger<LoginServices> _logger;
        private ResxFormat _resx;

        public LoginServices(ResourceManager resourceManager, IJobProfileContext context, IConfiguration configuration, Encryption encryption, LoginBusiness loginBusiness, ILogger<LoginServices> logger)
        {
            _resourceManager = new ResourceManager("Login.Business.LoginBusinessErrors", typeof(LoginServices).Assembly);
            _resx = new ResxFormat(_resourceManager);
            _dbContext = context;
            _configuration = configuration;
            _encryption = encryption;
            _loginBusiness = loginBusiness;
            _logger = logger;
        }
        public async Task<string> LoginAuth(string email, string pw)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pw))
            {
                throw new BusinessException(_resx.Create("LoginError"));
            }

            var delimiter = new char[] { '@' };
            var username = email.Split(delimiter)[0];
            var hashedPassword = _encryption.HashPassword(pw);
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null || !_encryption.VerifyPassword(pw, hashedPassword))
            {
                throw new BusinessException(_resx.Create("LoginError"));
            }

            return user.Id.ToString();
        }

        public string GenerateToken(string username)
        {
            var secretKey = _configuration["JWT-SECRET-KEY"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new BusinessException(_resx.Create("JWTInvalid"));
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
                throw new BusinessException(_resx.Create("JWTInvalid"));
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
            var secretKey = _configuration["JWT-SECRET-KEY"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new BusinessException(_resx.Create("JWTInvalid"));
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
                throw new BusinessException(_resx.Create("JWTInvalid"));
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
                throw new BusinessException(_resx.Create("TokenInvalid"));
            }
            var expClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "exp");
            var userClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "sub") ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var emailClaim = tokenInfo.Claims.FirstOrDefault(c => c.Type == "email") ?? tokenInfo.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (expClaim == null || userClaim == null || emailClaim == null)
            {
                throw new BusinessException(_resx.Create("TokenInvalid"));
            }

            var exp = expClaim.Value;
            var user = userClaim.Value;
            var email = emailClaim.Value;

            var userExist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Name == user && u.Email == email);

            if (userExist == null || exp == null) 
            {
                throw new BusinessException(_resx.Create("TokenInvalid"));
            }

            return GenerateRefreshToken(user);
        }

        public async Task<UserProfile> GetCurrentUser()
        {
            var user = await _dbContext.UserProfiles.FirstOrDefaultAsync();

            if (user == null)
            {
                throw new BusinessException(_resx.Create("UserNotExist"));
            }

            return user;
        }

        public async Task<OperationResult> Register(string email, string pw)
        {
            var exist = await _dbContext.UserProfiles.FirstOrDefaultAsync(u => u.Email == email);

            if (exist != null)
            {
                return OperationResult.CreateFailure(_resx.Create("UserExist"));
            }

            var delimiter = new char[] { '@' };
           
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

            return OperationResult.CreateSuccess("User registered successfully.");
        }
    }
}
