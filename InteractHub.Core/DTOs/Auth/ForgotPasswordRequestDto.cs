using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Auth;

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;
}
