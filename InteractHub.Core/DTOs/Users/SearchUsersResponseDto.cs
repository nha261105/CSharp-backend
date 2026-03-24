namespace InteractHub.Core.DTOs.Users;

public class SearchUsersResponseDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Fullname { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public int FollowerCount { get; set; }
    public bool IsPrivateAccount { get; set; }
}
