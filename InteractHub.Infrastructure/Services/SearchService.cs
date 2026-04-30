using InteractHub.Core.DTOs.Search;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly AppDbContext _context;

    public SearchService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GlobalSearchResponseDto> GlobalSearchAsync(
        long? currentUserId,
        string keyword,
        int page,
        int pageSize)
    {
        var lower = keyword.ToLower().Trim();
        var skip = (page - 1) * pageSize;

        // Chạy 2 query song song để giảm latency
        var usersTask = SearchUsersAsync(currentUserId, lower, skip, pageSize);
        var postsTask = SearchPostsAsync(lower, skip, pageSize);

        await Task.WhenAll(usersTask, postsTask);

        return new GlobalSearchResponseDto
        {
            Keyword = keyword,
            Users = usersTask.Result,
            Posts = postsTask.Result
        };
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<List<SearchUserResultDto>> SearchUsersAsync(
        long? currentUserId, string lower, int skip, int pageSize)
    {
        var users = await _context.Users
            .Where(u => !u.Delflg &&
                (u.UserName!.ToLower().Contains(lower) ||
                 u.Fullname!.ToLower().Contains(lower) ||
                 u.Email!.ToLower().Contains(lower)))
            .OrderBy(u => u.Fullname)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        // Đếm mutual friends nếu có currentUserId
        var result = new List<SearchUserResultDto>();
        foreach (var u in users)
        {
            var mutual = 0;
            if (currentUserId.HasValue && u.Id != currentUserId.Value)
            {
                // Bạn bè của currentUser
                var myFriends = await _context.Friendships
                    .Where(f => f.Status == "Accepted" && !f.IsBlocked && !f.Delflg &&
                                (f.RequesterId == currentUserId.Value || f.AddresseeId == currentUserId.Value))
                    .Select(f => f.RequesterId == currentUserId.Value ? f.AddresseeId : f.RequesterId)
                    .ToListAsync();

                // Bạn bè của user kết quả
                var theirFriends = await _context.Friendships
                    .Where(f => f.Status == "Accepted" && !f.IsBlocked && !f.Delflg &&
                                (f.RequesterId == u.Id || f.AddresseeId == u.Id))
                    .Select(f => f.RequesterId == u.Id ? f.AddresseeId : f.RequesterId)
                    .ToListAsync();

                mutual = myFriends.Intersect(theirFriends).Count();
            }

            result.Add(new SearchUserResultDto
            {
                UserId = u.Id,
                UserName = u.UserName ?? string.Empty,
                FullName = u.Fullname ?? string.Empty,
                AvatarUrl = u.AvatarUrl,
                MutualFriendsCount = mutual
            });
        }

        return result;
    }

    private async Task<List<SearchPostResultDto>> SearchPostsAsync(
        string lower, int skip, int pageSize)
    {
        return await _context.Posts
            .Where(p => !p.Delflg &&
                        p.Visibility == "Public" &&
                        p.Content != null &&
                        p.Content.ToLower().Contains(lower))
            .Include(p => p.User)
            .OrderByDescending(p => p.RegDatetime)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .Select(p => new SearchPostResultDto
            {
                PostId = p.PostId,
                UserId = p.UserId,
                UserName = p.User.UserName ?? string.Empty,
                FullName = p.User.Fullname ?? string.Empty,
                AvatarUrl = p.User.AvatarUrl,
                Content = p.Content,
                PostType = p.PostType,
                LikeCount = p.LikeCount,
                CommentCount = p.CommentCount,
                ShareCount = p.ShareCount,
                RegDateTime = p.RegDatetime
            })
            .ToListAsync();
    }
}
