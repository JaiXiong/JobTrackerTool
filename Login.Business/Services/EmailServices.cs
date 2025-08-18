using JobData.Entities;
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
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;
using Utils.Operations;
using static System.Net.WebRequestMethods;

namespace Login.Business.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly ILogger<EmailServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJobTrackerContext _dbContext;
        private ResxFormat _resx;

        public EmailServices(ILogger<EmailServices> logger, IConfiguration configuration, IJobTrackerContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = context;
            _resx = new ResxFormat(new ResourceManager("Login.Business.EmailErrors", typeof(EmailServices).Assembly));
        }

        public async Task<OperationResult> VerifyEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                throw new BusinessException(_resx.Create("EmailVerificationFailed"));
            }

            var exist = await _dbContext.UserProfiles.AnyAsync(u => u.Email == email);

            if (exist)
            {
                throw new BusinessException(_resx.Create("EmailAlreadyExists"));
            }

            return OperationResult.CreateSuccess("Email is valid and does not exist in the system.");
        }

        public async Task<OperationResult> SendVerificationEmail(Guid userId, string toEmail, string token)
        {
            var subject = "Your JobTracker Email Verification";
            var baseUrl = _configuration["EmailConfirmation:BaseUrl"];
            var confirmationPath = _configuration["EmailConfirmation:Path"] ?? "/login/confirm-email";
            var confirmationLink = $"{baseUrl}{confirmationPath}?token={Uri.EscapeDataString(token)}";

            var body = $"<p>Please verify your email by clicking the link below:</p><a href='{confirmationLink}'>Verify Email</a>";

            _logger.LogInformation($"Sending email to {toEmail} with subject {subject}");

            var smtpSection = _configuration.GetSection("SMTP2Go");

            if (smtpSection == null)
            {
                throw new BusinessException("SMTP settings are not configured.");
            }

            var smtpClient = new SmtpClient(smtpSection["Host"])
            {
                Port = int.Parse(smtpSection["Port"]),
                //Credentials = new NetworkCredential(smtpSection["Username"], smtpSection["Password"]),
                Credentials = new NetworkCredential(_configuration["EMAIL-USERNAME"], _configuration["EMAIL-PASSWORD"]),
                EnableSsl = bool.Parse(smtpSection["EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                //From = new MailAddress(smtpSection["FromEmail"], "Admin"),
                From = new MailAddress(_configuration["EMAIL-DOMAIN"], "Admin"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            CreateEmailConfirmation(userId, token);

            await smtpClient.SendMailAsync(mailMessage);

            //return Task.CompletedTask;
            return OperationResult.CreateSuccess("Email sent successfully.");
        }

        private Task<OperationResult?> SendVerifiedLink(string toEmail, string subject, string body, string token)
        {

            throw new NotImplementedException();
        }

        private async void CreateEmailConfirmation(Guid userId, string token)
        {
            EmailConfirmation emailConfirmation = new EmailConfirmation
            {
                Id = Guid.NewGuid(),
                UserProfileId = userId,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddMinutes(60),
                CreatedAt = DateTime.UtcNow,
            };

            try
            {
                await _dbContext.EmailConfirmations.AddAsync(emailConfirmation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating email confirmation.");
                throw new BusinessException(_resx.Create("EmailConfirmationCreationFailed"));
            }
        }

       public async Task<OperationResult> AddEmailConfirmation(EmailConfirmation emailConfirmationProfile)
        {
            if (emailConfirmationProfile == null)
            {
                throw new ArgumentNullException(nameof(emailConfirmationProfile), "Email confirmation profile cannot be null.");
            }

            try
            {
                await _dbContext.EmailConfirmations.AddAsync(emailConfirmationProfile);
                await _dbContext.SaveChangesAsync();
                return OperationResult.CreateSuccess("Email confirmation added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding email confirmation.");
                return OperationResult.CreateFailure("Failed to add email confirmation.");
            }
        }
    }
}
