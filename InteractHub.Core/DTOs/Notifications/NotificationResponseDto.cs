namespace InteractHub.Core.DTOs.Notifications;

public class NotificationResponseDto
{
    public long NotificationId { get; set; }

    public long RecipientId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public bool IsRead { get; set; }

    public long? SenderId { get; set; }
    public string? SenderUserName { get; set; }
    public string? SenderFullname { get; set; }
    public string? SenderAvatarUrl { get; set; }

    public long? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public string? Message { get; set; }
    public DateTime? ReadDatetime { get; set; }
    public string? RedirectUrl { get; set; }

    public DateTime RegDatetime { get; set; }
}