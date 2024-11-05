using Humanizer.Localisation;
using System.Resources;
using System.Security.AccessControl;

namespace Login.Business.Services
{
    public class LoginServices
    {
        private readonly ResourceManager _resourceManager;

        public LoginServices(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
        public void Login(string username, string pw)
        {
            if (string.IsNullOrEmpty(username))
            {
                //throw new ArgumentException(Resources.GetResource("UsernameError"));
                throw new ArgumentException(_resourceManager.GetString("UsernameError"));
            }

            if (string.IsNullOrEmpty(pw))
            {
                //throw new ArgumentException(Resources.GetResource("PasswordError"));
                throw new ArgumentException(_resourceManager.GetString("PasswordError"));
            }



        }
    }
}
