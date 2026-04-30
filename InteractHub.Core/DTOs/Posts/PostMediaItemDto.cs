using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Posts;

/// <summary>
/// 1 item media trong request tạo bài viết.
/// Client upload trước (POST /api/uploads/post-media) → nhận URL → gửi cùng CreatePost.
/// </summary>
public class PostMediaItemDto
{
    [Required]
    public string MediaUrl { get; set; } = string.Empty;

    /// <summary>Image | Video | Audio</summary>
    [Required]
    public string MediaType { get; set; } = "Image";

    /// <summary>Thứ tự hiển thị (0-based).</summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>URL thumbnail (bắt buộc nếu MediaType = Video).</summary>
    public string? ThumbnailUrl { get; set; }
}
