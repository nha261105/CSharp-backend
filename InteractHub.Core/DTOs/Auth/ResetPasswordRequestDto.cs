using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Auth;

public class ResetPasswordRequestDto
{
    [Required]
    public long UserId { get; set; }

    [Required]
    [StringLength(2048)]
    public string Token { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
}
