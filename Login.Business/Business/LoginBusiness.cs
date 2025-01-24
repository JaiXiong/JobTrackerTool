using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Login.Business.Business
{
    public class LoginBusiness: ILoginBusiness
    {
        private readonly IConfiguration _configuration;
        private readonly ResourceManager _resourceManager;
        public LoginBusiness(IConfiguration configuration, ResourceManager resourceManager)
        {
            _configuration = configuration;
            _resourceManager = resourceManager;
        }

        public ClaimsPrincipal GetTokenInfo(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT_SECRET_KEY"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format(_resourceManager.GetString("JWTInvalid")));
            }
        }
    }
}
