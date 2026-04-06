namespace InteractHub.Core.DTOs.Stories;

public class CreateStoryRequestDto
{
    public string MediaUrl { get; set; } = string.Empty;
    public string MediaType { get; set; } = string.Empty; // "Image" or "Video"
    public string? ThumbnailUrl { get; set; }
    public string? Caption { get; set; }
    public string? CaptionFormat { get; set; }
    public string? BgColor { get; set; }
    public string? FontStyle { get; set; }
    public int DurationSec { get; set; } = 5;
    public string Visibility { get; set; } = "Friends"; // "Friends" or "Public"
    public long? BackgroundMusicId { get; set; }
    public int MusicStartSec { get; set; } = 0;
    public int? MusicEndSec { get; set; }
}
