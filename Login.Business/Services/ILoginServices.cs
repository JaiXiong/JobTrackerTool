using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Operations;

namespace Login.Business.Services
{
    public interface ILoginServices
    {
        Task<string> LoginAuth(string username, string password);
        Task<OperationResult> Register(string email, string password);

        string GenerateToken(string username);
        string GenerateRefreshToken(string username);
    }
}
