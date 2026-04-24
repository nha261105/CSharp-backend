namespace InteractHub.Core.DTOs.Friends
{
    public class MutualFriendResponseDto
    {
        public long UserId { get; set; }
        public string? Fullname { get; set; }
        public string? AvatarUrl { get; set; }
        public int MutualFriendsCount { get; set; } 
        public List<string?> TopMutualAvatars { get; set; } = new();
    }
}