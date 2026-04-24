namespace InteractHub.Core.DTOs.Notifications;
public class CreateNotificationRequestDto
{
    public long RecipientId {get; set;}
    public string NotificationType {get; set;} = string.Empty;
    public long? SenderId {get; set;}
    public long? ReferenceId {get; set;}
    public string? ReferenceType {get; set;}

    public string? Message {get; set;}
    public string? RedirectUrl {get; set;}
}