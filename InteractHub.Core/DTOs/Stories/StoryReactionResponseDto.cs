namespace InteractHub.Core.DTOs.Stories;

public class StoryReactionResponseDto
{
    public long ReactionId { get; set; }
    public long StoryId { get; set; }
    public long UserId { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? AvatarUrl { get; set; }
    public string ReactionType { get; set; } = string.Empty;
    public DateTime RegDatetime { get; set; }
}
