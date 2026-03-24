namespace InteractHub.Core.DTOs.Stories;

public class StoryResponseDto
{
    public long StoryId { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? AvatarUrl { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public string? CaptionFormat { get; set; }
    public string? BgColor { get; set; }
    public string? FontStyle { get; set; }
    public int DurationSec { get; set; }
    public string Visibility { get; set; } = string.Empty;
    public long? BackgroundMusicId { get; set; }
    public string? BackgroundMusicTitle { get; set; }
    public int MusicStartSec { get; set; }
    public int? MusicEndSec { get; set; }
    public int ViewCount { get; set; }
    public int ReactionCount { get; set; }
    public DateTime ExpireDatetime { get; set; }
    public bool IsExpired { get; set; }
    public bool IsHighlighted { get; set; }
    public string? HighlightName { get; set; }
    public DateTime RegDatetime { get; set; }
    public bool HasReacted { get; set; }
    public bool HasViewed { get; set; }
}
