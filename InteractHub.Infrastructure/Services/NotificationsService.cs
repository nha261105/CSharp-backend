using InteractHub.Core.DTOs.Notifications;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services;

public class NotificationsService : INotificationsService
{

    private readonly AppDbContext _context;

    public NotificationsService(AppDbContext context)
    {
        _context = context;
    }

    private static NotificationResponseDto MapToDto(Notification notification)
    {
        return new NotificationResponseDto
        {
            NotificationId = notification.NotificationId,
            RecipientId = notification.RecipientId,
            NotificationType = notification.NotificationType,
            IsRead = notification.IsRead,
            SenderId = notification.SenderId,
            SenderUserName = notification.Sender?.UserName,
            SenderFullname = notification.Sender?.Fullname,
            SenderAvatarUrl = notification.Sender?.AvatarUrl,
            ReferenceId = notification.ReferenceId,
            ReferenceType = notification.ReferenceType,
            Message = notification.Message,
            ReadDatetime = notification.ReadDatetime,
            RedirectUrl = notification.RedirectUrl,
            RegDatetime = notification.RegDatetime
        };
    }

    public async Task<List<NotificationResponseDto>> GetMyNotificationsAsync(
        long currentUserId,
        int page = 1,
        int pageSize = 20,
        bool unreadOnly = false
    )
    {
        if (page < 1)
        {
            page = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 20;
        }

        if (pageSize > 50)
        {
            pageSize = 50;
        }

        var query = _context.Notifications
            .AsNoTracking()
            .Include(n => n.Sender)
            .Where(n => n.RecipientId == currentUserId && !n.Delflg);

        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }

        var notifications = await query
            .OrderByDescending(n => n.RegDatetime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return notifications.Select(MapToDto).ToList();
    }

    public async Task<int> GetUnreadCountAsync(long currentUserId)
    {
        return await _context.Notifications
            .AsNoTracking()
            .CountAsync(n => n.RecipientId == currentUserId && !n.Delflg && !n.IsRead);
    }

    public async Task<NotificationResponseDto?> MarkAsReadAsync(long currentUserId, long notificationId)
    {
        var notification = await _context.Notifications
            .Include(n => n.Sender)
            .FirstOrDefaultAsync(n =>
                n.NotificationId == notificationId
                && n.RecipientId == currentUserId
                && !n.Delflg);

        if (notification == null)
        {
            return null;
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadDatetime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        return MapToDto(notification);
    }

    public async Task<int> MarkAllAsReadAsync(long currentUserId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.RecipientId == currentUserId && !n.Delflg && !n.IsRead)
            .ToListAsync();

        if (notifications.Count == 0)
        {
            return 0;
        }

        var now = DateTime.UtcNow;
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadDatetime = now;
        }

        await _context.SaveChangesAsync();
        return notifications.Count;
    }

    public async Task<NotificationResponseDto> CreateNotificationAsync(
        long recipientId,
        string notificationType,
        long? senderId = null,
        long? referenceId = null,
        string? referenceType = null,
        string? message = null,
        string? redirectUrl = null
    )
    {
        var recipientExists = await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == recipientId && !u.Delflg);

        if (!recipientExists)
        {
            throw new InvalidOperationException($"Recipient {recipientId} does not exist.");
        }

        if (senderId.HasValue)
        {
            var senderExists = await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == senderId.Value && !u.Delflg);

            if (!senderExists)
            {
                throw new InvalidOperationException($"Sender {senderId.Value} does not exist.");
            }
        }

        var notification = new Notification
        {
            RecipientId = recipientId,
            NotificationType = notificationType,
            SenderId = senderId,
            ReferenceId = referenceId,
            ReferenceType = referenceType,
            Message = message,
            RedirectUrl = redirectUrl,
            IsRead = false,
            ReadDatetime = null,
            Delflg = false,
            RegDatetime = DateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        var createdNotification = await _context.Notifications
            .AsNoTracking()
            .Include(n => n.Sender)
            .FirstAsync(n => n.NotificationId == notification.NotificationId);

        return MapToDto(createdNotification);
    }
}