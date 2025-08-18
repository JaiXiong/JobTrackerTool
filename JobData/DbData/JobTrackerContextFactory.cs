using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using JobTracker.API.Tool.DbData;

namespace JobData.DbData
{
    public class JobTrackerContextFactory : IDesignTimeDbContextFactory<JobTrackerContext>
    {
        public JobTrackerContext CreateDbContext(string[] args)
        {
            // Start from current directory and search upward for the solution
            var currentDirectory = Directory.GetCurrentDirectory();
            var searchDirectory = currentDirectory;
            
            // Look for the startup project containing appsettings.json
            string? startupProjectPath = null;
            
            // Search up the directory tree for JobTracker.API.Tool
            while (searchDirectory != null)
            {
                var candidatePath = Path.Combine(searchDirectory, "JobTracker.API.Tool");
                if (Directory.Exists(candidatePath) && File.Exists(Path.Combine(candidatePath, "appsettings.json")))
                {
                    startupProjectPath = candidatePath;
                    break;
                }
                
                // Move up one directory level
                var parentDirectory = Directory.GetParent(searchDirectory);
                searchDirectory = parentDirectory?.FullName;
            }
            
            // Fallback: if we can't find the proper structure, use current directory
            if (startupProjectPath == null)
            {
                startupProjectPath = currentDirectory;
            }

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(startupProjectPath)
                .AddJsonFile("appsettings.json", optional: true) // Make it optional to prevent crashes
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            // Configure DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<JobTrackerContext>();
            
            // Try to get connection string, provide fallback if not found
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback connection string for design-time (you may want to update this)
                connectionString = "Server=(local),1433;Database=JobTracker01;Integrated Security=True;TrustServerCertificate=Yes";
            }
            
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

            return new JobTrackerContext(optionsBuilder.Options, configuration);
        }
    }
}