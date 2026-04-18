namespace InteractHub.Core.DTOs.PostReports
{
    public class PostReportsByPostDetailResponseDto
    {
        public long ReportId { get; set; }
        public long PostId { get; set; }
        public long ReporterId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = "Pending";
        public long? ReviewedById { get; set; }
        public string? ReviewNote { get; set; }
        public string? ActionTaken { get; set; }
        public bool Delflg { get; set; }
        public DateTime RegDatetime { get; set; }
        public DateTime? UpdDatetime { get; set; }
    }
}