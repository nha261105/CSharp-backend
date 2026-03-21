namespace InteractHub.Core.Entities;

public class PostShare
{
    public long ShareId { get; set; }
    public long PostId { get; set; }
    public long UserId { get; set; }
    public string? ShareContent { get; set; }
    public string Visibility { get; set; } = "Public";

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
