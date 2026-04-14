using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Posts;

public class CreatePostRequestDto
{
    // public long UserId {get; set;}
    [Required]
    public string PostType {get; set;} = "Text";
    [Required]
    public string Visibility {get; set;} = "Public";
    public int MusicStartSec { get; set; } = 0;
    // public int LikeCount { get; set; } = 0;
    // public int CommentCount { get; set; } = 0;
    // public int ShareCount { get; set; } = 0;
    // public bool IsEdited { get; set; } = false;
    public bool IsPinned { get; set; } = false;
    // public bool IsReported { get; set; } = false;
    // public int ReportCount { get; set; } = 0;
    public bool AllowComment { get; set; } = true;
    
    [MaxLength(50000)]
    public string? Content {get; set;}
    public string? ContentFormat {get; set;}
    public string? LocationName {get; set;}
    public decimal? LocationLat { get; set; }
    public decimal? LocationLng { get; set; }
    public string? Feeling {get; set;}
    public long? OriginalPostId {get; set;}
     public long? BackgroundMusicId { get; set; }
    public int? MusicEndSec { get; set; }


    // Audit
    // public bool Delflg { get; set; } = false;
    // public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    // public DateTime? UpdDatetime { get; set; }
}