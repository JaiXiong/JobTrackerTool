using JobTracker.API.Tool.DbData;
using JobTracker.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Resources;
using System.Text;
using Utils.Encryption;
using AutoMapper;
using Utils.AutoMapper;
using Utils.Middleware;
using JobTracker.Business.Business;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Set up Serilog
var logDirectory = builder.Configuration["Logging:LogDirectory:LogPath"] ?? "Logs";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Configuration.AddEnvironmentVariables();
var jwtSecretKey = builder.Configuration["JWT_SECRET_KEY"];

if (string.IsNullOrEmpty(jwtSecretKey))
{
    throw new InvalidOperationException("JWT secret key is not configured.");
}

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    });
}

// Read allowed origins from configuration file in appsettings
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (allowedOrigins == null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Allowed origins are not configured.");
}

// Add the CORS policy, for now we allow all, buckle down later
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

//Use Serilog
builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(DataMapper));

// Register ResourceManager so we can catch errors
builder.Services.AddSingleton<ResourceManager>(new ResourceManager("JobTackerBusinessErrors.ResourceFileName", typeof(Program).Assembly));

// Register JobTrackerToolService and Encryption
builder.Services.AddScoped<IJobTrackerToolService, JobTrackerToolService>();
builder.Services.AddScoped<IJobTrackerToolBusiness, JobTrackerToolBusiness>();
builder.Services.AddScoped<Encryption>();

// Register the DbContext with a connection string in the appsettings
builder.Services.AddDbContext<JobProfileContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobTracker API", Version = "1.0.0" });
});

var app = builder.Build();

// Register Middleware
// We're not using the custom exception handling middleware for now, as it doesn't suit our needs for specific error handling in APIs

//app.UseMiddleware<ExceptionHandlingMiddleware>();
//app.UseMiddleware<LoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobTracker API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobTracker API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
