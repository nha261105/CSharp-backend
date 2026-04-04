using InteractHub.Core.DTOs.Stories;

namespace InteractHub.Core.Interfaces.Services;

public interface IStoriesService
{
    Task<StoryResponseDto> CreateStoryAsync(long userId, CreateStoryRequestDto dto);
    Task<StoryResponseDto?> GetStoryByIdAsync(long storyId, long? currentUserId = null);
    Task<PagedStoriesResponseDto> GetUserStoriesAsync(long userId, long? currentUserId, int page, int pageSize);
    Task<PagedStoriesResponseDto> GetFriendsStoriesAsync(long userId, int page, int pageSize);
    Task<PagedStoriesResponseDto> GetFeedStoriesAsync(long userId, int page, int pageSize);
    Task<StoryResponseDto?> UpdateStoryAsync(long storyId, long userId, UpdateStoryRequestDto dto);
    Task<bool> DeleteStoryAsync(long storyId, long userId);
    Task<bool> MarkStoryAsViewedAsync(long storyId, long viewerId, int? viewDuration = null);
    Task<StoryReactionResponseDto?> AddReactionAsync(long storyId, long userId, AddStoryReactionRequestDto dto);
    Task<bool> RemoveReactionAsync(long storyId, long userId);
    Task<List<StoryViewerResponseDto>> GetStoryViewersAsync(long storyId, long userId);
    Task<List<StoryReactionResponseDto>> GetStoryReactionsAsync(long storyId, long userId);
    Task<List<StoryResponseDto>> GetHighlightsAsync(long userId);
}
