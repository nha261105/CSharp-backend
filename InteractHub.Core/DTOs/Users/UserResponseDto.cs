namespace InteractHub.Core.DTOs.Users;

public class UserResponseDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? AvatarUrl { get; set; }
    public string? CoverPhotoUrl { get; set; }
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public bool IsPrivateAccount { get; set; }
    public DateTime? LastLoginDateTime { get; set; }
    public DateTime RegDateTime { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public int PostCount { get; set; }
    public int FriendCount { get; set; }
    public RelationState Relation { get; set; } = RelationState.NONE;
}
