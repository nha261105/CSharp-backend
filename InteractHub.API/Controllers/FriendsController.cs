using System.Security.Claims;
using System.Threading.Tasks;
using InteractHub.Core.DTOs.Friends;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/friends")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService _friendsService;

        public FriendsController(IFriendsService friendsService)
        {
            _friendsService = friendsService;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestDto dto)
        {
            var (success, message) = await _friendsService.SendFriendRequestAsync(GetUserId(), dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpPut("accept-request")]
        public async Task<IActionResult> AcceptRequest([FromBody] AcceptFriendRequestDto dto)
        {
            var result = await _friendsService.AcceptFriendRequestAsync(GetUserId(), dto);
            return result ? Ok(new { message = "Đã trở thành bạn bè" }) 
                          : BadRequest(new { message = "Thao tác chấp nhận thất bại" });
        }

        [HttpDelete("decline-request/{requesterId}")]
        public async Task<IActionResult> DeclineRequest(long requesterId)
        {
            var result = await _friendsService.DeclineFriendRequestAsync(GetUserId(), requesterId);
            return result ? Ok(new { message = "Đã từ chối lời mời kết bạn" }) 
                          : BadRequest(new { message = "Từ chối thất bại hoặc lời mời không tồn tại" });
        }

        [HttpDelete("unfriend")]
        public async Task<IActionResult> Unfriend([FromBody] UnfriendRequestDto dto)
        {
            var result = await _friendsService.UnfriendOrCancelRequestAsync(GetUserId(), dto);
            return result ? Ok(new { message = "Đã thực hiện xóa quan hệ" }) 
                          : BadRequest(new { message = "Thao tác thất bại" });
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserRequestDto dto)
        {
            var result = await _friendsService.BlockUserAsync(GetUserId(), dto);
            return result ? Ok(new { message = "Đã chặn người dùng" }) 
                          : BadRequest(new { message = "Chặn thất bại" });
        }

        [HttpPut("unblock/{targetUserId}")]
        public async Task<IActionResult> UnblockUser(long targetUserId)
        {
            var (success, message) = await _friendsService.UnblockUserAsync(GetUserId(), targetUserId);
            return success ? Ok(new { message }) 
                           : BadRequest(new { message });
        }

        private long GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? long.Parse(userIdClaim.Value) : 0;
        }
        [HttpGet("my-friends")]
        public async Task<IActionResult> GetFriendsList()
        {
            return Ok(await _friendsService.GetFriendsListAsync(GetUserId()));
        }

        [HttpGet("pending-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            return Ok(await _friendsService.GetPendingRequestsAsync(GetUserId()));
        }
        [HttpGet("mutual-friends/{targetUserId}")]
        public async Task<IActionResult> GetMutualFriendsDetail(long targetUserId)
        {
            return Ok(await _friendsService.GetMutualFriendsDetailAsync(GetUserId(), targetUserId));
        }
        [HttpGet("blocked-users")]
        public async Task<IActionResult> GetBlockedList()
        {
            var result = await _friendsService.GetBlockedListAsync(GetUserId());
            return Ok(result);
        }

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetSuggestions([FromQuery] int limit = 10)
        {
            var result = await _friendsService.GetFriendSuggestionsAsync(GetUserId(), limit);
            return Ok(result);
        }
    }
}