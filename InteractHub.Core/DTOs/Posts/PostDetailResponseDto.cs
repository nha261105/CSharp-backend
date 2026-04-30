namespace InteractHub.Core.DTOs.Posts;

///
/// Response đầy đủ cho GET /api/posts/{id}.
/// Khác với PostResponseDto (dùng cho list view):
///   - Kèm PostReactions: danh sách người đã react bài viết, grouped by ReactionType
///   - Kèm Comments: trang 1 của comments, mỗi comment có kèm Reactions riêng
///   - Kèm CommentsHasMore: cho client biết còn trang tiếp hay không
/// 
public class PostDetailResponseDto
{
    // ── Thông tin người đăng ──────────────────────────────────────
    public long PostId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;

    // ── Nội dung ──────────────────────────────────────────────────
    public string Content { get; set; } = string.Empty;
    public string PostType { get; set; } = "Text";
    public string Visibility { get; set; } = "Public";

    // ── Số liệu tương tác ────────────────────────────────────────
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ShareCount { get; set; }

    // ── Trạng thái với người dùng hiện tại ───────────────────────
    public bool IsLikeByMe { get; set; }
    public bool IsPinned { get; set; }
    public bool AllowComment { get; set; }

    // ── Địa điểm & thời gian ─────────────────────────────────────
    public string? LocationName { get; set; }
    public DateTime RegDateTime { get; set; }

    // ── Media & Music ─────────────────────────────────────────────
    public List<PostMediaResponseDto> Medias { get; set; } = new();
    public long? BackgroundMusicId { get; set; }

    // ── Bài gốc (khi là shared post) ─────────────────────────────
    public PostResponseDto? OriginalPost { get; set; }

    // ── Reactions bài viết (grouped by type, top 5 users/type) ───
    public List<PostReactionDetailResponseDto> PostReactions { get; set; } = new();

    // ── Comments trang 1 (mỗi comment kèm reactions) ─────────────
    public List<CommentWithReactionsDto> Comments { get; set; } = new();
    public int CommentsPage { get; set; } = 1;

    /// 
    /// true nếu tổng số comment > pageSize (10).
    /// Client dùng để hiển thị nút "Xem thêm bình luận".
    ///
    public bool CommentsHasMore { get; set; }
}
