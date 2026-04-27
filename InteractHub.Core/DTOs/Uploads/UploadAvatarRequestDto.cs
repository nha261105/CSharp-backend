using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Uploads;

public class UploadAvatarRequestDto
{
    [Required]
    public long UserId { get; set; }
}
