using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Posts;

public class CreatePostRequestDto
{
    [Required]
    public string PostType { get; set; } = "Text";
    [Required]
    public string Visibility { get; set; } = "Public";
    public int MusicStartSec { get; set; } = 0;
    public bool IsPinned { get; set; } = false;
    public bool AllowComment { get; set; } = true;

    [MaxLength(50000)]
    public string? Content { get; set; }
    public string? ContentFormat { get; set; }
    public string? LocationName { get; set; }
    public decimal? LocationLat { get; set; }
    public decimal? LocationLng { get; set; }
    public string? Feeling { get; set; }
    public long? OriginalPostId { get; set; }
    public long? BackgroundMusicId { get; set; }
    public int? MusicEndSec { get; set; }

    /// <summary>
    /// Danh sách media đính kèm bài viết.
    /// Client phải upload trước (POST /api/uploads/post-media),
    /// lấy URL rồi gửi cùng request này.
    /// </summary>
    public List<PostMediaItemDto>? Medias { get; set; }

    /// <summary>
    /// Danh sách user được tag (@mention) trong bài viết.
    /// </summary>
    public List<CreateCommentMentionRequestDto>? Mentions { get; set; }
}