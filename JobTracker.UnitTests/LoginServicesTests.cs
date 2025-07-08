using Azure.Identity;
using Castle.Core.Logging;
using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Login.Business.Business;
using Login.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Resources;
using Utils.Encryption;
using Utils.Operations;

namespace JobTracker.UnitTests;

public class LoginServicesTests
{
    private readonly LoginServices _loginServices;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IJobProfileContext> _mockDbContext;
    private readonly Mock<ResourceManager> _mockResourceManager;
    private readonly Mock<Encryption> _mockEncryption;
    private readonly Mock<LoginBusiness> _mockLoginBusiness;
    private readonly Mock<ILogger<LoginServices>> _mockLogger;
    private readonly Mock<ILogger<LoginBusiness>> _mockLoggerBusiness;

    public LoginServicesTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(config => config["JWT_SECRET_KEY"]).Returns("your-very-secure-key-12345678901011");
        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("Test.com");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
        _mockConfiguration.Setup(config => config["Jwt:ExpiresInMinutes"]).Returns("15");

        _mockDbContext = new Mock<IJobProfileContext>();
        //_mockResourceManager = new Mock<ResourceManager>("LoginErrors.ResourceFileName", typeof(LoginServices).Assembly
        _mockResourceManager = new Mock<ResourceManager>("Login.Business.LoginBusinessErrors", typeof(LoginServices).Assembly);
        _mockEncryption = new Mock<Encryption>();
        _mockLogger = new Mock<ILogger<LoginServices>>();
        _mockLoggerBusiness = new Mock<ILogger<LoginBusiness>>();
        _mockLoginBusiness = new Mock<LoginBusiness>(_mockConfiguration.Object, _mockResourceManager.Object, _mockLoggerBusiness.Object);
       
        _loginServices = new LoginServices(_mockResourceManager.Object, _mockDbContext.Object, _mockConfiguration.Object,
            _mockEncryption.Object, _mockLoginBusiness.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task LoginAuth_ShouldReturnUserId_WhenUserExists()
    {
        var username = "admin";
        var password = "pw";
        var email = "admin@example.com";
        var hashedPassword = "pw";
        var userId = Guid.NewGuid().ToString();

        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            Name = username,
            Password = hashedPassword,
            Email = email
        };

        //var userProfiles = new List<UserProfile> { userProfile }.AsQueryable();
        //var mockDbSet = new Mock<DbSet<UserProfile>>();
        //mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Provider).Returns(userProfiles.Provider);
        //mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Expression).Returns(userProfiles.Expression);
        //mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.ElementType).Returns(userProfiles.ElementType);
        //mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.GetEnumerator()).Returns(userProfiles.GetEnumerator());

        //_mockDbContext.Setup(db => db.UserProfiles).Returns(mockDbSet.Object);
        //_mockDbContext.Setup(db => db.GetUserProfileAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(userProfile);

        //_mockEncryption.Setup(e => e.VerifyPassword(password, hashedPassword)).Returns(true);

        var userProfiles = new List<UserProfile> { userProfile }.AsQueryable();

        _mockDbContext.Setup(db => db.UserProfiles).ReturnsDbSet(userProfiles);
        _mockEncryption.Setup(e => e.HashPassword(It.IsAny<string>())).Returns((string pw) => hashedPassword);

        _mockEncryption.Setup(e => e.VerifyPassword(password, hashedPassword)).Returns(true);

        var result = await _loginServices.LoginAuth(userProfile.Email, userProfile.Password);

        //var result = await _loginServices.LoginAuth(userProfile.Email, userProfile.Password);

        Assert.NotNull(result);
        Assert.Equal(userProfile.Id.ToString(), result.ToString());
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenCalled()
    {
        var username = "admin";
        var password = "pw";
        //var config = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json")
        //    //.AddJsonFile("appsettings.Development.json", optional: true)
        //    //.AddEnvironmentVariables()
        //    .AddAzureKeyVault(
        //    new Uri("https://jobappvault.vault.azure.net/"),
        //    new DefaultAzureCredential())
        //    .Build();

        //var jwtSecretKey = config["JWT-SECRET-KEY"];
        var jwtSecretKey = _mockConfiguration.Setup(config => config["JWT-SECRET-KEY"]).Returns("your-very-secure-key-12345678901011");
        //_mockConfiguration.Setup(config => config["JWT-SECRET-KEY"]).Returns(jwtSecretKey);
        _mockEncryption.Setup(e => e.HashPassword(password)).Returns("hashedPassword");

        var token = _loginServices.GenerateToken(username);

        Assert.NotNull(token);
    }
    
    [Fact]
    public async Task Register_ShouldAddUser_WhenCalled()
    {
        var userProfiles = new List<UserProfile>
        {
            new UserProfile {Name = "existing", Email = "existing@example.com", Password = "pw" }
        }.AsQueryable();

        _mockDbContext.Setup(db => db.UserProfiles).ReturnsDbSet(userProfiles);
        //_mockResourceManager.Setup(rm => rm.GetString("UserExist")).Returns("User exists already.");
        _mockResourceManager.Setup(rm => rm.GetString("User registered successfully.")).Returns("User registered successfully.");

        _mockEncryption.Setup(e => e.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

        await _loginServices.Register("newuser@example.com", "password");

        _mockDbContext.Verify(db => db.UserProfiles.AddAsync(It.IsAny<UserProfile>(), default), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
}