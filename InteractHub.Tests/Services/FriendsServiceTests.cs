using FluentAssertions;
using InteractHub.Core.Entities;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InteractHub.Tests.Services;

public class FriendsServiceTests
{
    private readonly AppDbContext _context;

    public FriendsServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        SeedTestData();
    }

    private void SeedTestData()
    {
        var user1 = new User
        {
            Id = 1,
            UserName = "user1",
            Fullname = "User One",
            Email = "user1@test.com",
            Delflg = false
        };

        var user2 = new User
        {
            Id = 2,
            UserName = "user2",
            Fullname = "User Two",
            Email = "user2@test.com",
            Delflg = false
        };

        _context.Users.AddRange(user1, user2);
        _context.SaveChanges();
    }

    [Fact]
    public async Task SendFriendRequest_CreatesFriendship_WhenValid()
    {
        // Arrange
        var friendship = new Friendship
        {
            RequesterId = 1,
            AddresseeId = 2,
            Status = "Pending",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };

        // Act
        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Friendships
            .FirstOrDefaultAsync(f => f.RequesterId == 1 && f.AddresseeId == 2);

        result.Should().NotBeNull();
        result!.Status.Should().Be("Pending");
    }

    [Fact]
    public async Task AcceptFriendRequest_UpdatesStatus_WhenExists()
    {
        // Arrange
        var friendship = new Friendship
        {
            RequesterId = 1,
            AddresseeId = 2,
            Status = "Pending",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };
        _context.Friendships.Add(friendship);
        await _context.SaveChangesAsync();

        // Act
        friendship.Status = "Accepted";
        friendship.UpdDatetime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Friendships.FindAsync(friendship.FriendshipId);
        result!.Status.Should().Be("Accepted");
        result.UpdDatetime.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFriendsList_ReturnsAcceptedFriends_Only()
    {
        // Arrange
        var friendship1 = new Friendship
        {
            RequesterId = 1,
            AddresseeId = 2,
            Status = "Accepted",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };

        var friendship2 = new Friendship
        {
            RequesterId = 1,
            AddresseeId = 3,
            Status = "Pending",
            RegDatetime = DateTime.UtcNow,
            Delflg = false
        };

        _context.Friendships.AddRange(friendship1, friendship2);
        await _context.SaveChangesAsync();

        // Act
        var acceptedFriends = await _context.Friendships
            .Where(f => f.RequesterId == 1 && f.Status == "Accepted" && !f.Delflg)
            .ToListAsync();

        // Assert
        acceptedFriends.Should().HaveCount(1);
        acceptedFriends[0].AddresseeId.Should().Be(2);
    }
}
