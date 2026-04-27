using InteractHub.API.Hubs;
using InteractHub.Core.DTOs.Notifications;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace InteractHub.API.Services;

public class NotificationRealtimeService : INotificationRealtimeService
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationsService _notificationsService;

    public NotificationRealtimeService(
        IHubContext<NotificationHub> hubContext,
        INotificationsService notificationsService)
    {
        _hubContext = hubContext;
        _notificationsService = notificationsService;
    }

    public async Task PushNotificationCreatedAsync(long recipientId, NotificationResponseDto notification)
    {
        var unreadCount = await _notificationsService.GetUnreadCountAsync(recipientId);

        await _hubContext.Clients.User(recipientId.ToString())
            .SendAsync("NotificationCreated", notification);

        await _hubContext.Clients.User(recipientId.ToString())
            .SendAsync("NotificationUnreadCountUpdated", unreadCount);
    }
}