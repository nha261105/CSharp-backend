using InteractHub.Core.DTOs.Auth;
using InteractHub.Core.Entities;
using InteractHub.Core.Helpers;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings,
        AppDbContext dbContext,
        IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
        _dbContext = dbContext;
        _environment = environment;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        var email = dto.Email.Trim();
        var username = dto.Username.Trim();
        var fullname = dto.Fullname.Trim();
        var phoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
            ? null
            : dto.PhoneNumber.Trim();

        if (await _userManager.FindByEmailAsync(email) != null)
        {
            return BadRequest(new { message = "Email đã được sử dụng" });
        }
        if (await _userManager.FindByNameAsync(username) != null)
        {
            return BadRequest(new { message = "User đã tồn tại" });
        }

        var user = new User
        {
            UserName = username,
            Email = email,
            Fullname = fullname,
            PhoneNumber = phoneNumber,
            IsActive = true,
        };

        var res = await _userManager.CreateAsync(user, dto.Password);
        if (!res.Succeeded)
        {
            return BadRequest(new { errors = res.Errors.Select(e => e.Description) });
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!addRoleResult.Succeeded)
        {
            // Roll back created account to avoid an active user without required default role.
            await _userManager.DeleteAsync(user);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = addRoleResult.Errors.Select(e => e.Description) });
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            Fullname = user.Fullname,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var email = dto.Email.Trim();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Unauthorized(new { message = "user không tồn tại" });
        }

        if (!user.IsActive)
        {
            return Unauthorized(new { message = "Tài khoản đã bị khóa" });
        }

        var res = await _signInManager.CheckPasswordSignInAsync(
            user, dto.Password, lockoutOnFailure: true
        );

        if (res.IsLockedOut)
        {
            return StatusCode(423, new { message = "Tài khoản tạm thời bị khóa" });
        }

        if (!res.Succeeded)
        {
            return Unauthorized(new { message = "Email hoặc mật khẩu ko hợp lệ" });
        }

        await _dbContext.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.LastLoginDateTime, DateTime.UtcNow));

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            Fullname = user.Fullname,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userId, out var parsedUserId))
        {
            return Unauthorized(new { message = "Phiên đăng nhập không hợp lệ" });
        }

        var user = await _userManager.FindByIdAsync(parsedUserId.ToString());
        if (user == null)
        {
            return Unauthorized(new { message = "Người dùng không tồn tại" });
        }

        var updateStampResult = await _userManager.UpdateSecurityStampAsync(user);
        if (!updateStampResult.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { errors = updateStampResult.Errors.Select(e => e.Description) });
        }

        await _signInManager.SignOutAsync();

        return Ok(new { message = "Đăng xuất thành công" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        var email = dto.Email.Trim();
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || !user.IsActive)
        {
            return Ok(new { message = "Nếu email tồn tại,Flow hướng dẫn đặt lại mật khẩu" });
        }

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (_environment.IsDevelopment())
        {
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
            return Ok(new
            {
                message = "create reset token",
                userId = user.Id,
                resetToken = encodedToken
            });
        }

        return Ok(new { message = "Nếu email tồn tại,Flow  hướng dẫn đặt lại mật khẩu" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
        if (user == null || !user.IsActive)
        {
            return BadRequest(new { message = "Yêu cầu đặt lại mật khẩu không hợp lệ" });
        }

        var rawToken = dto.Token.Trim();
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return BadRequest(new { message = "Token không hợp lệ" });
        }

        // Support both raw Identity token and Base64Url-encoded token returned by forgot-password (dev mode).
        string decodedToken;
        try
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(rawToken);
            decodedToken = Encoding.UTF8.GetString(tokenBytes);
        }
        catch (FormatException)
        {
            decodedToken = rawToken;
        }

        var resetResult = await _userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);
        if (!resetResult.Succeeded)
        {
            return BadRequest(new { errors = resetResult.Errors.Select(e => e.Description) });
        }

        await _userManager.UpdateSecurityStampAsync(user);

        return Ok(new { message = "Đặt lại mật khẩu thành công" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!long.TryParse(userId, out var parsedUserId))
        {
            return Unauthorized(new { message = "Phiên đăng nhập không hợp lệ" });
        }

        var user = await _userManager.FindByIdAsync(parsedUserId.ToString());
        if (user == null || !user.IsActive)
        {
            return Unauthorized(new { message = "Người dùng không tồn tại hoặc đã bị khóa" });
        }

        var changeResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!changeResult.Succeeded)
        {
            return BadRequest(new { errors = changeResult.Errors.Select(e => e.Description) });
        }

        await _userManager.UpdateSecurityStampAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user, roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            Fullname = user.Fullname,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        });
    }
}