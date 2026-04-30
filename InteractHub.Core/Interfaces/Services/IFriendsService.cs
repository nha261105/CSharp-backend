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
        Task<List<FriendResponseDto>> GetFriendsListAsync(long currentUserId);
        Task<List<FriendResponseDto>> GetPendingRequestsAsync(long currentUserId);
        Task<List<MutualFriendResponseDto>> GetMutualFriendsDetailAsync(long currentUserId, long targetUserId);
        Task<List<BlockedUserResponseDto>> GetBlockedListAsync(long currentUserId);

        /// <summary>
        /// Gợi ý kết bạn dựa trên 2-hop (bạn của bạn bè).
        /// Loại bỏ: đã là bạn, pending request (cả 2 chiều), bị block, chính mình.
        /// Sắp xếp theo mutualFriendsCount giảm dần.
        /// </summary>
        Task<List<FriendSuggestionDto>> GetFriendSuggestionsAsync(long currentUserId, int limit);
    }
}