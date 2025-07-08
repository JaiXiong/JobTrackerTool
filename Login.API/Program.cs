using Azure.Identity;
using JobTracker.API.Tool.DbData;
using Login.Business.Business;
using Login.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Resources;
using System.Text;
using Utils.Encryption;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

var builder = WebApplication.CreateBuilder(args);

//Set up Serilog
var logDirectory = builder.Configuration["Logging:LogDirectory:LogPath"] ?? "Logs";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Configuration.AddEnvironmentVariables();

builder.Configuration.AddAzureKeyVault(
    new Uri("https://jobappvault.vault.azure.net/"),
    new DefaultAzureCredential());

//var jwtSecretKey = builder.Configuration["JWT_SECRET_KEY"];
//var jwtSecretKey = builder.Configuration["JWT-SECRET-KEY"] ?? builder.Configuration["JWT_SECRET_KEY"];

string? jwtSecretKey;

if (builder.Environment.IsDevelopment())
{
    jwtSecretKey = Environment.GetEnvironmentVariable("Jwt__Key");

}
else
{
    jwtSecretKey = builder.Configuration["JWT-SECRET-KEY"];
}

if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("JWT secret key is not configured.");
}

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 3001;
    });
}

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Allowed origins are not configured.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });


//Set up Serilog
builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        build =>
        {
            build.WithOrigins(allowedOrigins)
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials();
        });
});

builder.Services.AddSingleton<ResourceManager>(new ResourceManager("LoginErrors.ResourceFileName", typeof(Program).Assembly));

builder.Services.AddDbContext<JobProfileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IJobProfileContext, JobProfileContext>();

builder.Services.AddScoped<LoginServices>();
builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<Encryption>();
builder.Services.AddScoped<LoginBusiness>();


builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Login API", Version = "1.0.0" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Login API v1");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Login API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
