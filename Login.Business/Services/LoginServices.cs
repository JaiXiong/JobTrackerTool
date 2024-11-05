using Humanizer.Localisation;
using System.Resources;

namespace Login.Business.Services
{
    public class LoginServices
    {
        public void Login(string username, string pw)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException(Resources.GetResource("UsernameError"));
            }

            if (string.IsNullOrEmpty(pw))
            {
                throw new ArgumentException(Resources.GetResource("PasswordError"));
            }



        }
    }
}
