using Humanizer.Localisation;
using JobTracker.API.Tool.DbData;
using System.Resources;
using System.Security.AccessControl;

namespace Login.Business.Services
{
    public class LoginServices
    {
        private readonly ResourceManager _resourceManager;
        private readonly JobProfileContext _dbContext;

        public LoginServices(ResourceManager resourceManager, JobProfileContext context)
        {
            _resourceManager = resourceManager;
            _dbContext = context;
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
    }
}
