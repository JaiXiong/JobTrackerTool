using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;


namespace AzureFunctions.FunctionCalls
{
    public class FunctionCalls
    {
        readonly ILogger<FunctionCalls> _logger;
        public FunctionCalls(ILogger<FunctionCalls> logger)
        {
            _logger = logger;
        }

        [Function("GetConnectionString")]
        public async Task<HttpResponseData> GetConnectionString([HttpTrigger(AuthorizationLevel.Function, "get", Route = "func/database")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            //Later we will create something like this to get the API URL from environment variables
            //Azure Functions serves as a proxy to an external API that provides the connection string.
            // It also runs small code in the cloud without the need to know the Infrastructure. (main advantage)
            //Azure logic apps is more a low-code solution for integrating apps, data, and services.
            //var apiUrl = Environment.GetEnvironmentVariable("ExternalApiUrl");
            var apiUrl = "https://jobtrackerloginapi.azurewebsites.net/api/Login/database";
            
            using var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Optionally, deserialize if you expect JSON
                // var result = JsonSerializer.Deserialize<YourType>(content);

                var httpResponse = req.CreateResponse(HttpStatusCode.OK);
                httpResponse.Headers.Add("Content-Type", "application/json");
                await httpResponse.WriteStringAsync(content);

                return httpResponse;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error fetching connection string from API.");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("Error fetching connection string from API.");
                return errorResponse;
            }
        }
    }
}
