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

        // Chạy tuần tự thay vì Task.WhenAll
        var users = await SearchUsersAsync(currentUserId, lower, skip, pageSize);
        var posts = await SearchPostsAsync(lower, skip, pageSize);

        return new GlobalSearchResponseDto
        {
            Keyword = keyword,
            Users = users,
            Posts = posts
        };
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<List<SearchUserResultDto>> SearchUsersAsync(
    long? currentUserId, string lower, int skip, int pageSize)
    {
        var searchPattern = $"%{lower}%";
        var users = await _context.Users
            .Where(u => !u.Delflg &&
                        (EF.Functions.Like(u.UserName, searchPattern) ||
                        EF.Functions.Like(u.Fullname, searchPattern) ||
                        EF.Functions.Like(u.Email, searchPattern)))
            .OrderBy(u => u.Fullname)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        // ✅ FIX: Nếu không có currentUserId, return ngay không cần tính mutual friends
        if (!currentUserId.HasValue || users.Count == 0)
        {
            return users.Select(u => new SearchUserResultDto
            {
                UserId = u.Id,
                UserName = u.UserName ?? string.Empty,
                FullName = u.Fullname ?? string.Empty,
                AvatarUrl = u.AvatarUrl,
                MutualFriendsCount = 0
            }).ToList();
        }

        var userIds = users.Select(u => u.Id).ToList();
        userIds.Add(currentUserId.Value); 

        var allFriendships = await _context.Friendships
            .Where(f => f.Status == "Accepted" && !f.IsBlocked && !f.Delflg &&
                        (userIds.Contains(f.RequesterId) || userIds.Contains(f.AddresseeId)))
            .Select(f => new { f.RequesterId, f.AddresseeId })
            .AsNoTracking()
            .ToListAsync();

        var friendshipMap = new Dictionary<long, HashSet<long>>();
        foreach (var f in allFriendships)
        {
            if (!friendshipMap.ContainsKey(f.RequesterId))
                friendshipMap[f.RequesterId] = new HashSet<long>();
            if (!friendshipMap.ContainsKey(f.AddresseeId))
                friendshipMap[f.AddresseeId] = new HashSet<long>();
            
            friendshipMap[f.RequesterId].Add(f.AddresseeId);
            friendshipMap[f.AddresseeId].Add(f.RequesterId);
        }

        var myFriends = friendshipMap.GetValueOrDefault(currentUserId.Value, new HashSet<long>());

        var result = new List<SearchUserResultDto>();
        foreach (var u in users)
        {
            var theirFriends = friendshipMap.GetValueOrDefault(u.Id, new HashSet<long>());
            var mutual = myFriends.Intersect(theirFriends).Count();

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
        var searchPattern = $"%{lower}%";
        return await _context.Posts
            .Where(p => !p.Delflg &&
                        p.Visibility == "Public" &&
                        p.Content != null &&
                        EF.Functions.Like(p.Content, searchPattern)) 
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
