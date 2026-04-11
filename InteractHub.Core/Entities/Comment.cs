namespace InteractHub.Core.Entities;

public class Comment
{
    public long CommentId { get; set; }
    public long PostId { get; set; }
    public long UserId { get; set; }
    public long? ParentCommentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ContentFormat { get; set; }
    public string? ImageUrl { get; set; }
    public int LikeCount { get; set; } = 0;
    public int ReplyCount { get; set; } = 0;
    public bool IsEdited { get; set; } = false;
    public bool IsReported { get; set; } = false;

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual Comment? ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    public virtual ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    public virtual ICollection<CommentMention> CommentMentions { get; set; } = new List<CommentMention>();
}
