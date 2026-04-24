using InteractHub.Core.DTOs.Notifications;

namespace InteractHub.Core.Interfaces.Services;

public interface INotificationsService
{
    Task<List<NotificationResponseDto>> GetMyNotificationsAsync(
        long currentUserId,
        int page = 1,
        int pageSize = 20,
        bool unreadOnly = false
    );

    Task<int> GetUnreadCountAsync(long currentUserId);

    Task<NotificationResponseDto?> MarkAsReadAsync(long currentUserId, long notificationId);

    Task<int> MarkAllAsReadAsync(long currentUserId);

    Task<NotificationResponseDto> CreateNotificationAsync(
        long recipientId,
        string notificationType,
        long? senderId = null,
        long? referenceId = null,
        string? referenceType = null,
        string? message = null,
        string? redirectUrl = null
    );
}