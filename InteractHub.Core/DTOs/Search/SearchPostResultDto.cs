namespace InteractHub.Core.DTOs.Search;

public class SearchPostResultDto
{
    public long PostId { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Content { get; set; }
    public string PostType { get; set; } = string.Empty;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
    public int ShareCount { get; set; }
    public DateTime RegDateTime { get; set; }
}
