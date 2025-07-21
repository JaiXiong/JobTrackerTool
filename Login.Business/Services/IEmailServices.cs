using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Utils.Operations;

namespace Login.Business.Services
{
    public interface IEmailServices
    {
        //Task SendEmailAsync(string to, string subject, string body);
        Task<OperationResult> VerifyEmail(string email);

        Task<OperationResult> SendVerificationEmail(Guid userId, string toEmail, string token);

    }
}
