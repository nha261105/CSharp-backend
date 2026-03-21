namespace InteractHub.Core.Entities;

public class Hashtag
{
    public int HashtagId { get; set; }
    public string TagName { get; set; } = string.Empty;
    public int PostCount { get; set; } = 0;
    public decimal TrendingScore { get; set; } = 0;
    public bool IsTrending { get; set; } = false;

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
}
