namespace InteractHub.Core.Entities;

public class MusicTrack
{
    public long MusicId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Artist { get; set; }
    public string AudioUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int? DurationSec { get; set; }
    public bool IsLicensed { get; set; } = true;
    public string Source { get; set; } = "Internal";

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
}