namespace InteractHub.Core.Entities;

public class CommentMention
{
    public long MentionId { get; set; }
    public long CommentId { get; set; }
    public long MentionedUserId { get; set; }
    public int? StartPos { get; set; }
    public int? EndPos { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Comment Comment { get; set; } = null!;
    public virtual User MentionedUser { get; set; } = null!;
}
