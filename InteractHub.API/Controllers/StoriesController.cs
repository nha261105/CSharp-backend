using System.Security.Claims;
using InteractHub.Core.DTOs.Stories;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/stories")]
[Authorize]
public class StoriesController : ControllerBase
{
    private readonly IStoriesService _storiesService;

    public StoriesController(IStoriesService storiesService)
    {
        _storiesService = storiesService;
    }

    private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub");
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid user token");
        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStory([FromBody] CreateStoryRequestDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var story = await _storiesService.CreateStoryAsync(userId, dto);
            return CreatedAtAction(nameof(GetStoryById), new { id = story.StoryId }, story);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoryById(long id)
    {
        try
        {
            long? userId = null;
            if (User.Identity?.IsAuthenticated == true)
                userId = GetCurrentUserId();

            var story = await _storiesService.GetStoryByIdAsync(id, userId);
            if (story == null)
                return NotFound(new { message = "Story không tồn tại" });

            return Ok(story);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserStories(long userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 50) pageSize = 50;

        long? currentUserId = null;
        try { currentUserId = GetCurrentUserId(); } catch { }

        var result = await _storiesService.GetUserStoriesAsync(userId, currentUserId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("friends")]
    public async Task<IActionResult> GetFriendsStories([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 50) pageSize = 50;

            var userId = GetCurrentUserId();
            var result = await _storiesService.GetFriendsStoriesAsync(userId, page, pageSize);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("feed")]
    public async Task<IActionResult> GetFeedStories([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 50) pageSize = 50;

            var userId = GetCurrentUserId();
            var result = await _storiesService.GetFeedStoriesAsync(userId, page, pageSize);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("highlights")]
    public async Task<IActionResult> GetHighlights()
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _storiesService.GetHighlightsAsync(userId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStory(long id, [FromBody] UpdateStoryRequestDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var story = await _storiesService.UpdateStoryAsync(id, userId, dto);
            if (story == null)
                return NotFound(new { message = "Story không tồn tại hoặc bạn không có quyền sửa" });

            return Ok(story);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStory(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _storiesService.DeleteStoryAsync(id, userId);
            if (!result)
                return NotFound(new { message = "Story không tồn tại hoặc bạn không có quyền xóa" });

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPost("{id}/view")]
    public async Task<IActionResult> MarkAsViewed(long id, [FromQuery] int? viewDuration = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _storiesService.MarkStoryAsViewedAsync(id, userId, viewDuration);
            if (!result)
                return NotFound(new { message = "Story không tồn tại" });

            return Ok(new { message = "Đã xem story" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPost("{id}/reaction")]
    public async Task<IActionResult> AddReaction(long id, [FromBody] AddStoryReactionRequestDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var reaction = await _storiesService.AddReactionAsync(id, userId, dto);
            if (reaction == null)
                return NotFound(new { message = "Story không tồn tại" });

            return Ok(reaction);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpDelete("{id}/reaction")]
    public async Task<IActionResult> RemoveReaction(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _storiesService.RemoveReactionAsync(id, userId);
            if (!result)
                return NotFound(new { message = "Reaction không tồn tại" });

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("{id}/viewers")]
    public async Task<IActionResult> GetStoryViewers(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var viewers = await _storiesService.GetStoryViewersAsync(id, userId);
            return Ok(viewers);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("{id}/reactions")]
    public async Task<IActionResult> GetStoryReactions(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var reactions = await _storiesService.GetStoryReactionsAsync(id, userId);
            return Ok(reactions);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }
}
