using InteractHub.Core.DTOs.PostReports;

namespace InteractHub.Core.Interfaces.Services
{
    public interface IPostReportService
    {
        Task CreateReportAsync(long userId, CreatePostReportRequestDto dto);
        Task<PagedResponseDto<PostReportGroupSummaryResponseDto>> GetAllGroupedReportsAsync(int page, int pageSize);
        Task<PagedResponseDto<PostReportsByPostDetailResponseDto>> GetReportsByPostIdAsync(long postId, int page, int pageSize);
        Task<PostReportsByPostDetailResponseDto?> GetReportByIdAsync(long reportId);
        Task UpdateReportStatusByPostAsync(long adminId, long postId, UpdatePostReportRequestDto dto);
        Task DeleteReportsByPostAsync(long postId);
    }
}