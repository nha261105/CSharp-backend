namespace InteractHub.Core.DTOs.Friends
{
    public class FriendResponseDto
    {
        public long UserId { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; } 
        public string? ActionDate { get; set; }
        public int MutualFriendsCount { get; set; }
        public List<MutualFriendResponseDto> TopMutualFriends { get; set; } = new();
    }
}