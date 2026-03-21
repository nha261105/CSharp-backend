namespace InteractHub.Core.Entities;

public class Post
{
    public long PostId { get; set; }
    public long UserId { get; set; }
    public string? Content { get; set; }
    public string? ContentFormat { get; set; }
    public string PostType { get; set; } = "Text";
    public string Visibility { get; set; } = "Public";
    public string? LocationName { get; set; }
    public decimal? LocationLat { get; set; }
    public decimal? LocationLng { get; set; }
    public string? Feeling { get; set; }
    public long? OriginalPostId { get; set; }
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

    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual Post? OriginalPost { get; set; }
    public virtual MusicTrack? BackgroundMusic { get; set; }
    public virtual ICollection<Post> SharedPosts { get; set; } = new List<Post>();
    public virtual ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    public virtual ICollection<PostShare> PostShares { get; set; } = new List<PostShare>();
    public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    public virtual ICollection<PostMention> PostMentions { get; set; } = new List<PostMention>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<PostReport> PostReports { get; set; } = new List<PostReport>();
}