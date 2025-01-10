using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Login.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Resources;
using Utils.Encryption;

namespace JobTracker.UnitTests;

public class LoginServicesTests
{
    private readonly LoginServices _loginServices;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IJobProfileContext> _mockDbContext;
    private readonly Mock<ResourceManager> _mockResourceManager;
    private readonly Mock<Encryption> _mockEncryption;
    private readonly Mock<LoginBusiness> _mockLoginBusiness;

    public LoginServicesTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(config => config["JWT_SECRET_KEY"]).Returns("your-very-secure-key-12345678901011");
        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("Test.com");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
        _mockConfiguration.Setup(config => config["Jwt:ExpiresInMinutes"]).Returns("15");

        _mockDbContext = new Mock<IJobProfileContext>();
        _mockResourceManager = new Mock<ResourceManager>("LoginErrors.ResourceFileName", typeof(LoginServices).Assembly);
        _mockEncryption = new Mock<Encryption>();
        _mockLoginBusiness = new Mock<LoginBusiness>();

        _loginServices = new LoginServices(_mockResourceManager.Object, _mockDbContext.Object, _mockConfiguration.Object, _mockEncryption.Object, _mockLoginBusiness.Object);
    }

    [Fact]
    public async Task LoginAuth_ShouldReturnUserId_WhenUserExists()
    {
        var username = "testuser";
        var password = "testpassword";
        var hashedPassword = "hashedpassword";
        var userId = Guid.NewGuid().ToString();

        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            Name = username,
            Password = hashedPassword,
            Email = "test@example.com"
        };

        var userProfiles = new List<UserProfile> { userProfile }.AsQueryable();
        var mockDbSet = new Mock<DbSet<UserProfile>>();
        mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Provider).Returns(userProfiles.Provider);
        mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.Expression).Returns(userProfiles.Expression);
        mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.ElementType).Returns(userProfiles.ElementType);
        mockDbSet.As<IQueryable<UserProfile>>().Setup(m => m.GetEnumerator()).Returns(userProfiles.GetEnumerator());

        _mockDbContext.Setup(db => db.UserProfiles).Returns(mockDbSet.Object);
        _mockDbContext.Setup(db => db.GetUserProfileAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userProfile);

        _mockEncryption.Setup(e => e.VerifyPassword(password, hashedPassword)).Returns(true);

        var result = await _loginServices.LoginAuth(username, password);

        Assert.NotNull(result);
        Assert.Equal(userProfile.Id.ToString(), result);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenCalled()
    {
        var username = "testuser";
        //var password = "testpassword";

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
        _mockResourceManager.Setup(rm => rm.GetString("UserExist")).Returns("User already exists");
        _mockEncryption.Setup(e => e.HashPassword(It.IsAny<string>())).Returns("hashedPassword");

        await _loginServices.Register("newuser@example.com", "password");

        _mockDbContext.Verify(db => db.UserProfiles.AddAsync(It.IsAny<UserProfile>(), default), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
}