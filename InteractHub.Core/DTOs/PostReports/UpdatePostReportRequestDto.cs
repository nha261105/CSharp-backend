namespace InteractHub.Core.DTOs.PostReports
{
    public class UpdatePostReportRequestDto
    {
        public string Status { get; set; } = string.Empty; // Pending, Reviewing, Dismissed, Resolved
        public string? ReviewNote { get; set; }
        public string? ActionTaken { get; set; } // PostRemoved, UserWarned, UserBanned, NoAction
    }
}