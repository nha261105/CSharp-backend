using System.Security.Claims;
using FluentAssertions;
using InteractHub.API.Controllers;
using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InteractHub.Tests.Controllers;

public class PostsControllerTests
{
    private readonly Mock<IPostService> _postServiceMock;
    private readonly PostsController _controller;

    public PostsControllerTests()
    {
        _postServiceMock = new Mock<IPostService>();
        _controller = new PostsController(_postServiceMock.Object);

        // Setup mock user
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetPostById_ReturnsOkResult_WhenPostExists()
    {
        // Arrange
        var postId = 1L;
        var postDto = new PostDetailResponseDto { PostId = postId, Content = "Test Post" };
        _postServiceMock.Setup(s => s.GetPostDetailAsync(1L, postId))
                        .ReturnsAsync(postDto);

        // Act
        var result = await _controller.GetPostById(postId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPost = Assert.IsType<PostDetailResponseDto>(okResult.Value);
        returnPost.PostId.Should().Be(postId);
    }

    [Fact]
    public async Task GetPostById_ReturnsNotFound_WhenPostDoesNotExist()
    {
        // Arrange
        var postId = 1L;
        _postServiceMock.Setup(s => s.GetPostDetailAsync(1L, postId))
                        .ReturnsAsync((PostDetailResponseDto?)null);

        // Act
        var result = await _controller.GetPostById(postId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundResult.Value.Should().BeEquivalentTo(new { message = "Post không tồn tại" });
    }

    [Fact]
    public async Task GetPosts_ReturnsOkResult_WithPosts()
    {
        // Arrange
        var postsDto = new List<PostResponseDto> { new PostResponseDto { PostId = 1 } };
        _postServiceMock.Setup(s => s.GetListPostPageAsync(1L, 1, 20))
                        .ReturnsAsync(postsDto);

        // Act
        var result = await _controller.GetPosts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PostResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetUserPosts_ReturnsOkResult_WithPosts()
    {
        // Arrange
        var targetUserId = 2L;
        var postsDto = new List<PostResponseDto> { new PostResponseDto { PostId = 1 } };
        _postServiceMock.Setup(s => s.GetListUserPagePostAsync(1L, targetUserId, 1, 20))
                        .ReturnsAsync(postsDto);

        // Act
        var result = await _controller.GetUserPosts(targetUserId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PostResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreatePost_ReturnsCreatedAtAction_WhenSuccessful()
    {
        // Arrange
        var request = new CreatePostRequestDto { Content = "New Post" };
        var createdPost = new PostResponseDto { PostId = 1, Content = "New Post" };
        _postServiceMock.Setup(s => s.CreatePostAsync(1L, request))
                        .ReturnsAsync(createdPost);

        // Act
        var result = await _controller.CreatePost(request);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPostById));
        createdAtActionResult.RouteValues?["id"].Should().Be(1L);
        var returnPost = Assert.IsType<PostResponseDto>(createdAtActionResult.Value);
        returnPost.PostId.Should().Be(1L);
    }

    [Fact]
    public async Task UpdatePost_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var request = new UpdatePostRequestDto { Content = "Updated Post" };
        var updatedPost = new PostResponseDto { PostId = postId, Content = "Updated Post" };
        _postServiceMock.Setup(s => s.UpdatePostAsnc(1L, postId, request))
                        .ReturnsAsync(updatedPost);

        // Act
        var result = await _controller.UpdatePost(postId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPost = Assert.IsType<PostResponseDto>(okResult.Value);
        returnPost.Content.Should().Be("Updated Post");
    }

    [Fact]
    public async Task DeletePost_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        _postServiceMock.Setup(s => s.DeletePostAsync(1L, postId))
                        .ReturnsAsync(true);

        // Act
        var result = await _controller.DeletePost(postId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePost_ReturnsNotFound_WhenPostDoesNotExistOrNoPermission()
    {
        // Arrange
        var postId = 1L;
        _postServiceMock.Setup(s => s.DeletePostAsync(1L, postId))
                        .ReturnsAsync(false);

        // Act
        var result = await _controller.DeletePost(postId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        notFoundResult.Value.Should().BeEquivalentTo(new { message = "Post không tồn tại hoặc bạn không có quyền xóa" });
    }

    [Fact]
    public async Task TogglePostReaction_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var request = new PostReactionRequestDto { ReactionType = "Like" };
        _postServiceMock.Setup(s => s.TogglePostReactionAsync(1L, postId, "Like"))
                        .ReturnsAsync((1, "Like"));

        // Act
        var result = await _controller.TogglePostReaction(postId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(new { postId = postId, likeCount = 1, reactionType = "Like" });
    }

    [Fact]
    public async Task SharePost_ReturnsCreatedAtAction_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var request = new SharePostRequestDto { Content = "Shared" };
        var sharedPost = new PostResponseDto { PostId = 2 };
        _postServiceMock.Setup(s => s.SharePostAsync(1L, postId, request))
                        .ReturnsAsync(sharedPost);

        // Act
        var result = await _controller.SharePost(postId, request);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        createdAtActionResult.ActionName.Should().Be(nameof(PostsController.GetPostById));
        var returnPost = Assert.IsType<PostResponseDto>(createdAtActionResult.Value);
        returnPost.PostId.Should().Be(2L);
    }

    [Fact]
    public async Task AddComment_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var request = new CreateCommentRequestDto { Content = "Comment" };
        var newComment = new CommentResponseDto { CommentId = 1, Content = "Comment" };
        _postServiceMock.Setup(s => s.AddCommentAsync(1L, postId, request))
                        .ReturnsAsync(newComment);

        // Act
        var result = await _controller.AddComment(postId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnComment = Assert.IsType<CommentResponseDto>(okResult.Value);
        returnComment.CommentId.Should().Be(1L);
    }

    [Fact]
    public async Task UpdateComment_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var commentId = 1L;
        var request = new UpdateCommentRequestDto { Content = "Updated" };
        var updatedComment = new CommentResponseDto { CommentId = commentId, Content = "Updated" };
        _postServiceMock.Setup(s => s.UpdateCommentAsync(1L, postId, commentId, request))
                        .ReturnsAsync(updatedComment);

        // Act
        var result = await _controller.UpdateComment(postId, commentId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnComment = Assert.IsType<CommentResponseDto>(okResult.Value);
        returnComment.Content.Should().Be("Updated");
    }

    [Fact]
    public async Task DeleteComment_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var commentId = 1L;
        _postServiceMock.Setup(s => s.DeleteCommentAsync(1L, postId, commentId))
                        .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteComment(postId, commentId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ToggleCommentReaction_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var commentId = 1L;
        var request = new CommentReactionRequestDto { ReactionType = "Haha" };
        _postServiceMock.Setup(s => s.ToggleCommentReactionAsync(1L, postId, commentId, "Haha"))
                        .ReturnsAsync(5);

        // Act
        var result = await _controller.ToggleCommentReaction(postId, commentId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(new { commentId = commentId, likeCount = 5 });
    }

    [Fact]
    public async Task GetCommentReactionsDetail_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L; // Note: controller takes postId but doesn't use it for the service call
        var commentId = 1L;
        var details = new List<CommentReactionDetailResponseDto> { new CommentReactionDetailResponseDto { ReactionType = "Like", Count = 1 } };
        _postServiceMock.Setup(s => s.GetCommentReactionsDetailAsync(commentId))
                        .ReturnsAsync(details);

        // Act
        var result = await _controller.GetCommentReactionsDetail(postId, commentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CommentReactionDetailResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetPostCommentsList_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var commentsDto = new List<CommentResponseDto> { new CommentResponseDto { CommentId = 1 } };
        _postServiceMock.Setup(s => s.GetPostCommentsAsync(postId, 1, 10))
                        .ReturnsAsync(commentsDto);

        // Act
        var result = await _controller.GetPostCommentsList(postId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CommentResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetCommentReplies_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var commentId = 1L;
        var repliesDto = new List<CommentResponseDto> { new CommentResponseDto { CommentId = 2 } };
        _postServiceMock.Setup(s => s.GetCommentRepliesAsync(postId, commentId, 1, 10))
                        .ReturnsAsync(repliesDto);

        // Act
        var result = await _controller.GetCommentReplies(postId, commentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<CommentResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetPostReactionsDetail_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var postId = 1L;
        var detailsDto = new List<PostReactionDetailResponseDto> { new PostReactionDetailResponseDto { ReactionType = "Like", Count = 1 } };
        _postServiceMock.Setup(s => s.GetPostReactionsDetailAsync(postId, 1, 20))
                        .ReturnsAsync(detailsDto);

        // Act
        var result = await _controller.GetPostReactionsDetail(postId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<PostReactionDetailResponseDto>>(okResult.Value);
        returnValue.Should().HaveCount(1);
    }
}
