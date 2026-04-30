namespace InteractHub.Core.DTOs.Posts;

public class SharePostRequestDto
{
    /// <summary>Caption tùy chọn khi chia sẻ bài viết.</summary>
    public string? Content { get; set; }

    public string Visibility { get; set; } = "Public";
}
