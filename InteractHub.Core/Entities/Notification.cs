namespace InteractHub.Core.Entities;

public class Notification
{
    public long NotificationId { get; set; }
    public long RecipientId { get; set; }
    public long? SenderId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public long? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public string? Message { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadDatetime { get; set; }
    public string? RedirectUrl { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User Recipient { get; set; } = null!;
    public virtual User? Sender { get; set; }
}
