namespace InteractHub.Core.Entities;

public class CommentLike
{
    public long LikeId { get; set; }
    public long CommentId { get; set; }
    public long UserId { get; set; }
    public string ReactionType { get; set; } = "Like";

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Comment Comment { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
