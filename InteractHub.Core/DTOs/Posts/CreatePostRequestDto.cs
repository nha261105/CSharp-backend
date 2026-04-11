namespace InteractHub.Core.DTOs.Posts;

public class CreatePostRequestDto
{
    public long UserId {get; set;}
    public string? Content {get; set;} = string.Empty;

    public string? ContentFormat {get; set;}

    public string PostType {get; set;} = "Text";
    public string Visibility {get; set;} = "Public";

    public string LocationName {get; set;} = string.Empty;

    public decimal? LocationLat { get; set; }
    public decimal? LocationLng { get; set; }

    public string Feeling {get; set;} = string.Empty;

    public long? OriginalPostId {get; set;}

     public long? BackgroundMusicId { get; set; }
    public int MusicStartSec { get; set; } = 0;
    public int? MusicEndSec { get; set; }
    public int LikeCount { get; set; } = 0;
    public int CommentCount { get; set; } = 0;
    public int ShareCount { get; set; } = 0;
    public bool IsEdited { get; set; } = false;
    public bool IsPinned { get; set; } = false;
    public bool IsReported { get; set; } = false;
    public int ReportCount { get; set; } = 0;
    public bool AllowComment { get; set; } = true;

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }
}