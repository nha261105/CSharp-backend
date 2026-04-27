using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Uploads;

public class UploadPostMediaRequestDto
{
    [Required]
    public long PostId { get; set; }

    public int SortOrder { get; set; } = 0;
}
