namespace InteractHub.Core.Entities;

public class Friendship
{
    public long FriendshipId { get; set; }
    public long RequesterId { get; set; }
    public long AddresseeId { get; set; }
    public string Status { get; set; } = "Pending";
    public long? ActionUserId { get; set; }
    public bool IsBlocked { get; set; } = false;
    public long? BlockedById { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual User Requester { get; set; } = null!;
    public virtual User Addressee { get; set; } = null!;
    public virtual User? ActionUser { get; set; }
    public virtual User? BlockedBy { get; set; }
}
