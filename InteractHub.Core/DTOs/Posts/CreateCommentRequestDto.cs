namespace InteractHub.Core.DTOs.Posts;

public class CreateCommentRequestDto
{
    public string Content { get; set; } = string.Empty;
    public string? ContentFormat { get; set; }
    public string? ImageUrl { get; set; }
    public long? ParentCommentId { get; set; }
    public List<CreateCommentMentionRequestDto>? Mentions { get; set; }
}