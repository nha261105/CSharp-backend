using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Users;

public class UpdateProfileRequestDto
{
    [MaxLength(150)]
    public string? Fullname { get; set; }

    [MaxLength(20)]
    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(300)]
    [Url]
    public string? WebsiteUrl { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public bool IsPrivateAccount { get; set; }
}
