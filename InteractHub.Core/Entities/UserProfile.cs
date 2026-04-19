namespace InteractHub.Core.Entities;

public class UserProfile
{
    // Primary Key: do EF tự suy ra (tên class + "Id" = PK)
    public long ProfileId { get; set; }

    // FK đến User (1:1)
    public long UserId { get; set; }
    public int FollowerCount { get; set; } = 0;
    public int FollowingCount { get; set; } = 0;
    public int PostCount { get; set; } = 0;
    public int FriendCount { get; set; } = 0;
    public string PrivacyPosts { get; set; } = "Public";
    public string PrivacyFriends { get; set; } = "Public";
    public string PrivacyPhotos { get; set; } = "Public";
    public bool NotificationEmailFlg { get; set; } = true;
    public bool NotificationPushFlg { get; set; } = true;

    public string? RelationshipStatus { get; set; }
    public string? WorkPlace { get; set; }
    public string? Position { get; set; }
    public string? Education { get; set; }
    public string? Hometown { get; set; }
    public string? CurrentCity { get; set; }
    public string? FacebookLink { get; set; }
    public string? InstagramLink { get; set; }
    public string? TwitterLink { get; set; }

    // Audit
    public bool Delflg { get; set; } = false;
    public DateTime RegDatetime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdDatetime { get; set; }

    // Navigation properties
    public virtual User User { get; set; } = null!;
}