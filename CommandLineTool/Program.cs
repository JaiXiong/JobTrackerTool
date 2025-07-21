using JobData.Entities;
using JobTracker.API.Tool.DbData;
using JobTracker.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration.EnvironmentVariables;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting XML generation...");

        // Initialize the service (use dependency injection in a real-world scenario)
        IJobTrackerToolService jobTrackerToolService = InitializeJobTrackerToolService();

        try
        {
            var employerProfiles = await jobTrackerToolService.GetAllEmployerProfiles();

            // Serialize to XML
            var xmlSerializer = new XmlSerializer(typeof(List<EmployerProfile>));
            var fileName = $"EmployerProfiles_{DateTime.Now:yyyyMMdd}.xml";

            using (var writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                xmlSerializer.Serialize(writer, employerProfiles);
            }

            Console.WriteLine($"XML file generated successfully: {fileName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    //private static string GetConnectionString()
    //{
    //    var configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json")
    //        .Build();

    //    return configuration.GetConnectionString("DefaultConnection");
    //}

    private static IJobTrackerToolService InitializeJobTrackerToolService()
    {
        //var logger = new LoggerFactory().CreateLogger<JobTrackerToolService>();
        //var optionsBuilder = new DbContextOptionsBuilder<JobTrackerContext>();
        //optionsBuilder.UseSqlServer("Server=(local), 1433;Database=JobTracker01; Integrated Security=True; TrustServerCertificate=Yes");

        //var dataContext = new JobTrackerContext(optionsBuilder.Options);

        //// Use MemoryCache instead of IMemoryCache directly
        //var memoryCache = new MemoryCache(new MemoryCacheOptions());

        //return new JobTrackerToolService(dataContext, logger, memoryCache);
        // Set up configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Set up logging
        var logger = new LoggerFactory().CreateLogger<JobTrackerToolService>();

        // Set up DbContext
        var optionsBuilder = new DbContextOptionsBuilder<JobTrackerContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        var dataContext = new JobTrackerContext(optionsBuilder.Options, configuration);

        // Memory cache remains the same
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        return new JobTrackerToolService(dataContext, logger, memoryCache);
    }
}