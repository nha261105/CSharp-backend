using System.Threading.Tasks;
using InteractHub.Core.DTOs.Friends;

namespace InteractHub.Core.Interfaces.Services
{
    public interface IFriendsService
    {
        Task<(bool Success, string Message)> SendFriendRequestAsync(long currentUserId, SendFriendRequestDto dto);
        Task<bool> AcceptFriendRequestAsync(long currentUserId, AcceptFriendRequestDto dto);
        Task<bool> DeclineFriendRequestAsync(long currentUserId, long requesterId);
        Task<bool> UnfriendOrCancelRequestAsync(long currentUserId, UnfriendRequestDto dto);
        Task<bool> BlockUserAsync(long currentUserId, BlockUserRequestDto dto);
        Task<(bool Success, string Message)> UnblockUserAsync(long currentUserId, long targetUserId);
    }
}