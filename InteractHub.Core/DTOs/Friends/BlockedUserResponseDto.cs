namespace InteractHub.Core.DTOs.Friends
{
    public class BlockedUserResponseDto
    {
        public long UserId { get; set; }
        public string? Fullname { get; set; }
        public string? AvatarUrl { get; set; }
    }
}