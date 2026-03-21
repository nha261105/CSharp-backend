namespace InteractHub.Core.Entities;

public class PostHashtag
{
    public long PostHashtagId { get; set; }
    public long PostId { get; set; }
    public int HashtagId { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual Hashtag Hashtag { get; set; } = null!;
}
