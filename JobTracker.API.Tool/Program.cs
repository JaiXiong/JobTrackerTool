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
using JobTracker.Business.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
var jwtSecretKey = builder.Configuration["JWT_SECRET_KEY"];

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHttpsRedirection(options =>
    {
        options.HttpsPort = 5001;
    });
}

// Read allowed origins from configuration file in appsettings
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

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

// Add services
builder.Services.AddControllers();
builder.Services.AddLogging();
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

// Register the Swagger generator so we can view it
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobTracker API", Version = "v1" });
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "JobTracker API v1");
    });
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();

app.Run();
