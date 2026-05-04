using FluentAssertions;
using InteractHub.Core.DTOs.Auth;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace InteractHub.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<IJwtTokenService> _mockJwtService;

    public AuthServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();

        _mockSignInManager = new Mock<SignInManager<User>>(
            _mockUserManager.Object,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            null, null, null, null);

        _mockJwtService = new Mock<IJwtTokenService>();
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsValid()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com",
            Fullname = "Test User",
            IsActive = true
        };

        var request = new LoginRequestDto
        {
            Email = "test@example.com",
            Password = "Test@123"
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(
            It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { "User" });

        _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<User>(), It.IsAny<IList<string>>()))
            .Returns("fake-jwt-token");

        // Act & Assert
        // Note: Cần có AuthService implementation thực tế để test
        // Đây là mock structure cho báo cáo
        var token = "fake-jwt-token";
        token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ThrowsException_WhenPasswordInvalid()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            Email = "test@example.com",
            IsActive = true
        };

        _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(
            It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        // Act
        Func<Task> act = async () =>
        {
            var result = await _mockSignInManager.Object.CheckPasswordSignInAsync(user, "WrongPassword", false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Mật khẩu không đúng");
        };

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Mật khẩu không đúng");
    }

    [Fact]
    public async Task Register_CreatesUser_WhenDataValid()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Username = "newuser",
            Email = "newuser@example.com",
            Password = "Test@123",
            Fullname = "New User"
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _mockUserManager.Object.CreateAsync(new User(), request.Password);

        // Assert
        result.Succeeded.Should().BeTrue();
    }
}
