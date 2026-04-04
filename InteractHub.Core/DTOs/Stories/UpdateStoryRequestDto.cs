namespace InteractHub.Core.DTOs.Stories;

public class UpdateStoryRequestDto
{
    public string? MediaUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public string? CaptionFormat { get; set; }
    public string? BgColor { get; set; }
    public string? FontStyle { get; set; }
    public int? DurationSec { get; set; }
    public string? Visibility { get; set; }
    public long? BackgroundMusicId { get; set; }
    public int? MusicStartSec { get; set; }
    public int? MusicEndSec { get; set; }
    public bool? IsHighlighted { get; set; }
    public string? HighlightName { get; set; }
}
