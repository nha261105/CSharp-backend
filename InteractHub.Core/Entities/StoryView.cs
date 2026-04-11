namespace InteractHub.Core.Entities;

public class StoryView
{
    public long ViewId { get; set; }
    public long StoryId { get; set; }
    public long ViewerId { get; set; }
    public int? ViewDuration { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Story Story { get; set; } = null!;
    public virtual User Viewer { get; set; } = null!;
}
