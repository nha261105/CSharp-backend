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

        public FriendsService(AppDbContext context)
        {
            _context = context;
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
                friendship.UpdDatetime = DateTime.UtcNow;
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
                    Delflg = false,
                    IsBlocked = false
                };
                _context.Friendships.Add(friendship);
            }

            var result = await _context.SaveChangesAsync() > 0;
            return result ? (true, "Gửi lời mời thành công.") : (false, "Lỗi hệ thống.");
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

            return await _context.SaveChangesAsync() > 0;
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
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == currentUserId && f.AddresseeId == dto.TargetUserId) ||
                (f.RequesterId == dto.TargetUserId && f.AddresseeId == currentUserId));

            if (friendship == null)
            {
                friendship = new Friendship
                {
                    RequesterId = currentUserId,
                    AddresseeId = dto.TargetUserId,
                    RegDatetime = DateTime.UtcNow,
                    Delflg = false 
                };
                _context.Friendships.Add(friendship);
            }

            friendship.Status = "Blocked";
            friendship.IsBlocked = true;
            friendship.BlockedById = currentUserId;
            friendship.ActionUserId = currentUserId;
            friendship.Delflg = false; 
            friendship.UpdDatetime = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<(bool Success, string Message)> UnblockUserAsync(long currentUserId, long targetUserId)
        {
            var friendship = await _context.Friendships.FirstOrDefaultAsync(f =>
                f.BlockedById == currentUserId &&
                (f.RequesterId == targetUserId || f.AddresseeId == targetUserId));

            if (friendship == null)
                return (false, "Bạn chưa chặn người này.");

            friendship.IsBlocked = false;
            friendship.BlockedById = null;
            friendship.Status = "Unblocked";
            friendship.Delflg = true; 
            friendship.UpdDatetime = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync() > 0;
            return result ? (true, "Đã gỡ chặn thành công.") : (false, "Lỗi cập nhật dữ liệu.");
        }
    }
}