using FluentAssertions;
using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using InteractHub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InteractHub.Tests.Services;

public class PostsServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<INotificationsService> _mockNotificationService;
    private readonly Mock<INotificationRealtimeService> _mockRealtimeService;
    private readonly PostsService _postsService;

    public PostsServiceTests()
    {
        // Setup In-Memory Database
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        // Mock dependencies
        _mockNotificationService = new Mock<INotificationsService>();
        _mockRealtimeService = new Mock<INotificationRealtimeService>();

        _postsService = new PostsService(
            _context,
            _mockNotificationService.Object,
            _mockRealtimeService.Object
        );

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var user = new User
        {
            Id = 1,
            UserName = "testuser",
            Fullname = "Test User",
            Email = "test@example.com",
            AvatarUrl = "avatar.jpg",
            Delflg = false
        };

        _context.Users.Add(user);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreatePost_ReturnsPostDto_WhenValid()
    {
        // Arrange
        var request = new CreatePostRequestDto
        {
            Content = "Hello Test Post",
            PostType = "Text",
            Visibility = "Public",
            AllowComment = true
        };

        // Act
        var result = await _postsService.CreatePostAsync(1, request);

        // Assert
        result.Should().NotBeNull();
        result.Content.Should().Be("Hello Test Post");
        result.UserId.Should().Be(1);
        result.PostType.Should().Be("Text");
    }

    [Fact]
    public async Task CreatePost_WithMentions_SendsNotifications()
    {
        // Arrange: create a second user to be mentioned
        var mentioned = new User
        {
            Id = 2,
            UserName = "mentioned",
            Fullname = "Mentioned User",
            Email = "m@example.com",
            Delflg = false
        };
        _context.Users.Add(mentioned);
        await _context.SaveChangesAsync();

        var request = new CreatePostRequestDto
        {
            Content = "Hello with mention",
            PostType = "Text",
            Visibility = "Public",
            AllowComment = true,
            Mentions = new List<CreateCommentMentionRequestDto>
            {
                new CreateCommentMentionRequestDto { MentionedUserId = 2 }
            }
        };

        _mockNotificationService
            .Setup(n => n.CreateNotificationAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new InteractHub.Core.DTOs.Notifications.NotificationResponseDto { NotificationId = 1, RecipientId = 2 });

        // Act
        var result = await _postsService.CreatePostAsync(1, request);

        // Assert
        result.Should().NotBeNull();
        _mockRealtimeService.Verify(r => r.PushNotificationCreatedAsync(2, It.IsAny<InteractHub.Core.DTOs.Notifications.NotificationResponseDto>()), Times.Once);
    }

    [Fact]
    public async Task GetPostById_ReturnsPost_WhenExists()
    {
        // Arrange
        var post = new Post
        {
            UserId = 1,
            Content = "Test Post Content",
            PostType = "Text",
            Visibility = "Public",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _postsService.GetPostWithIdAsync(1, post.PostId);

        // Assert
        result.Should().NotBeNull();
        result!.Content.Should().Be("Test Post Content");
    }

    [Fact]
    public async Task GetPostById_ReturnsNull_WhenNotFound()
    {
        // Act
        var result = await _postsService.GetPostWithIdAsync(1, 999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePost_ReturnsTrue_WhenPostExists()
    {
        // Arrange
        var post = new Post
        {
            UserId = 1,
            Content = "Post to delete",
            PostType = "Text",
            Visibility = "Public",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // Act
        var result = await _postsService.DeletePostAsync(1, post.PostId);

        // Assert
        result.Should().BeTrue();
        var deletedPost = await _context.Posts.FindAsync(post.PostId);
        deletedPost!.Delflg.Should().BeTrue();
    }

    [Fact]
    public async Task AddComment_ReplyAndMentions_SendsNotifications()
    {
        // Arrange: create post owner (user 2) and parent comment owner (user 3)
        var user2 = new User { Id = 2, UserName = "owner", Fullname = "Owner", Email = "o@example.com", Delflg = false };
        var user3 = new User { Id = 3, UserName = "commenter", Fullname = "Commenter", Email = "c@example.com", Delflg = false };
        _context.Users.AddRange(user2, user3);
        await _context.SaveChangesAsync();

        var post = new Post { UserId = 2, Content = "Post", PostType = "Text", Visibility = "Public", RegDatetime = DateTime.UtcNow, Delflg = false };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var parentComment = new Comment { PostId = post.PostId, UserId = 3, Content = "Parent", RegDatetime = DateTime.UtcNow, Delflg = false };
        _context.Comments.Add(parentComment);
        await _context.SaveChangesAsync();

        var req = new CreateCommentRequestDto { Content = "Reply", ParentCommentId = parentComment.CommentId, Mentions = new List<CreateCommentMentionRequestDto> { new CreateCommentMentionRequestDto { MentionedUserId = 2 } } };

        _mockNotificationService
            .Setup(n => n.CreateNotificationAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new InteractHub.Core.DTOs.Notifications.NotificationResponseDto { NotificationId = 1 });

        // Act
        var res = await _postsService.AddCommentAsync(1, post.PostId, req);

        // Assert: should send to post owner and parent comment owner and mentioned user (post owner may be duplicated with mention)
        _mockRealtimeService.Verify(r => r.PushNotificationCreatedAsync(2, It.IsAny<InteractHub.Core.DTOs.Notifications.NotificationResponseDto>()), Times.AtLeastOnce);
        _mockRealtimeService.Verify(r => r.PushNotificationCreatedAsync(3, It.IsAny<InteractHub.Core.DTOs.Notifications.NotificationResponseDto>()), Times.Once);
    }

    [Fact]
    public async Task ToggleCommentReaction_SendsNotificationToCommentOwner()
    {
        // Arrange: create post and comment owner
        var user2 = new User { Id = 2, UserName = "owner", Fullname = "Owner", Email = "o@example.com", Delflg = false };
        _context.Users.Add(user2);
        await _context.SaveChangesAsync();

        var post = new Post { UserId = 1, Content = "Post", PostType = "Text", Visibility = "Public", RegDatetime = DateTime.UtcNow, Delflg = false };
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var comment = new Comment { PostId = post.PostId, UserId = 2, Content = "Comment", RegDatetime = DateTime.UtcNow, Delflg = false };
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        _mockNotificationService
            .Setup(n => n.CreateNotificationAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new InteractHub.Core.DTOs.Notifications.NotificationResponseDto { NotificationId = 1 });

        // Act
        var likeCount = await _postsService.ToggleCommentReactionAsync(1, post.PostId, comment.CommentId, "Like");

        // Assert
        likeCount.Should().BeGreaterThanOrEqualTo(0);
        _mockRealtimeService.Verify(r => r.PushNotificationCreatedAsync(2, It.IsAny<InteractHub.Core.DTOs.Notifications.NotificationResponseDto>()), Times.Once);
    }
}
