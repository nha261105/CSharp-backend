using System.Security.Claims;
using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InteractHub.API.Controllers;

[ApiController]
[Route("api/posts")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

     private long GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub");
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
            throw new UnauthorizedAccessException("Invalid user token");
        return userId;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(long id)
    {
        try
        {
            long userId = GetCurrentUserId();
            var post = await _postService.GetPostDetailAsync(userId, id);

            if (post == null)
            {
                return NotFound(new { message = "Post không tồn tại" });
            }

            return Ok(post);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 50) pageSize = 50;

            var userId = GetCurrentUserId();
            var posts = await _postService.GetListPostPageAsync(userId, page, pageSize);
            return Ok(posts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserPosts(long userId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            if (pageSize > 50) pageSize = 50;

            var currentUserId = GetCurrentUserId();
            var posts = await _postService.GetListUserPagePostAsync(currentUserId, userId, page, pageSize);
            return Ok(posts);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequestDto req)
    {
        try
        {
            var userId = GetCurrentUserId();
            var post = await _postService.CreatePostAsync(userId, req);
            return CreatedAtAction(nameof(GetPostById), new { id = post.PostId }, post);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(long id, [FromBody] UpdatePostRequestDto req)
    {
        try
        {
            var userId = GetCurrentUserId();
            var post = await _postService.UpdatePostAsnc(userId, id, req);

            if (post == null)
            {
                return NotFound(new { message = "Post không tồn tại hoặc bạn không có quyền sửa" });
            }

            return Ok(post);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(long id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var deleted = await _postService.DeletePostAsync(userId, id);

            if (!deleted)
            {
                return NotFound(new { message = "Post không tồn tại hoặc bạn không có quyền xóa" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPost("{id}/reaction")]
    public async Task<IActionResult> TogglePostReaction(long id, [FromBody] PostReactionRequestDto req)
    {
        try
        {
            var userId = GetCurrentUserId();
            var (likeCount, reactionType) = await _postService.TogglePostReactionAsync(userId, id, req.ReactionType);

            if (likeCount < 0)
            {
                return NotFound(new { message = "Post không tồn tại" });
            }

            return Ok(new { postId = id, likeCount, reactionType });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }


    [HttpPost("{id}/share")]
    public async Task<IActionResult> SharePost(long id, [FromBody] SharePostRequestDto req)
    {
        try
        {
            var userId = GetCurrentUserId();
            var sharedPost = await _postService.SharePostAsync(userId, id, req);

            if (sharedPost == null)
            {
                return NotFound(new { message = "Post không tồn tại" });
            }

            return CreatedAtAction(nameof(GetPostById), new { id = sharedPost.PostId }, sharedPost);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }
    [HttpPost("{id}/comments")]
    public async Task<IActionResult> AddComment(long id, [FromBody] CreateCommentRequestDto req)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var result = await _postService.AddCommentAsync(currentUserId, id, req);

            if (result == null)
            {
                return BadRequest(new { message = "Không thể thêm bình luận. Vui lòng kiểm tra lại dữ liệu." });
            }

            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }

    [HttpPut("{postId}/comments/{commentId}")]
    public async Task<IActionResult> UpdateComment(long postId, long commentId, [FromBody] UpdateCommentRequestDto req)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var result = await _postService.UpdateCommentAsync(currentUserId, postId, commentId, req);

            if (result == null)
            {
                return NotFound(new { message = "Bình luận không tồn tại hoặc đã bị xóa" });
            }

            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    [HttpDelete("{postId}/comments/{commentId}")]
    public async Task<IActionResult> DeleteComment(long postId, long commentId)
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var deleted = await _postService.DeleteCommentAsync(currentUserId, postId, commentId);

            if (!deleted)
            {
                return NotFound(new { message = "Bình luận không tồn tại hoặc đã bị xóa" });
            }

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    [HttpPost("{postId}/comments/{commentId}/reaction")]
    public async Task<IActionResult> ToggleCommentReaction(long postId, long commentId, [FromBody] CommentReactionRequestDto req)
    {
        try
        {
            var currentUserId = GetCurrentUserId();

            var likeCount = await _postService.ToggleCommentReactionAsync(currentUserId, postId, commentId, req.ReactionType);

            if (likeCount < 0)
            {
                return NotFound(new { message = "Bình luận không tồn tại, đã bị xóa, hoặc không thuộc bài viết này" });
            }

            return Ok(new { commentId, likeCount });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Token không hợp lệ" });
        }
    }
    [HttpGet("{postId}/comments/{commentId}/reactions-detail")]
    public async Task<IActionResult> GetCommentReactionsDetail(long postId, long commentId)
    {
        try
        {
            var result = await _postService.GetCommentReactionsDetailAsync(commentId);
            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy chi tiết cảm xúc" });
        }
    }
    [HttpGet("{id}/comments-list")]
public async Task<IActionResult> GetPostCommentsList(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
{
    try
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;
        var comments = await _postService.GetPostCommentsAsync(id, page, pageSize);
        
        return Ok(comments);
    }
    catch (Exception)
    {
        return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách bình luận" });
    }
}

[HttpGet("{postId}/comments/{commentId}/replies")]
public async Task<IActionResult> GetCommentReplies(long postId, long commentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
{
    try
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;
        var replies = await _postService.GetCommentRepliesAsync(postId, commentId, page, pageSize);
        return Ok(replies);
    }
    catch (Exception)
    {
        return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách replies" });
    }
}
[HttpGet("{id}/post-reactions-detail")]
public async Task<IActionResult> GetPostReactionsDetail(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
{
    try
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 50) pageSize = 50;

        var result = await _postService.GetPostReactionsDetailAsync(id, page, pageSize);
        return Ok(result);
    }
    catch (Exception)
    {
        return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách cảm xúc bài viết" });
    }
}
}