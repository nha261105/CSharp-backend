namespace InteractHub.Core.Entities;

public class StoryReaction
{
    public long ReactionId { get; set; }
    public long StoryId { get; set; }
    public long UserId { get; set; }
    public string ReactionType { get; set; } = "Like";

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Story Story { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
