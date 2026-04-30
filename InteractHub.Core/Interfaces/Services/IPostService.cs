using InteractHub.Core.DTOs.Posts;

namespace InteractHub.Core.Interfaces.Services;

public interface IPostService
{
    Task<List<PostResponseDto>> GetListPostPageAsync(long? currentUserId, int page, int pageSize);

    Task<List<PostResponseDto>> GetListUserPagePostAsync(long? currentUserId, long targetUserId, int page, int pageSize);

    /// <summary>
    /// Trả về thông tin đầy đủ 1 bài viết, kèm comments trang 1 và reactions.
    /// Dùng cho màn hình chi tiết bài viết.
    /// </summary>
    Task<PostDetailResponseDto?> GetPostDetailAsync(long? currentUserId, long postId);

    /// <summary>
    /// Trả về thông tin cơ bản của 1 bài viết (không kèm comments/reactions).
    /// Dùng nội bộ sau khi create/update.
    /// </summary>
    Task<PostResponseDto?> GetPostWithIdAsync(long? currentUserId, long postId);

    Task<PostResponseDto> CreatePostAsync(long currentUserId, CreatePostRequestDto req);

    Task<PostResponseDto?> UpdatePostAsnc(long currentUserId, long postId, UpdatePostRequestDto req);

    Task<CommentResponseDto?> AddCommentAsync(long currentUserId, long postId, CreateCommentRequestDto req);

    /// <summary>
    /// Sửa nội dung comment. Chỉ chủ comment mới được sửa.
    /// Trả null nếu không tìm thấy. Throw UnauthorizedAccessException nếu không phải chủ.
    /// </summary>
    Task<CommentResponseDto?> UpdateCommentAsync(long currentUserId, long postId, long commentId, UpdateCommentRequestDto req);

    /// <summary>
    /// Xóa mềm comment (Delflg=true). Chỉ chủ comment mới được xóa.
    /// Trả false nếu không tìm thấy. Throw UnauthorizedAccessException nếu không phải chủ.
    /// </summary>
    Task<bool> DeleteCommentAsync(long currentUserId, long postId, long commentId);

    /// <summary>
    /// Toggle reaction cho comment. Trả -1 nếu comment không tồn tại hoặc không thuộc postId.
    /// </summary>
    Task<int> ToggleCommentReactionAsync(long currentUserId, long postId, long commentId, string reactionType);

    Task<List<CommentReactionDetailResponseDto>> GetCommentReactionsDetailAsync(long commentId);
    Task<List<CommentResponseDto>> GetPostCommentsAsync(long postId, int page, int pageSize);
    Task<List<CommentResponseDto>> GetCommentRepliesAsync(long postId, long commentId, int page, int pageSize);
    Task<List<PostReactionDetailResponseDto>> GetPostReactionsDetailAsync(long postId, int page, int pageSize);
    Task<bool> DeletePostAsync(long currentUserId, long postId);

    /// <summary>
    /// Toggle reaction bài viết (Like/Love/Haha/Wow/Sad/Angry).
    /// - Chưa react → thêm mới, LikeCount++
    /// - React cùng type → toggle off, LikeCount--
    /// - React khác type → đổi type, LikeCount không đổi
    /// Trả -1 nếu post không tồn tại.
    /// </summary>
    Task<(int likeCount, string? reactionType)> TogglePostReactionAsync(long currentUserId, long postId, string reactionType);

    /// <summary>
    /// Tạo bài Post mới (PostType=Shared, OriginalPostId=postId),
    /// ghi PostShare tracking, tăng ShareCount bài gốc.
    /// Trả null nếu bài gốc không tồn tại.
    /// </summary>
    Task<PostResponseDto?> SharePostAsync(long currentUserId, long postId, SharePostRequestDto req);
}