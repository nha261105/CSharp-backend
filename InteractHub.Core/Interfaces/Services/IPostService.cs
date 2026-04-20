using InteractHub.Core.DTOs.Posts;

namespace InteractHub.Core.Interfaces.Services;

public interface IPostService
{
    Task<List<PostResponseDto>> GetListPostPageAsync(long? currentUserId,int page,int pageSize);

    Task<List<PostResponseDto>> GetListUserPagePostAsync(long? currentUserId,long targetUserId,int page,int pageSize);

    Task<PostResponseDto?> GetPostWithIdAsync(long? currentUserId, long postId);

    Task<PostResponseDto> CreatePostAsync(long currentUserId, CreatePostRequestDto req);

    Task<PostResponseDto?> UpdatePostAsnc(long currentUserId, long PostId,UpdatePostRequestDto req);

    Task<bool> DeletePostAsync(long currentUserId, long PostId);

    Task<int> LikePostAsync(long currentUserId, long postId);
    Task<int> UnLikePostAsync(long currentUserId,long postId);

    Task<int> SharePostAsync(long currentUserId, long postId);
}