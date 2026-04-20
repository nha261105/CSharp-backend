namespace InteractHub.Core.Entities;

public class PostReport
{
    public long ReportId { get; set; }
    public long PostId { get; set; }
    public long ReporterId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public string? Description { get; set; }
    public long? ReviewedById { get; set; }
    public string? ReviewNote { get; set; }
    public string? ActionTaken { get; set; }
    public DateTime? ReviewDatetime { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual User Reporter { get; set; } = null!;
    public virtual User? ReviewedBy { get; set; }
}
