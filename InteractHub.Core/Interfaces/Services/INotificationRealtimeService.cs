using InteractHub.Core.DTOs.Notifications;

namespace InteractHub.Core.Interfaces.Services;

public interface INotificationRealtimeService
{
    Task PushNotificationCreatedAsync(long recipientId, NotificationResponseDto notification);
}