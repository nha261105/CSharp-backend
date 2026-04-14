namespace InteractHub.Core.Entities;

public class PostMention
{
    public long MentionId { get; set; }
    public long PostId { get; set; }
    public long MentionedUserId { get; set; }
    public int? StartPos { get; set; }
    public int? EndPos { get; set; }

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual User MentionedUser { get; set; } = null!;

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
}
