using JobData.Entities;
using JobTracker.API.Tool.DbData;
using JobTracker.Business.Business;
using JobTracker.Business.Services;
using Login.Business.Business;
using Login.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Resources;
using Utils.Encryption;

namespace JobTracker.Business.DataServices
{
    internal class DataCreationUtil
    {
        public static async Task Create(string[] args)
        {
            var connectionString = "Server=(local),1433;Database=JobTracker01;Integrated Security=True;TrustServerCertificate=Yes";
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    {"ConnectionStrings:DefaultConnection", connectionString}
                })
                .Build();
            
            var serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .AddDbContext<JobTrackerContext>()
                //.AddDbContext<JobTrackerContext>(options =>
                //    options.UseSqlServer(connectionString))
                .AddMemoryCache()
                .AddScoped<JobTrackerToolService>()
                .AddScoped<IJobTrackerContext, JobTrackerContext>()
                .AddScoped<IJobTrackerToolService, JobTrackerToolService>()
                .AddScoped<ILoginServices, LoginServices>()
                .AddScoped<IEmailServices, EmailServices>()
                .AddScoped<IJobTrackerToolBusiness, JobTrackerToolBusiness>()
                .AddScoped<ILoginBusiness, LoginBusiness>()
                .AddScoped<LoginBusiness>()
                .AddSingleton<Encryption>()
                //.AddSingleton(new ResourceManager("JobTracker.Business.Resources", typeof(JobTrackerToolService).Assembly))
                .AddSingleton(new ResourceManager("JobTracker.Business.JobTackerBusinessErrors", typeof(IJobTrackerToolService).Assembly))
                .AddSingleton<IConfiguration>(configuration)
                .BuildServiceProvider();

            var jobTrackerToolService = serviceProvider.GetService<IJobTrackerToolService>();
            var loginServices = serviceProvider.GetService<ILoginServices>();
            var emailServices = serviceProvider.GetService<IEmailServices>();
            var logger = serviceProvider.GetService<ILogger<Program>>();

            if (jobTrackerToolService == null || loginServices == null || emailServices == null || logger == null)
            {
                Console.WriteLine("Failed to initialize services.");
                return;
            }

