namespace InteractHub.Core.DTOs.PostReports
{
    public class PostReportGroupSummaryResponseDto
    {
        public long PostId { get; set; }
        public int TotalReports { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime LatestReportDatetime { get; set; }
        public long? ReviewedById { get; set; }
    }
}