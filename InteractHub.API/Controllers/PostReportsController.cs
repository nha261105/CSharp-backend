using InteractHub.Core.DTOs.PostReports;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InteractHub.API.Controllers
{
    [ApiController]
    [Route("api/postreports")]
    public class PostReportsController : ControllerBase
    {
        private readonly IPostReportService _reportService;

        public PostReportsController(IPostReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReport([FromBody] CreatePostReportRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) 
                return Unauthorized(new { message = "Bạn cần đăng nhập trước khi thực hiện báo cáo." });
            
          
            if (User.IsInRole("Admin") || User.IsInRole("Moderator"))
            {
                return BadRequest(new { message = "Tài khoản quản trị không thể thực hiện chức năng báo cáo." });
            }
            
            long userId = long.Parse(userIdClaim.Value);

           
            if (dto.PostId <= 0)
                return BadRequest(new { message = "Mã bài viết không hợp lệ." });

            if (string.IsNullOrWhiteSpace(dto.Reason))
                return BadRequest(new { message = "Lý do báo cáo không được để trống." });

            var validReasons = new[] { "Spam", "Harassment", "FakeNews", "HateSpeech", "Violence", "Nudity", "Other" };
            if (!validReasons.Contains(dto.Reason))
                return BadRequest(new { message = "Hãy chọn 1 trong các lỗi sau: Spam, Harassment, FakeNews, HateSpeech, Violence, Nudity, Other" });

            try 
            {
                
                var reportData = await _reportService.GetReportsByPostIdAsync(dto.PostId, 1, int.MaxValue);
                if (reportData.Items.Any(r => r.ReporterId == userId))
                    return BadRequest(new { message = "Bạn đã gửi báo cáo cho bài viết này rồi." });

                await _reportService.CreateReportAsync(userId, dto);
                return Ok(new { message = "Gửi báo cáo thành công." });
            }
            catch (KeyNotFoundException)
            {
                
                return NotFound(new { message = "Bài viết không tồn tại." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Đã có lỗi xảy ra. Vui lòng thử lại sau." });
            }
        }

        [HttpPut("post/{postId}/status")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UpdateStatus(long postId, [FromBody] UpdatePostReportRequestDto dto)
        {
            var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (adminIdClaim == null) return Unauthorized();
            long adminId = long.Parse(adminIdClaim.Value);

            
            var reports = await _reportService.GetReportsByPostIdAsync(postId, 1, 1);
            if (reports.TotalCount == 0)
                return NotFound(new { message = "Không tìm thấy dữ liệu báo cáo cho bài viết này." });

            var currentReport = reports.Items.First();

            
            if (currentReport.ReviewedById.HasValue && currentReport.ReviewedById.Value != adminId)
            {
                return BadRequest(new { 
                    message = $"Bài viết này đang được một quản trị viên khác (ID: {currentReport.ReviewedById}) xử lý." 
                });
            }

            
            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest(new { message = "Trạng thái mới không hợp lệ." });

            if (currentReport.Status == "Pending")
            {
                if (dto.Status != "Reviewing")
                    return BadRequest(new { message = "Bạn cần chuyển trạng thái sang 'Reviewing' trước khi xử lý." });
            }
            else if (currentReport.Status == "Reviewing")
            {
                if (dto.Status == "Resolved")
                {
                    var validActions = new[] { "PostRemoved", "UserWarned", "UserBanned" };
                    if (string.IsNullOrEmpty(dto.ActionTaken) || !validActions.Contains(dto.ActionTaken))
                        return BadRequest(new { message = "Vui lòng chọn hành động xử lý cụ thể (PostRemoved, UserWarned, UserBanned)." });
                }
                else if (dto.Status == "Dismissed")
                {
                    dto.ActionTaken = "NoAction";
                }
                else if (dto.Status != "Reviewing")
                {
                    return BadRequest(new { message = "Trạng thái sau khi Review chỉ có thể là Resolved hoặc Dismissed." });
                }
            }
            else 
            {
                return BadRequest(new { message = "Báo cáo này đã được xử lý, không thể thay đổi thêm." });
            }

            try 
            {
                await _reportService.UpdateReportStatusByPostAsync(adminId, postId, dto);
                return Ok(new { message = "Cập nhật trạng thái xử lý thành công." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bài viết hoặc dữ liệu liên quan." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Thao tác thất bại. Vui lòng kiểm tra lại." });
            }
        }

        [HttpDelete("post/{postId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromBody] DeletePostReportRequestDto dto)
        {
            try 
            {
            
                await _reportService.DeleteReportsByPostAsync(dto.PostId); 
                return Ok(new { message = "Xóa toàn bộ báo cáo của bài viết thành công." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy báo cáo nào liên quan." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Đã xảy ra lỗi trong quá trình xóa." });
            }
        }

        [HttpGet("summary")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetAllGrouped([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var reports = await _reportService.GetAllGroupedReportsAsync(page, pageSize);
            return Ok(reports);
        }

        [HttpGet("post/{postId}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetReportsByPost(long postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var reports = await _reportService.GetReportsByPostIdAsync(postId, page, pageSize);
            return Ok(reports);
        }

    }
}