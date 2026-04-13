namespace InteractHub.Core.DTOs.Friends
{
    public class FriendshipResponseDto
    {
        public long FriendshipId { get; set; }
        public string Status { get; set; } = string.Empty;
        public long ActionUserId { get; set; }
        public decimal IsBlocked { get; set; } 
    }
}