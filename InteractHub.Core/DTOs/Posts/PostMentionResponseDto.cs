namespace InteractHub.Core.DTOs.Posts;

public class PostMentionResponseDto
{
    public long MentionedUserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public int? StartPos { get; set; }
    public int? EndPos { get; set; }
}
