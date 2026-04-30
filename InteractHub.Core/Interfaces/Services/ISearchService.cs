using InteractHub.Core.DTOs.Search;

namespace InteractHub.Core.Interfaces.Services;

public interface ISearchService
{
    /// <summary>
    /// Tìm kiếm tổng hợp: users (theo username/fullname/email)
    /// và posts công khai (theo content).
    /// Chạy 2 query song song để tối ưu thời gian phản hồi.
    /// </summary>
    Task<GlobalSearchResponseDto> GlobalSearchAsync(
        long? currentUserId,
        string keyword,
        int page,
        int pageSize);
}
