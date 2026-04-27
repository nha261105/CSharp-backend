namespace InteractHub.Core.DTOs.Uploads;

public class FileUploadResponseDto
{
    public string FileCategory { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string MediaType { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public long? UserId { get; set; }
    public long? PostId { get; set; }
    public long? StoryId { get; set; }
}
