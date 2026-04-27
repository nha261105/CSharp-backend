using System.ComponentModel.DataAnnotations;

namespace InteractHub.Core.DTOs.Uploads;

public class UploadStoryMediaRequestDto
{
    [Required]
    public long StoryId { get; set; }
}
