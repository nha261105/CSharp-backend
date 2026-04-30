namespace InteractHub.Core.DTOs.Posts;

/// 
/// CommentResponseDto mở rộng, kèm danh sách reactions của comment đó.
/// Dùng trong PostDetailResponseDto (GET /api/posts/{id}).
///
public class CommentWithReactionsDto
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
    public int ReplyCount { get; set; }
    public bool IsEdited { get; set; }
    public DateTime RegDatetime { get; set; }
    public List<CommentMentionResponseDto> Mentions { get; set; } = new();

    /// 
    /// Reactions của comment này, grouped by ReactionType.
    /// Mỗi group chứa tối đa 5 users để giữ response gọn.
    ///
    public List<CommentReactionDetailResponseDto> Reactions { get; set; } = new();
}
