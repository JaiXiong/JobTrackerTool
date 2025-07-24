using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Login.Business.Business
{
    public class LoginBusiness : ILoginBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        private ILogger<LoginBusiness> _logger;
        private ResxFormat _resx;
        public LoginBusiness(IConfiguration configuration, ResourceManager resourceManager, ILogger<LoginBusiness> logger)
        {
            _configuration = configuration;
            _resourceManager = new ResourceManager("Login.Business.LoginBusinessErrors", typeof(LoginBusiness).Assembly);
            _resx = new ResxFormat(_resourceManager);
            _logger = logger;
        }

        public virtual ClaimsPrincipal GetTokenInfo(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = _configuration["JWT-SECRET-KEY"];

            if (string.IsNullOrEmpty(secret))
            {
                throw new BusinessException(_resx.Create("JWTInvalid"));
            }

            var key = Encoding.UTF8.GetBytes(secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return principal;
        }
    }
}
