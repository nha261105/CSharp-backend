using System;
using System.Threading.Tasks;
using InteractHub.Core.DTOs.Friends;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly AppDbContext _context;
        private readonly INotificationsService _notificationsService;
        private readonly INotificationRealtimeService _notificationRealtimeService;

        public FriendsService(
            AppDbContext context,
            INotificationsService notificationsService,
            INotificationRealtimeService notificationRealtimeService)
        {
            _context = context;
            _notificationsService = notificationsService;
            _notificationRealtimeService = notificationRealtimeService;
        }

        public async Task<(bool Success, string Message)> SendFriendRequestAsync(long currentUserId, SendFriendRequestDto dto)
{
    if (currentUserId == dto.ReceiverId)
        return (false, "Bạn không thể gửi lời mời kết bạn cho chính mình.");

    var receiverExists = await _context.Users.AnyAsync(u => u.Id == dto.ReceiverId);
    if (!receiverExists)
        return (false, "Người dùng này không tồn tại.");

    
    var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
        (f.RequesterId == currentUserId && f.AddresseeId == dto.ReceiverId) ||
        (f.RequesterId == dto.ReceiverId && f.AddresseeId == currentUserId));

    if (friendship != null)
    {
        
        if (friendship.IsBlocked)
            return (false, "Không thể gửi lời mời. Người dùng này đã bị chặn hoặc bạn đã bị họ chặn.");

       
        if (friendship.Delflg == false)
        {
            if (friendship.Status == "Pending")
                return (false, "Lời mời kết bạn đã tồn tại.");
            
            if (friendship.Status == "Accepted")
                return (false, "Hai người đã là bạn bè.");
        }

        friendship.RequesterId = currentUserId; 
        friendship.AddresseeId = dto.ReceiverId;
        friendship.Status = "Pending";
        friendship.ActionUserId = currentUserId;
        friendship.Delflg = false; 

       
        friendship.RegDatetime = DateTime.UtcNow; 
        friendship.UpdDatetime = null;            
    }
    else
    {
       
        friendship = new Friendship
        {
            RequesterId = currentUserId,
            AddresseeId = dto.ReceiverId,
            Status = "Pending",
            ActionUserId = currentUserId,
            RegDatetime = DateTime.UtcNow,
            UpdDatetime = null, 
            Delflg = false,
            IsBlocked = false
        };
        _context.Friendships.Add(friendship);
    }

    var result = await _context.SaveChangesAsync() > 0;
    if (result)
    {
        var notification = await _notificationsService.CreateNotificationAsync(
            recipientId: dto.ReceiverId,
            notificationType: "FriendRequestSent",
            senderId: currentUserId,
            referenceId: dto.ReceiverId,
            referenceType: "FriendRequest",
            message: "đã gửi lời mời kết bạn cho bạn",
            redirectUrl: $"/profile/{currentUserId}");

        await _notificationRealtimeService.PushNotificationCreatedAsync(dto.ReceiverId, notification);
    }
    return result ? (true, "Gửi lời mời thành công.") : (false, "Lỗi hệ thống khi lưu dữ liệu.");
}

        public async Task<bool> AcceptFriendRequestAsync(long currentUserId, AcceptFriendRequestDto dto)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.RequesterId == dto.RequesterId &&
                f.AddresseeId == currentUserId &&
                f.Status == "Pending" &&
                f.Delflg == false);

            if (friendship == null) return false;

            friendship.Status = "Accepted";
            friendship.ActionUserId = currentUserId;
            friendship.UpdDatetime = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync() > 0;
            if (result)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: dto.RequesterId,
                    notificationType: "FriendRequestAccepted",
                    senderId: currentUserId,
                    referenceId: currentUserId,
                    referenceType: "FriendRequest",
                    message: "đã chấp nhận lời mời kết bạn của bạn",
                    redirectUrl: $"/profile/{currentUserId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(dto.RequesterId, notification);
            }
            return result;
        }

        public async Task<bool> DeclineFriendRequestAsync(long currentUserId, long requesterId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.RequesterId == requesterId && 
                f.AddresseeId == currentUserId && 
                f.Status == "Pending" &&
                f.Delflg == false);

            if (friendship == null) return false;

            friendship.Status = "Declined";
            friendship.Delflg = true; 
            friendship.UpdDatetime = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnfriendOrCancelRequestAsync(long currentUserId, UnfriendRequestDto dto)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                ((f.RequesterId == currentUserId && f.AddresseeId == dto.TargetUserId) ||
                 (f.RequesterId == dto.TargetUserId && f.AddresseeId == currentUserId)) &&
                f.Delflg == false);

            if (friendship == null) return false;

            friendship.Status = "Unfriended";
            friendship.Delflg = true; 
            friendship.UpdDatetime = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> BlockUserAsync(long currentUserId, BlockUserRequestDto dto)
        {
            if (currentUserId == dto.TargetUserId) return false;

            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == currentUserId && f.AddresseeId == dto.TargetUserId) ||
                (f.RequesterId == dto.TargetUserId && f.AddresseeId == currentUserId));

            if (friendship != null)
            {
                
                if (friendship.IsBlocked && friendship.BlockedById == currentUserId) return true;

                friendship.Status = "Blocked";
                friendship.IsBlocked = true;
                friendship.BlockedById = currentUserId; 
                friendship.ActionUserId = currentUserId;
                friendship.Delflg = true; 
                friendship.UpdDatetime = DateTime.UtcNow;
            }
            else
            {
                friendship = new Friendship
                {
                    RequesterId = currentUserId,
                    AddresseeId = dto.TargetUserId,
                    Status = "Blocked",
                    IsBlocked = true,
                    BlockedById = currentUserId,
                    ActionUserId = currentUserId,
                    RegDatetime = DateTime.UtcNow,
                    Delflg = true 
                };
                _context.Friendships.Add(friendship);
            }

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<BlockedUserResponseDto>> GetBlockedListAsync(long currentUserId)
        {
           
            var blockedFriendships = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Where(f => f.IsBlocked == true && f.BlockedById == currentUserId)
                .ToListAsync();

            return blockedFriendships.Select(f => {
               
                var targetUser = f.RequesterId == currentUserId ? f.Addressee : f.Requester;
                return new BlockedUserResponseDto
                {
                    UserId = targetUser.Id,
                    Fullname = targetUser.Fullname,
                    AvatarUrl = targetUser.AvatarUrl
                };
            }).ToList();
        }
        public async Task<(bool Success, string Message)> UnblockUserAsync(long currentUserId, long targetUserId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.BlockedById == currentUserId &&
                ((f.RequesterId == targetUserId && f.AddresseeId == currentUserId) || 
                (f.RequesterId == currentUserId && f.AddresseeId == targetUserId)) &&
                f.IsBlocked == true);

            if (friendship == null)
                return (false, "Người dùng này không nằm trong danh sách chặn của bạn.");

            friendship.IsBlocked = false;
            friendship.BlockedById = null;
            friendship.Status = "None"; 
            friendship.Delflg = true;  
            friendship.UpdDatetime = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync() > 0;
            return result ? (true, "Đã gỡ chặn thành công.") : (false, "Lỗi cập nhật dữ liệu.");
        }
        private async Task<List<long>> GetFriendIdsAsync(long userId)
        {
            return await _context.Friendships
                .Where(f => (f.RequesterId == userId || f.AddresseeId == userId) 
                            && f.Status == "Accepted" && f.Delflg == false)
                .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();
        }

        private string GetRelativeTime(DateTime date)
        {
            var timeSpan = DateTime.UtcNow - date;
            if (timeSpan.TotalMinutes < 1) return "Vừa xong";
            if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes} phút trước";
            if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours} giờ trước";
            return $"{(int)timeSpan.TotalDays} ngày trước";
        }

        private async Task<List<MutualFriendResponseDto>> GetMutualFriendsInfoAsync(long currentUserId, List<long> mutualIds, int? takeCount = null)
        {
            if (mutualIds == null || !mutualIds.Any()) return new List<MutualFriendResponseDto>();

            var myFriendIds = await GetFriendIdsAsync(currentUserId);
            
           
            var query = _context.Users.Where(u => mutualIds.Contains(u.Id));
            if (takeCount.HasValue) query = query.Take(takeCount.Value);

            var users = await query.ToListAsync();
            var result = new List<MutualFriendResponseDto>();

            foreach (var u in users)
            {
                var theirFriendIds = await GetFriendIdsAsync(u.Id);
                var commonIds = myFriendIds.Intersect(theirFriendIds).ToList();

                result.Add(new MutualFriendResponseDto
                {
                    UserId = u.Id,
                    Fullname = u.Fullname,
                    AvatarUrl = u.AvatarUrl,
                    MutualFriendsCount = commonIds.Count,
                   
                    TopMutualAvatars = await _context.Users
                        .Where(sub => commonIds.Take(3).Contains(sub.Id))
                        .Select(sub => sub.AvatarUrl)
                        .ToListAsync()
                });
            }
            return result;
        }

        public async Task<List<FriendResponseDto>> GetFriendsListAsync(long currentUserId)
        {
            var myFriendIds = await GetFriendIdsAsync(currentUserId);
            var friendships = await _context.Friendships
                .Include(f => f.Requester).Include(f => f.Addressee)
                .Where(f => (f.RequesterId == currentUserId || f.AddresseeId == currentUserId) 
                            && f.Status == "Accepted" && f.Delflg == false)
                .ToListAsync();

            var result = new List<FriendResponseDto>();
            foreach (var f in friendships)
            {
                var targetUser = f.RequesterId == currentUserId ? f.Addressee : f.Requester;
                var theirFriendIds = await GetFriendIdsAsync(targetUser.Id);
                var mutualIds = myFriendIds.Intersect(theirFriendIds).ToList();

                result.Add(new FriendResponseDto {
                    UserId = targetUser.Id,
                    FullName = targetUser.Fullname,
                    AvatarUrl = targetUser.AvatarUrl,
                    ActionDate = GetRelativeTime(f.UpdDatetime ?? f.RegDatetime),
                    MutualFriendsCount = mutualIds.Count,
                    TopMutualFriends = await GetMutualFriendsInfoAsync(currentUserId, mutualIds, 3)
                });
            }
            return result;
        }

        public async Task<List<FriendResponseDto>> GetPendingRequestsAsync(long currentUserId)
        {
            var myFriendIds = await GetFriendIdsAsync(currentUserId);
            var requests = await _context.Friendships
                .Include(f => f.Requester)
                .Where(f => f.AddresseeId == currentUserId && f.Status == "Pending" && f.Delflg == false)
                .ToListAsync();

            var result = new List<FriendResponseDto>();
            foreach (var req in requests)
            {
                var theirFriendIds = await GetFriendIdsAsync(req.RequesterId);
                var mutualIds = myFriendIds.Intersect(theirFriendIds).ToList();

                result.Add(new FriendResponseDto {
                    UserId = req.RequesterId,
                    FullName = req.Requester.Fullname,
                    AvatarUrl = req.Requester.AvatarUrl,
                    ActionDate = GetRelativeTime(req.RegDatetime),
                    MutualFriendsCount = mutualIds.Count,
                    TopMutualFriends = await GetMutualFriendsInfoAsync(currentUserId, mutualIds, 3)
                });
            }
            return result;
        }

        public async Task<List<MutualFriendResponseDto>> GetMutualFriendsDetailAsync(long currentUserId, long targetUserId)
        {
            var myFriendIds = await GetFriendIdsAsync(currentUserId);
            var theirFriendIds = await GetFriendIdsAsync(targetUserId);
            var mutualIds = myFriendIds.Intersect(theirFriendIds).ToList();

            return await GetMutualFriendsInfoAsync(currentUserId, mutualIds);
        }

        public async Task<List<FriendSuggestionDto>> GetFriendSuggestionsAsync(long currentUserId, int limit)
        {
            // ✅ FIX: Query 1 - Lấy bạn bè của currentUser
            var myFriendIds = await GetFriendIdsAsync(currentUserId);

            // ✅ FIX: Nếu không có bạn bè, return empty (không có gợi ý)
            if (!myFriendIds.Any())
            {
                return new List<FriendSuggestionDto>();
            }

            // ✅ FIX: Query 2 - Lấy tất cả IDs cần loại trừ
            var excludedIds = new HashSet<long>(myFriendIds) { currentUserId };

            var pendingOrBlockedIds = await _context.Friendships
                .AsNoTracking()
                .Where(f => (f.RequesterId == currentUserId || f.AddresseeId == currentUserId)
                            && (f.Status == "Pending" || f.IsBlocked)
                            && f.Delflg == false)
                .Select(f => f.RequesterId == currentUserId ? f.AddresseeId : f.RequesterId)
                .ToListAsync();

            foreach (var id in pendingOrBlockedIds) excludedIds.Add(id);

            // ✅ FIX: Query 3 - Lấy candidates (bạn của bạn bè)
            var candidates = await _context.Friendships
                .AsNoTracking()
                .Where(f => (myFriendIds.Contains(f.RequesterId) || myFriendIds.Contains(f.AddresseeId))
                            && f.Status == "Accepted" && f.Delflg == false)
                .Select(f => myFriendIds.Contains(f.RequesterId) ? f.AddresseeId : f.RequesterId)
                .Distinct()
                .Where(id => !excludedIds.Contains(id))
                .ToListAsync();

            // ✅ FIX: Nếu không có candidates, return empty
            if (!candidates.Any())
            {
                return new List<FriendSuggestionDto>();
            }

            // ✅ FIX: Query 4 - Lấy user info
            var users = await _context.Users
                .Where(u => candidates.Contains(u.Id) && u.Delflg == false && u.IsActive)
                .Select(u => new { u.Id, u.UserName, u.Fullname, u.AvatarUrl })
                .AsNoTracking()
                .ToListAsync();

            // ✅ FIX: Query 5 - SINGLE QUERY để lấy TẤT CẢ friendships của candidates
            // Thay vì query trong loop (N queries), ta query 1 lần cho tất cả
            var candidateIds = users.Select(u => u.Id).ToList();
            
            var allCandidateFriendships = await _context.Friendships
                .Where(f => f.Status == "Accepted" && !f.IsBlocked && !f.Delflg &&
                            (candidateIds.Contains(f.RequesterId) || candidateIds.Contains(f.AddresseeId)))
                .Select(f => new { f.RequesterId, f.AddresseeId })
                .AsNoTracking()
                .ToListAsync();

            // ✅ FIX: Build friendship map trong memory
            // Dictionary<UserId, HashSet<FriendIds>>
            var candidateFriendshipMap = new Dictionary<long, HashSet<long>>();
            foreach (var f in allCandidateFriendships)
            {
                if (!candidateFriendshipMap.ContainsKey(f.RequesterId))
                    candidateFriendshipMap[f.RequesterId] = new HashSet<long>();
                if (!candidateFriendshipMap.ContainsKey(f.AddresseeId))
                    candidateFriendshipMap[f.AddresseeId] = new HashSet<long>();
                
                candidateFriendshipMap[f.RequesterId].Add(f.AddresseeId);
                candidateFriendshipMap[f.AddresseeId].Add(f.RequesterId);
            }

            // ✅ FIX: Convert myFriendIds sang HashSet để Intersect nhanh hơn
            var myFriendsSet = new HashSet<long>(myFriendIds);

            // ✅ FIX: Tính mutual friends trong memory (KHÔNG query database)
            var result = new List<FriendSuggestionDto>();
            foreach (var u in users)
            {
                var theirFriends = candidateFriendshipMap.GetValueOrDefault(u.Id, new HashSet<long>());
                var mutualCount = myFriendsSet.Intersect(theirFriends).Count();
                
                result.Add(new FriendSuggestionDto
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    FullName = u.Fullname,
                    AvatarUrl = u.AvatarUrl,
                    MutualFriendsCount = mutualCount
                });
            }

            // ✅ FIX: Sort và limit trong memory (rất nhanh)
            return result
                .OrderByDescending(x => x.MutualFriendsCount)
                .ThenBy(x => x.FullName) // Thêm secondary sort để consistent
                .Take(limit)
                .ToList();
        }
    }
}