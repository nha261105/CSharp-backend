using System.Security.Claims;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
	private readonly INotificationsService _notificationsService;

	public NotificationsController(INotificationsService notificationsService)
	{
		_notificationsService = notificationsService;
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

	[HttpGet]
	public async Task<IActionResult> GetMyNotifications(
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20,
		[FromQuery] bool unreadOnly = false)
	{
		try
		{
			if (page < 1) page = 1;
			if (pageSize < 1) pageSize = 20;
			if (pageSize > 50) pageSize = 50;

			var userId = GetCurrentUserId();
			var notifications = await _notificationsService.GetMyNotificationsAsync(
				userId,
				page,
				pageSize,
				unreadOnly);

			return Ok(notifications);
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized(new { message = "Token không hợp lệ" });
		}
	}

	[HttpGet("unread-count")]
	public async Task<IActionResult> GetUnreadCount()
	{
		try
		{
			var userId = GetCurrentUserId();
			var unreadCount = await _notificationsService.GetUnreadCountAsync(userId);

			return Ok(new { unreadCount });
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized(new { message = "Token không hợp lệ" });
		}
	}

	[HttpPatch("{id}/read")]
	public async Task<IActionResult> MarkAsRead(long id)
	{
		try
		{
			var userId = GetCurrentUserId();
			var notification = await _notificationsService.MarkAsReadAsync(userId, id);

			if (notification == null)
			{
				return NotFound(new { message = "Notification không tồn tại hoặc bạn không có quyền cập nhật" });
			}

			return Ok(notification);
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized(new { message = "Token không hợp lệ" });
		}
	}

	[HttpPatch("read-all")]
	public async Task<IActionResult> MarkAllAsRead()
	{
		try
		{
			var userId = GetCurrentUserId();
			var updatedCount = await _notificationsService.MarkAllAsReadAsync(userId);

			return Ok(new { updatedCount });
		}
		catch (UnauthorizedAccessException)
		{
			return Unauthorized(new { message = "Token không hợp lệ" });
		}
	}
}