            try
            {
                await GenerateData(jobTrackerToolService, emailServices, logger);
                //await GenerateData2(jobTrackerToolService, logger);
                //await GenerateDataForAdmin(jobTrackerToolService, logger);
                Console.WriteLine("Data generation completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while generating data.");
            }
        }

        private static async Task GenerateData(IJobTrackerToolService jobTrackerToolService, IEmailServices emailServices, ILogger logger)
        {
            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                var userProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    LatestUpdate = DateTime.UtcNow,
                    Name = $"User {i}",
                    Email = $"user{i}@example.com",
                    Password = "password",
                    Phone = $"555-010{i}",
                    Address = $"123 Main St Apt {i}",
                    City = "City",
                    State = "State",
                    Zip = "12345",
                    IsEmailVerified = true
                };

                await jobTrackerToolService.AddUserProfile(userProfile);

                var emailConfirmation = new EmailConfirmation
                {
                    Id = Guid.NewGuid(),
                    UserProfileId = userProfile.Id,
                    Token = Guid.NewGuid().ToString(),
                    ExpirationDate = DateTime.UtcNow.AddMinutes(5),
                    CreatedAt = DateTime.UtcNow
                };

                await emailServices.AddEmailConfirmation(emailConfirmation);

                for (int j = 0; j < 10; j++)
                {
                    var jobProfile = new JobProfile
                    {
                        Id = Guid.NewGuid(),
                        UserProfileId = userProfile.Id,
                        Date = DateTime.UtcNow,
                        LatestUpdate = DateTime.UtcNow,
                        ProfileName = $"Job Profile {j} for User {i}"
                    };

                    await jobTrackerToolService.AddJobProfile(jobProfile);

                    for (int k = 0; k < 100; k++)
                    {
                        int randomNumber = random.Next(0, 1);
                        var employerProfile = new EmployerProfile
                        {
                            Id = Guid.NewGuid(),
                            JobProfileId = jobProfile.Id,
                            Name = $"Employer {k} for Job Profile {j}",
                            Title = "Title",
                            Address = "Address",
                            City = "City",
                            State = "State",
                            Zip = "12345",
                            Phone = "555-1234",
                            Email = "employer@example.com",
                            Website = "http://example.com"
                        };

                        await jobTrackerToolService.AddEmployerProfile(employerProfile);

                        if (randomNumber == 0)
                        {
                            var jobAction = new JobAction
                            {
                                Id = Guid.NewGuid(),
                                EmployerProfileId = employerProfile.Id,
                                Action = "Apply",
                                Method = "Email",
                                ActionResult = "Success"
                            };

                            await jobTrackerToolService.AddJobAction(employerProfile.Id, jobAction);

                            var detail = new Detail
                            {
                                Id = Guid.NewGuid(),
                                EmployerProfileId = employerProfile.Id,
                                Date = DateTime.UtcNow,
                                LatestUpdate = DateTime.UtcNow,
                                Comments = "Rejected",
                                Updates = "Rejected"
                            };

                            await jobTrackerToolService.AddDetail(employerProfile.Id, detail);
                        }
                        else
                        {
                            var jobAction = new JobAction
                            {
                                Id = Guid.NewGuid(),
                                EmployerProfileId = employerProfile.Id,
                                Action = "Apply",
                                Method = "Email",
                                ActionResult = "Success"
                            };

                            await jobTrackerToolService.AddJobAction(employerProfile.Id, jobAction);

                            var detail = new Detail
                            {
                                Id = Guid.NewGuid(),
                                EmployerProfileId = employerProfile.Id,
                                Date = DateTime.UtcNow,
                                LatestUpdate = DateTime.UtcNow,
                                Comments = "Call back",
                                Updates = "Follow-up scheduled"
                            };

                            await jobTrackerToolService.AddDetail(employerProfile.Id, detail);
                        }

                            //await jobTrackerToolService.AddDetail(employerProfile.Id, detail);
                    }
                }
            }
        }

        private static async Task GenerateData2(JobTrackerToolService jobTrackerToolService, ILogger logger)
        {
            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                var userProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    LatestUpdate = DateTime.UtcNow,
                    Name = $"User {i}",
                    Email = $"user{i}@example.com",
                    Password = "password",
                    Phone = $"555-010{i}",
                    Address = $"123 Main St Apt {i}",
                    City = "City",
                    State = "State",
                    Zip = "12345"
                };

                await jobTrackerToolService.AddUserProfile(userProfile);

                for (int j = 0; j < 3; j++)
                {
                    var jobProfile = new JobProfile
                    {
                        Id = Guid.NewGuid(),
                        UserProfileId = userProfile.Id,
                        Date = DateTime.UtcNow,
                        LatestUpdate = DateTime.UtcNow,
                        ProfileName = $"Job Profile {j} for User {i}"
                    };

                    await jobTrackerToolService.AddJobProfile(jobProfile);

                    for (int k = 0; k < 50; k++)
                    {
                        var employerProfile = new EmployerProfile
                        {
                            Id = Guid.NewGuid(),
                            JobProfileId = jobProfile.Id,
                            Name = $"Employer {k} for Job Profile {j}",
                            Title = "Title",
                            Address = "Address",
                            City = "City",
                            State = "State",
                            Zip = "12345",
                            Phone = "555-1234",
                            Email = "employer@example.com",
                            Website = "http://example.com"
                        };

                        await jobTrackerToolService.AddEmployerProfile(employerProfile);

                        var jobAction = new JobAction
                        {
                            Id = Guid.NewGuid(),
                            EmployerProfileId = employerProfile.Id,
                            Action = "Apply",
                            Method = "Email",
                            ActionResult = "Success"
                        };

                        await jobTrackerToolService.AddJobAction(employerProfile.Id, jobAction);

                        var detail = new Detail
                        {
                            Id = Guid.NewGuid(),
                            EmployerProfileId = employerProfile.Id,
                            Date = DateTime.UtcNow,
                            LatestUpdate = DateTime.UtcNow,
                            Comments = "Initial contact",
                            Updates = "Follow-up scheduled"
                        };

                        await jobTrackerToolService.AddDetail(employerProfile.Id, detail);
                    }
                }
            }
        }

        private static async Task GenerateDataForAdmin(JobTrackerToolService jobTrackerToolService, ILogger logger)
        {
            var random = new Random();

            //for (int i = 0; i < 2; i++)
            //{
                var userProfile = new UserProfile
                {
                    //Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    LatestUpdate = DateTime.UtcNow,
                    Name = $"Admin",
                    Email = $"user@example.com",
                    Password = "password",
                    Phone = $"555-010",
                    Address = $"123 Main St Apt",
                    City = "City",
                    State = "State",
                    Zip = "12345"
                };

                await jobTrackerToolService.AddUserProfile(userProfile);

                for (int j = 0; j < 6; j++)
                {
                    var jobProfile = new JobProfile
                    {
                        //Id = Guid.NewGuid(),
                        UserProfileId = userProfile.Id,
                        Date = DateTime.UtcNow,
                        LatestUpdate = DateTime.UtcNow,
                        ProfileName = $"Job Profile {j} for User"
                    };

                    await jobTrackerToolService.AddJobProfile(jobProfile);

                    for (int k = 0; k < 5; k++)
                    {
                        var employerProfile = new EmployerProfile
                        {
                            //Id = Guid.NewGuid(),
                            JobProfileId = jobProfile.Id,
                            Date = DateTime.UtcNow,
                            LatestUpdate = DateTime.UtcNow,
                            Name = $"Employer {k} for Job Profile {j}",
                            Title = "Title",
                            Address = "Address",
                            City = "City",
                            State = "State",
                            Zip = "12345",
                            Phone = "555-1234",
                            Email = "employer@example.com",
                            Website = "http://example.com"
                        };

                        await jobTrackerToolService.AddEmployerProfile(employerProfile);

                        var jobAction = new JobAction
                        {
                            //Id = Guid.NewGuid(),
                            EmployerProfileId = employerProfile.Id,
                            Date = DateTime.UtcNow,
                            LatestUpdate = DateTime.UtcNow,
                            Action = "Apply",
                            Method = "Email",
                            ActionResult = "Success"
                        };

                        await jobTrackerToolService.AddJobAction(employerProfile.Id, jobAction);

                        var detail = new Detail
                        {
                            //Id = Guid.NewGuid(),
                            EmployerProfileId = employerProfile.Id,
                            Date = DateTime.UtcNow,
                            LatestUpdate = DateTime.UtcNow,
                            Comments = "Initial contact",
                            Updates = "Follow-up scheduled"
                        };

                        await jobTrackerToolService.AddDetail(employerProfile.Id, detail);
                    }
                }
           // }
        }
    }
}
