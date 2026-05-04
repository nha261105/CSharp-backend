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
}
