using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Auth;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}