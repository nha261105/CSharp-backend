using System.Security.Claims;
using InteractHub.Core.DTOs.Uploads;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/uploads")]
[Authorize]
public class UploadsController : ControllerBase
{
    private readonly IFileUploadService _fileUploadService;

    public UploadsController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub");

        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }

        return userId;
    }

    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadAvatar(
        [FromForm] UploadAvatarRequestDto dto,
        IFormFile file)
    {
        return await ExecuteUploadAsync(async () =>
        {
            var currentUserId = GetCurrentUserId();
            await using var stream = file.OpenReadStream();
            return await _fileUploadService.UploadAvatarAsync(
                currentUserId,
                dto,
                stream,
                file.FileName,
                file.ContentType,
                file.Length);
        }, file);
    }

    [HttpPost("cover")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCoverPhoto(
        [FromForm] UploadCoverPhotoRequestDto dto,
        IFormFile file)
    {
        return await ExecuteUploadAsync(async () =>
        {
            var currentUserId = GetCurrentUserId();
            await using var stream = file.OpenReadStream();
            return await _fileUploadService.UploadCoverPhotoAsync(
                currentUserId,
                dto,
                stream,
                file.FileName,
                file.ContentType,
                file.Length);
        }, file);
    }

    [HttpPost("post-media")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadPostMedia(
        [FromForm] UploadPostMediaRequestDto dto,
        IFormFile file)
    {
        return await ExecuteUploadAsync(async () =>
        {
            var currentUserId = GetCurrentUserId();
            await using var stream = file.OpenReadStream();
            return await _fileUploadService.UploadPostMediaAsync(
                currentUserId,
                dto,
                stream,
                file.FileName,
                file.ContentType,
                file.Length);
        }, file);
    }

    [HttpPost("story-media")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadStoryMedia(
        [FromForm] UploadStoryMediaRequestDto dto,
        IFormFile file)
    {
        return await ExecuteUploadAsync(async () =>
        {
            var currentUserId = GetCurrentUserId();
            await using var stream = file.OpenReadStream();
            return await _fileUploadService.UploadStoryMediaAsync(
                currentUserId,
                dto,
                stream,
                file.FileName,
                file.ContentType,
                file.Length);
        }, file);
    }

    private static bool IsInvalidFile(IFormFile? file)
    {
        return file == null || file.Length <= 0;
    }

    private async Task<IActionResult> ExecuteUploadAsync(
        Func<Task<FileUploadResponseDto>> uploadFunc,
        IFormFile? file)
    {
        if (IsInvalidFile(file))
        {
            return BadRequest(new { message = "File upload không hợp lệ." });
        }

        try
        {
            var response = await uploadFunc();
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Bạn không có quyền thực hiện thao tác này." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
