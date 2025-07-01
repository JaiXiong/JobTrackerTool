using JobTracker.API.Tool.DbData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Operations;

namespace Login.Business.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly ILogger<EmailServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJobProfileContext _dbContext;
        private ResxFormat _resx;

        public EmailServices(ILogger<EmailServices> logger, IConfiguration configuration,IJobProfileContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = context;
        }

        public async Task<OperationResult> VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new BusinessException(_resx.Create("EmailError"));
            }

            var exist = await _dbContext.UserProfiles.AnyAsync(u => u.Email == email);

            if (exist)
            {
                throw new BusinessException(_resx.Create("EmailAlreadyExists"));
            }

            return OperationResult.CreateSuccess("Email is valid and does not exist in the system.");
        }

        public async Task SendEmail(string toEmail, string subject, string body)
        {
            // Implementation for sending email
            _logger.LogInformation($"Sending email to {toEmail} with subject {subject}");

            var smtpSection = _configuration.GetSection("SMTP2Go");

            if (smtpSection == null)
            {
                throw new BusinessException("SMTP settings are not configured.");
            }

            var smtpClient = new SmtpClient(smtpSection["Host"])
            {
                Port = int.Parse(smtpSection["Port"]),
                Credentials = new NetworkCredential(smtpSection["Username"], smtpSection["Password"]),
                EnableSsl = bool.Parse(smtpSection["EnableSsl"])
            };

            Console.WriteLine($"SMTP Client configured with Username: {smtpSection["Username"]}, Pass: {smtpSection["Password"]}, Domain: {smtpSection["FromEmail"]}");
            Console.WriteLine($"SMTP Client configured with FromName: {smtpSection["FromName"]}");
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSection["FromName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);

            //return Task.CompletedTask;
        }
    }
}
