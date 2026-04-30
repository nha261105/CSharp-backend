namespace  InteractHub.Core.DTOs.Posts;

public class PostResponseDto
{
    // Thông tin Người đăng bài
    public long PostId {get; set;}
    public long UserId {get; set;}
    public string UserName {get; set;}= string.Empty;
    public string FullName {get; set;}= string.Empty;
    public string AvatarUrl {get; set;}= string.Empty;

    // Nội dung
    public string Content {get; set;} = string.Empty;
    public string PostType {get; set;} = "text";
    public string Visibility {get; set;} = "Public";

    // tương tác
    public int LikeCount {get; set;} = 0;
    public int CommentCount {get; set;}  = 0;
    public int ShareCount {get; set;} = 0;

    // User hiện tại
    public bool IsLikeByMe {get; set;} = true;
    public bool IsPinned {get; set;}
    public bool AllowComment {get; set;}

    // địa điểm & thời gian
    public string? LocationName {get; set;}
    public DateTime RegDateTime {get; set;} = DateTime.UtcNow;

    // Media & Music
    public List<PostMediaResponseDto> Medias {get; set;} = new();
    public long? BackgroundMusicId {get; set;}

    // Nếu là share lại từ bài khác => OriginalPost
    public PostResponseDto? OriginalPost {get; set;}

    // Mentions (@tag)
    public List<PostMentionResponseDto> Mentions { get; set; } = new();
}