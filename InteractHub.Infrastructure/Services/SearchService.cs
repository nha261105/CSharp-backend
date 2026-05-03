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
        // ✅ FIX: Thêm null check để tránh NullReferenceException
        var users = await _context.Users
            .Where(u => !u.Delflg &&
                ((u.UserName != null && u.UserName.ToLower().Contains(lower)) ||
                 (u.Fullname != null && u.Fullname.ToLower().Contains(lower)) ||
                 (u.Email != null && u.Email.ToLower().Contains(lower))))
            .OrderBy(u => u.Fullname)
            .Skip(skip)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        // ✅ FIX: Nếu không có currentUserId, return ngay không cần tính mutual friends
        if (!currentUserId.HasValue)
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

        //  FIX: SINGLE QUERY - Lấy tất cả friendships cần thiết trong 1 lần
        // Thay vì query trong loop (N+1 problem), ta query 1 lần cho tất cả users
        var userIds = users.Select(u => u.Id).ToList();
        userIds.Add(currentUserId.Value); // Thêm currentUser vào list

        // Query 1 lần duy nhất để lấy tất cả friendships liên quan
        var allFriendships = await _context.Friendships
            .Where(f => f.Status == "Accepted" && !f.IsBlocked && !f.Delflg &&
                        (userIds.Contains(f.RequesterId) || userIds.Contains(f.AddresseeId)))
            .Select(f => new { f.RequesterId, f.AddresseeId })
            .AsNoTracking()
            .ToListAsync();

        // FIX: Build friendship map trong memory (rất nhanh)
        // Dictionary cho phép lookup O(1) thay vì query database O(n)
        var friendshipMap = new Dictionary<long, HashSet<long>>();
        foreach (var f in allFriendships)
        {
            // Tạo bidirectional mapping
            if (!friendshipMap.ContainsKey(f.RequesterId))
                friendshipMap[f.RequesterId] = new HashSet<long>();
            if (!friendshipMap.ContainsKey(f.AddresseeId))
                friendshipMap[f.AddresseeId] = new HashSet<long>();
            
            friendshipMap[f.RequesterId].Add(f.AddresseeId);
            friendshipMap[f.AddresseeId].Add(f.RequesterId);
        }

        // Lấy danh sách bạn bè của currentUser
        var myFriends = friendshipMap.GetValueOrDefault(currentUserId.Value, new HashSet<long>());

        // ✅ FIX: Tính mutual friends trong memory (không query database)
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
