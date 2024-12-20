﻿using JobTracker.API.Tool.DbData;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;

namespace Login.Business.Services
{
    public class LoginServices
    {
        private readonly ResourceManager _resourceManager;
        private readonly JobProfileContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginServices(ResourceManager resourceManager, JobProfileContext context, IConfiguration configuration)
        {
            _resourceManager = resourceManager;
            _dbContext = context;
            _configuration = configuration;
        }
        public async Task<string> LoginAuth(string username, string pw)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(_resourceManager.GetString("UsernameError"));
            }

            if (string.IsNullOrEmpty(pw))
            {
                throw new ArgumentException(_resourceManager.GetString("PasswordError"));
            }

            var user = _dbContext.UserProfiles.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                throw new ArgumentException(_resourceManager.GetString("UserNotExist"));
            }

            return user.Id.ToString();
        }

        public string GenerateToken(string username, string pw)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["JWT_SECRET_KEY"]));
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

        public string LoginEncrypt(string item)
        {
            //TODO
            var encryptedItem = item;
            return encryptedItem;
        }

        public string LoginDecrpyt(string item)
        {
            //TODO
            var decryptedItem = item;

            return decryptedItem;
        }
    }
}
