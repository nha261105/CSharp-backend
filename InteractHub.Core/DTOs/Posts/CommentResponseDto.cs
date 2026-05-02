namespace InteractHub.Core.DTOs.Posts;

public class CommentResponseDto
{
    public long CommentId { get; set; }
    public long PostId { get; set; }
    public long UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ContentFormat { get; set; }
    public string? ImageUrl { get; set; }
    public long? ParentCommentId { get; set; }
    public int LikeCount { get; set; }
    public string? MyReactionType { get; set; }
    public int ReplyCount { get; set; }
    public bool IsEdited { get; set; }
    public DateTime RegDatetime { get; set; }
    
    public List<CommentMentionResponseDto> Mentions { get; set; } = new();
}