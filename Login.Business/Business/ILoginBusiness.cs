using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Login.Business.Business
{
    public interface ILoginBusiness
    {
        ClaimsPrincipal GetTokenInfo(string token);
    }
}
