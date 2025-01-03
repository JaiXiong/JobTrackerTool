using JobData.Entities;
using JobTracker.API.Tool.DbData;
using Login.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
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

    public LoginServicesTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(config => config["Jwt:Key"]).Returns("your-very-secure-key-1234567890");
        _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("Test.com");
        _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("TestAudience");
        _mockConfiguration.Setup(config => config["Jwt:ExpiresInMinutes"]).Returns("15");

        _mockDbContext = new Mock<JobProfileContext>(new DbContextOptions<JobProfileContext>());
        _mockResourceManager = new Mock<ResourceManager>("LoginErrors.ResourceFileName", typeof(LoginServices).Assembly);
        _mockEncryption = new Mock<Encryption>();

        _loginServices = new LoginServices(_mockResourceManager.Object, _mockDbContext.Object, _mockConfiguration.Object, _mockEncryption.Object);
    }

    [Fact]
    public async Task LoginAuth_ShouldReturnUserId_WhenUserExists()
    {
        // Arrange
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
        mockDbSet.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<UserProfile, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userProfile);

        _mockDbContext.Setup(db => db.UserProfiles).Returns(mockDbSet.Object);

        _mockEncryption.Setup(e => e.VerifyPassword(password, hashedPassword)).Returns(true);


        // Act
        var result = await _loginServices.LoginAuth(username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userProfile.Id.ToString(), result);
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken_WhenCalled()
    {
        // Arrange
        var username = "testuser";
        var password = "testpassword";

        // Act
        var token = _loginServices.GenerateToken(username, password);

        // Assert
        Assert.NotNull(token);
    }

    [Fact]
    public async Task Register_ShouldAddUser_WhenCalled()
    {
        // Arrange
        var email = "test@example.com";
        var password = "testpassword";
        var hashedPassword = "hashedpassword";

        var userProfile = new UserProfile
        {
            Id = Guid.NewGuid(),
            Name = email,
            Password = hashedPassword,
            Email = email
        };

        _mockEncryption.Setup(e => e.HashPassword(password)).Returns(hashedPassword);

        _mockDbContext.Setup(db => db.UserProfiles.AddAsync(It.IsAny<UserProfile>(), default))
            .ReturnsAsync(new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<UserProfile>(null));

        _mockDbContext.Setup(db => db.SaveChangesAsync(default))
            .ReturnsAsync(1);

        // Act
        await _loginServices.Register(email, password);

        // Assert
        _mockDbContext.Verify(db => db.UserProfiles.AddAsync(It.IsAny<UserProfile>(), default), Times.Once);
        _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
    }
}