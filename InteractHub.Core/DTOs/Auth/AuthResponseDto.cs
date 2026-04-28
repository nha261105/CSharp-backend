namespace InteractHub.Core.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Fullname { get; set; } = string.Empty;

    public IList<string> Roles { get; set; } = [];

    public DateTime ExpiresAt { get; set; }
}