namespace InteractHub.Core.DTOs.Posts;

public class CreateCommentMentionRequestDto
{
    public long MentionedUserId { get; set; }
    public int? StartPos { get; set; }
    public int? EndPos { get; set; }
}