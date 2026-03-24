namespace InteractHub.Core.DTOs.Stories;

public class StoryViewerResponseDto
{
    public long ViewerId { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? AvatarUrl { get; set; }
    public int? ViewDuration { get; set; }
    public DateTime RegDatetime { get; set; }
}
