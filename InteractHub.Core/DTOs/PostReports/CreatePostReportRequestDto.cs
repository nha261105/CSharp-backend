namespace InteractHub.Core.DTOs.PostReports
{
    public class CreatePostReportRequestDto
    {
        public long PostId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}