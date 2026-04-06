using InteractHub.Core.DTOs.Users;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UsersService(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    private UserResponseDto MapToDto(User user, UserProfile? profile = null)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Fullname = user.Fullname,
            Gender = user.Gender,
            DateOfBirth = user.DateOfBirth,
            AvatarUrl = user.AvatarUrl,
            CoverPhotoUrl = user.CoverPhotoUrl,
            Bio = user.Bio,
            WebsiteUrl = user.WebsiteUrl,
            Location = user.Location,
            IsActive = user.IsActive,
            IsPrivateAccount = user.IsPrivateAccount,
            LastLoginDateTime = user.LastLoginDateTime,
            RegDateTime = user.RegDateTime,
            FollowerCount = profile?.FollowerCount ?? 0,
            FollowingCount = profile?.FollowingCount ?? 0,
            PostCount = profile?.PostCount ?? 0,
            FriendCount = profile?.FriendCount ?? 0,
        };
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(long id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == id);

        return MapToDto(user, profile);
    }

    public async Task<IEnumerable<SearchUsersResponseDto>> SearchUsersAsync(
        string keyword, int page = 1, int pageSize = 20)
    {
        var query = _userManager.Users
            .Where(u => !u.Delflg && u.IsActive)
            .Where(u =>
                (u.UserName != null && u.UserName.Contains(keyword)) ||
                u.Fullname.Contains(keyword) ||
                (u.Email != null && u.Email.Contains(keyword))
            )
            .OrderByDescending(u => u.RegDateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var users = await query.ToListAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var profiles = await _context.UserProfiles
            .Where(p => userIds.Contains(p.UserId))
            .ToDictionaryAsync(p => p.UserId);

        return users.Select(u =>
        {
            profiles.TryGetValue(u.Id, out var profile);
            return new SearchUsersResponseDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                Fullname = u.Fullname,
                AvatarUrl = u.AvatarUrl,
                Bio = u.Bio,
                FollowerCount = profile?.FollowerCount ?? 0,
                IsPrivateAccount = u.IsPrivateAccount,
            };
        });
    }

    public async Task<UserResponseDto?> GetUserProfileAsync(long id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == id);

        return MapToDto(user, profile);
    }

    public async Task<UserResponseDto?> UpdateProfileAsync(long userId, UpdateProfileRequestDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        if (dto.Fullname != null) user.Fullname = dto.Fullname;
        if (dto.Gender != null) user.Gender = dto.Gender;
        user.DateOfBirth = dto.DateOfBirth ?? user.DateOfBirth;
        if (dto.Bio != null) user.Bio = dto.Bio;
        if (dto.WebsiteUrl != null) user.WebsiteUrl = dto.WebsiteUrl;
        if (dto.Location != null) user.Location = dto.Location;
        user.IsPrivateAccount = dto.IsPrivateAccount;
        user.UpDatetime = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return null;

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);

        return MapToDto(user, profile);
    }

    public async Task<string?> UpdateAvatarAsync(long userId, string avatarUrl)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        user.AvatarUrl = avatarUrl;
        user.UpDatetime = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? avatarUrl : null;
    }

    public async Task<string?> UpdateCoverPhotoAsync(long userId, string coverUrl)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) return null;

        user.CoverPhotoUrl = coverUrl;
        user.UpDatetime = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded ? coverUrl : null;
    }
}
