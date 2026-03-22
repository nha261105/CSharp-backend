using InteractHub.Core.DTOs.Auth;
using InteractHub.Core.Entities;
using InteractHub.Core.Helpers;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(UserManager<User> userManager,SignInManager<User> signInManager,IJwtTokenService jwtTokenService, IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
    {
        if(await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            return BadRequest(new {message = "Email đã được sử dụng"});
        }
        if(await _userManager.FindByNameAsync(dto.Username) != null)
        {
            return BadRequest(new {message = "User đã tồn tại"});
        }

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email,
            Fullname = dto.Fullname,
            PhoneNumber = dto.PhoneNumber,
            IsActive = true,
        };

        var res = await _userManager.CreateAsync(user,dto.Password);
        if(!res.Succeeded)
        {
            return BadRequest(new {errors = res.Errors.Select(e => e.Description)});
        }

        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user,roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
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
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if(user == null)
        {
            return Unauthorized(new {message = "user không tồn tại"});
        }

        if(!user.IsActive)
        {
            return Unauthorized(new {message = "Tài khoản đã bị khóa"});
        }

        var res = await _signInManager.CheckPasswordSignInAsync(
            user, dto.Password, lockoutOnFailure: true
        );

        if(res.IsLockedOut)
        {
            return StatusCode(423, new {message = "Tài khoản tạm thời bị khóa"});
        }

        if(!res.Succeeded)
        {
            return Unauthorized(new {message = "Email hoặc mật khẩu ko hợp lệ"});
        }

        user.LastLoginDateTime = DateTime.Now;
        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenService.GenerateToken(user,roles);

        return Ok(new AuthResponseDto
        {
            Token = token,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            Fullname = user.Fullname,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        });
    }
}