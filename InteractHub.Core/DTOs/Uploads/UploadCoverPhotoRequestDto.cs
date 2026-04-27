using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Uploads;

public class UploadCoverPhotoRequestDto
{
    [Required]
    public long UserId { get; set; }
}
