using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Encryption
{
    public class Encryption
    {
        public string HashPassword(string data)
        {
            return BCrypt.Net.BCrypt.HashPassword(data);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string Encrypt(string data)
        {
            //TODO: Implement encryption if we need later


            return data;
        }

        public string Decrypt(string data)
        {
            //TODO: Implement decryption if we need later
            return data;
        }
    }
}
