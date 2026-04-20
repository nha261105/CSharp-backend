using InteractHub.Core.DTOs.Users;

namespace InteractHub.Core.Interfaces.Services;

public interface IUsersService
{
    // GET /api/users/{id}
    Task<UserResponseDto?> GetUserByIdAsync(long id);

    // GET /api/users/search?keyword=xxx
    Task<IEnumerable<SearchUsersResponseDto>> SearchUsersAsync(string keyword, int page = 1, int pageSize = 20);

    // GET /api/users/{id}/profile
    Task<UserResponseDto?> GetUserProfileAsync(long id);

    // PUT /api/users/{id}
    Task<UserResponseDto?> UpdateProfileAsync(long userId, UpdateProfileRequestDto dto);

    // PUT /api/users/{id}/avatar
    Task<string?> UpdateAvatarAsync(long userId, string avatarUrl);

    // PUT /api/users/{id}/cover
    Task<string?> UpdateCoverPhotoAsync(long userId, string coverUrl)   ;
}
