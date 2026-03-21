namespace InteractHub.Core.Entities;

public class Story
{
    public long StoryId { get; set; }
    public long UserId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public string? CaptionFormat { get; set; }
    public string? BgColor { get; set; }
    public string? FontStyle { get; set; }
    public int DurationSec { get; set; } = 5;
    public string Visibility { get; set; } = "Friends";
    public long? BackgroundMusicId { get; set; }
    public int MusicStartSec { get; set; } = 0;
    public int? MusicEndSec { get; set; }
    public int ViewCount { get; set; } = 0;
    public int ReactionCount { get; set; } = 0;
    public DateTime ExpireDatetime { get; set; }
    public bool IsExpired { get; set; } = false;
    public bool IsHighlighted { get; set; } = false;
    public string? HighlightName { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual MusicTrack? BackgroundMusic { get; set; }
    public virtual ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();
    public virtual ICollection<StoryReaction> StoryReactions { get; set; } = new List<StoryReaction>();
}
