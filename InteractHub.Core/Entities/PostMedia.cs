namespace InteractHub.Core.Entities;

public class PostMedia
{
    public long MediaId { get; set; }
    public long PostId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? FileName { get; set; }
    public int? FileSizeKb { get; set; }
    public int? WidthPx { get; set; }
    public int? HeightPx { get; set; }
    public int? DurationSeconds { get; set; }
    public int SortOrder { get; set; } = 0;
    public string ProcessingStatus { get; set; } = "Ready";

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
}