using FluentAssertions;
using InteractHub.Core.DTOs.Stories;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using InteractHub.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InteractHub.Tests.Services;

public class StoriesServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<INotificationsService> _mockNotificationService;
    private readonly Mock<INotificationRealtimeService> _mockRealtimeService;
    private readonly StoriesService _storiesService;

    public StoriesServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        _mockNotificationService = new Mock<INotificationsService>();
        _mockRealtimeService = new Mock<INotificationRealtimeService>();

        _storiesService = new StoriesService(_context, _mockNotificationService.Object, _mockRealtimeService.Object);

        Seed();
    }

    private void Seed()
    {
        _context.Users.Add(new User { Id = 1, UserName = "u1", Fullname = "User1", Email = "u1@example.com", Delflg = false });
        _context.Users.Add(new User { Id = 2, UserName = "u2", Fullname = "User2", Email = "u2@example.com", Delflg = false });
        _context.SaveChanges();
    }

    [Fact]
    public async Task AddReaction_NewReaction_SendsNotificationToStoryOwner()
    {
        // Arrange: create a story owned by user 2
        var story = new Story { UserId = 2, MediaUrl = "m.jpg", MediaType = "Image", RegDatetime = DateTime.UtcNow, Delflg = false };
        _context.Stories.Add(story);
        await _context.SaveChangesAsync();

        _mockNotificationService
            .Setup(n => n.CreateNotificationAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<long?>(), It.IsAny<long?>(), It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(new InteractHub.Core.DTOs.Notifications.NotificationResponseDto { NotificationId = 1 });

        // Act: user 1 reacts to story
        var dto = new AddStoryReactionRequestDto { ReactionType = "Like" };
        var res = await _storiesService.AddReactionAsync(story.StoryId, 1, dto);

        // Assert
        res.Should().NotBeNull();
        _mockRealtimeService.Verify(r => r.PushNotificationCreatedAsync(2, It.IsAny<InteractHub.Core.DTOs.Notifications.NotificationResponseDto>()), Times.Once);
    }
}
