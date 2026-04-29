namespace InteractHub.Core.DTOs.Posts;
public class CommentMentionResponseDto
{
    public long MentionedUserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int StartPos { get; set; }
    public int EndPos { get; set; }
}