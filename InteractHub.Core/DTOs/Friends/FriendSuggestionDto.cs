namespace InteractHub.Core.DTOs.Friends;

public class FriendSuggestionDto
{
    public long UserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public int MutualFriendsCount { get; set; }
}
