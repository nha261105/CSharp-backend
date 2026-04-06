using InteractHub.Core.DTOs.Users;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    /// <summary>Search users by keyword (username, fullname, email)</summary>
    /// GET /api/users/search?keyword=xxx&page=1&pageSize=20
    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers(
        [FromQuery] string keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống" });

        var result = await _usersService.SearchUsersAsync(keyword, page, pageSize);
        return Ok(result);
    }

    /// <summary>Get user by ID</summary>
    /// GET /api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(long id)
    {
        var result = await _usersService.GetUserByIdAsync(id);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy người dùng" });

        return Ok(result);
    }

    /// <summary>Get user profile (same as GetUserById but explicit route)</summary>
    /// GET /api/users/{id}/profile
    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetUserProfile(long id)
    {
        var result = await _usersService.GetUserProfileAsync(id);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy người dùng" });

        return Ok(result);
    }

    /// <summary>Update user profile</summary>
    /// PUT /api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProfile(long id, [FromBody] UpdateProfileRequestDto dto)
    {
        var result = await _usersService.UpdateProfileAsync(id, dto);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy người dùng" });

        return Ok(result);
    }

    /// <summary>Update user avatar</summary>
    /// PUT /api/users/{id}/avatar
    [HttpPut("{id}/avatar")]
    public async Task<IActionResult> UpdateAvatar(long id, [FromBody] string avatarUrl)
    {
        var result = await _usersService.UpdateAvatarAsync(id, avatarUrl);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy người dùng" });

        return Ok(new { avatarUrl = result });
    }

    /// <summary>Update user cover photo</summary>
    /// PUT /api/users/{id}/cover
    [HttpPut("{id}/cover")]
    public async Task<IActionResult> UpdateCoverPhoto(long id, [FromBody] string coverUrl)
    {
        var result = await _usersService.UpdateCoverPhotoAsync(id, coverUrl);
        if (result == null)
            return NotFound(new { message = "Không tìm thấy người dùng" });

        return Ok(new { coverPhotoUrl = result });
    }
}
