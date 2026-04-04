using InteractHub.Core.DTOs.Stories;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Services;

public class StoriesService : IStoriesService
{
    private readonly AppDbContext _context;

    public StoriesService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StoryResponseDto> CreateStoryAsync(long userId, CreateStoryRequestDto dto)
    {
        var story = new Story
        {
            UserId = userId,
            MediaUrl = dto.MediaUrl,
            MediaType = dto.MediaType,
            ThumbnailUrl = dto.ThumbnailUrl,
            Caption = dto.Caption,
            CaptionFormat = dto.CaptionFormat,
            BgColor = dto.BgColor,
            FontStyle = dto.FontStyle,
            DurationSec = dto.DurationSec,
            Visibility = dto.Visibility,
            BackgroundMusicId = dto.BackgroundMusicId,
            MusicStartSec = dto.MusicStartSec,
            MusicEndSec = dto.MusicEndSec,
            ExpireDatetime = DateTime.UtcNow.AddHours(24),
            RegDatetime = DateTime.UtcNow
        };

        _context.Stories.Add(story);
        await _context.SaveChangesAsync();

        var created = await _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .FirstAsync(s => s.StoryId == story.StoryId);

        return MapToDto(created, false, false);
    }

    public async Task<StoryResponseDto?> GetStoryByIdAsync(long storyId, long? currentUserId = null)
    {
        var story = await _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .FirstOrDefaultAsync(s => s.StoryId == storyId && !s.Delflg);

        if (story == null)
            return null;

        var hasReacted = currentUserId.HasValue && await _context.StoryReactions
            .AnyAsync(r => r.StoryId == storyId && r.UserId == currentUserId.Value && !r.Delflg);

        var hasViewed = currentUserId.HasValue && await _context.StoryViews
            .AnyAsync(v => v.StoryId == storyId && v.ViewerId == currentUserId.Value && !v.Delflg);

        return MapToDto(story, hasReacted, hasViewed);
    }

    public async Task<PagedStoriesResponseDto> GetUserStoriesAsync(long userId, long? currentUserId, int page, int pageSize)
    {
        var query = _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .Where(s => s.UserId == userId && !s.Delflg && !s.IsExpired)
            .OrderByDescending(s => s.RegDatetime);

        var totalCount = await query.CountAsync();

        var stories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = new List<StoryResponseDto>();
        foreach (var story in stories)
        {
            var hasReacted = currentUserId.HasValue && await _context.StoryReactions
                .AnyAsync(r => r.StoryId == story.StoryId && r.UserId == currentUserId.Value && !r.Delflg);

            var hasViewed = currentUserId.HasValue && await _context.StoryViews
                .AnyAsync(v => v.StoryId == story.StoryId && v.ViewerId == currentUserId.Value && !v.Delflg);

            dtos.Add(MapToDto(story, hasReacted, hasViewed));
        }

        return new PagedStoriesResponseDto
        {
            Stories = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedStoriesResponseDto> GetFriendsStoriesAsync(long userId, int page, int pageSize)
    {
        var friendIds = await _context.Friendships
            .Where(f =>
                ((f.RequesterId == userId || f.AddresseeId == userId) && f.Delflg == false) &&
                (f.Status == "Accepted"))
            .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
            .ToListAsync();

        friendIds.Add(userId);

        var query = _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .Where(s => friendIds.Contains(s.UserId) && !s.Delflg && !s.IsExpired)
            .OrderByDescending(s => s.RegDatetime);

        var totalCount = await query.CountAsync();

        var stories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = new List<StoryResponseDto>();
        foreach (var story in stories)
        {
            var hasReacted = await _context.StoryReactions
                .AnyAsync(r => r.StoryId == story.StoryId && r.UserId == userId && !r.Delflg);
            var hasViewed = await _context.StoryViews
                .AnyAsync(v => v.StoryId == story.StoryId && v.ViewerId == userId && !v.Delflg);
            dtos.Add(MapToDto(story, hasReacted, hasViewed));
        }

        return new PagedStoriesResponseDto
        {
            Stories = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PagedStoriesResponseDto> GetFeedStoriesAsync(long userId, int page, int pageSize)
    {
        var friendIds = await _context.Friendships
            .Where(f =>
                ((f.RequesterId == userId || f.AddresseeId == userId) && f.Delflg == false) &&
                (f.Status == "Accepted"))
            .Select(f => f.RequesterId == userId ? f.AddresseeId : f.RequesterId)
            .ToListAsync();

        friendIds.Add(userId);

        var query = _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .Where(s => friendIds.Contains(s.UserId) && !s.Delflg && !s.IsExpired)
            .OrderByDescending(s => s.RegDatetime);

        var totalCount = await query.CountAsync();

        var stories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = new List<StoryResponseDto>();
        foreach (var story in stories)
        {
            var hasReacted = await _context.StoryReactions
                .AnyAsync(r => r.StoryId == story.StoryId && r.UserId == userId && !r.Delflg);
            var hasViewed = await _context.StoryViews
                .AnyAsync(v => v.StoryId == story.StoryId && v.ViewerId == userId && !v.Delflg);
            dtos.Add(MapToDto(story, hasReacted, hasViewed));
        }

        return new PagedStoriesResponseDto
        {
            Stories = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<StoryResponseDto?> UpdateStoryAsync(long storyId, long userId, UpdateStoryRequestDto dto)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && s.UserId == userId && !s.Delflg);

        if (story == null)
            return null;

        if (dto.MediaUrl != null) story.MediaUrl = dto.MediaUrl;
        if (dto.ThumbnailUrl != null) story.ThumbnailUrl = dto.ThumbnailUrl;
        if (dto.Caption != null) story.Caption = dto.Caption;
        if (dto.CaptionFormat != null) story.CaptionFormat = dto.CaptionFormat;
        if (dto.BgColor != null) story.BgColor = dto.BgColor;
        if (dto.FontStyle != null) story.FontStyle = dto.FontStyle;
        if (dto.DurationSec.HasValue) story.DurationSec = dto.DurationSec.Value;
        if (dto.Visibility != null) story.Visibility = dto.Visibility;
        if (dto.BackgroundMusicId.HasValue) story.BackgroundMusicId = dto.BackgroundMusicId;
        if (dto.MusicStartSec.HasValue) story.MusicStartSec = dto.MusicStartSec.Value;
        if (dto.MusicEndSec.HasValue) story.MusicEndSec = dto.MusicEndSec.Value;
        if (dto.IsHighlighted.HasValue) story.IsHighlighted = dto.IsHighlighted.Value;
        if (dto.HighlightName != null) story.HighlightName = dto.HighlightName;

        await _context.SaveChangesAsync();

        return await GetStoryByIdAsync(story.StoryId, userId);
    }

    public async Task<bool> DeleteStoryAsync(long storyId, long userId)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && s.UserId == userId && !s.Delflg);

        if (story == null)
            return false;

        story.Delflg = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkStoryAsViewedAsync(long storyId, long viewerId, int? viewDuration = null)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && !s.Delflg);

        if (story == null)
            return false;

        if (story.UserId == viewerId)
            return true;

        var existingView = await _context.StoryViews
            .FirstOrDefaultAsync(v => v.StoryId == storyId && v.ViewerId == viewerId && !v.Delflg);

        if (existingView != null)
        {
            existingView.ViewDuration = viewDuration;
            existingView.RegDatetime = DateTime.UtcNow;
        }
        else
        {
            _context.StoryViews.Add(new StoryView
            {
                StoryId = storyId,
                ViewerId = viewerId,
                ViewDuration = viewDuration,
                RegDatetime = DateTime.UtcNow
            });

            story.ViewCount++;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<StoryReactionResponseDto?> AddReactionAsync(long storyId, long userId, AddStoryReactionRequestDto dto)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && !s.Delflg);

        if (story == null)
            return null;

        var existing = await _context.StoryReactions
            .FirstOrDefaultAsync(r => r.StoryId == storyId && r.UserId == userId && !r.Delflg);

        if (existing != null)
        {
            existing.ReactionType = dto.ReactionType;
            existing.RegDatetime = DateTime.UtcNow;
        }
        else
        {
            _context.StoryReactions.Add(new StoryReaction
            {
                StoryId = storyId,
                UserId = userId,
                ReactionType = dto.ReactionType,
                RegDatetime = DateTime.UtcNow
            });
            story.ReactionCount++;
        }

        await _context.SaveChangesAsync();

        var reaction = await _context.StoryReactions
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.StoryId == storyId && r.UserId == userId && !r.Delflg);

        if (reaction == null)
            return null;

        return new StoryReactionResponseDto
        {
            ReactionId = reaction.ReactionId,
            StoryId = reaction.StoryId,
            UserId = reaction.UserId,
            Username = reaction.User?.UserName,
            Fullname = reaction.User?.Fullname,
            AvatarUrl = reaction.User?.AvatarUrl,
            ReactionType = reaction.ReactionType,
            RegDatetime = reaction.RegDatetime
        };
    }

    public async Task<bool> RemoveReactionAsync(long storyId, long userId)
    {
        var reaction = await _context.StoryReactions
            .FirstOrDefaultAsync(r => r.StoryId == storyId && r.UserId == userId && !r.Delflg);

        if (reaction == null)
            return false;

        var story = await _context.Stories.FindAsync(storyId);
        if (story != null && story.ReactionCount > 0)
            story.ReactionCount--;

        reaction.Delflg = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<StoryViewerResponseDto>> GetStoryViewersAsync(long storyId, long userId)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && s.UserId == userId && !s.Delflg);

        if (story == null)
            return new List<StoryViewerResponseDto>();

        var viewers = await _context.StoryViews
            .Include(v => v.Viewer)
            .Where(v => v.StoryId == storyId && !v.Delflg)
            .OrderByDescending(v => v.RegDatetime)
            .ToListAsync();

        return viewers.Select(v => new StoryViewerResponseDto
        {
            ViewerId = v.ViewerId,
            Username = v.Viewer?.UserName,
            Fullname = v.Viewer?.Fullname,
            AvatarUrl = v.Viewer?.AvatarUrl,
            ViewDuration = v.ViewDuration,
            RegDatetime = v.RegDatetime
        }).ToList();
    }

    public async Task<List<StoryReactionResponseDto>> GetStoryReactionsAsync(long storyId, long userId)
    {
        var story = await _context.Stories
            .FirstOrDefaultAsync(s => s.StoryId == storyId && s.UserId == userId && !s.Delflg);

        if (story == null)
            return new List<StoryReactionResponseDto>();

        var reactions = await _context.StoryReactions
            .Include(r => r.User)
            .Where(r => r.StoryId == storyId && !r.Delflg)
            .OrderByDescending(r => r.RegDatetime)
            .ToListAsync();

        return reactions.Select(r => new StoryReactionResponseDto
        {
            ReactionId = r.ReactionId,
            StoryId = r.StoryId,
            UserId = r.UserId,
            Username = r.User?.UserName,
            Fullname = r.User?.Fullname,
            AvatarUrl = r.User?.AvatarUrl,
            ReactionType = r.ReactionType,
            RegDatetime = r.RegDatetime
        }).ToList();
    }

    public async Task<List<StoryResponseDto>> GetHighlightsAsync(long userId)
    {
        var stories = await _context.Stories
            .Include(s => s.User)
            .Include(s => s.BackgroundMusic)
            .Where(s => s.UserId == userId && !s.Delflg && s.IsHighlighted)
            .OrderByDescending(s => s.RegDatetime)
            .ToListAsync();

        return stories.Select(s => MapToDto(s, false, false)).ToList();
    }

    private StoryResponseDto MapToDto(Story story, bool hasReacted, bool hasViewed)
    {
        return new StoryResponseDto
        {
            StoryId = story.StoryId,
            UserId = story.UserId,
            Username = story.User?.UserName,
            Fullname = story.User?.Fullname,
            AvatarUrl = story.User?.AvatarUrl,
            MediaUrl = story.MediaUrl,
            MediaType = story.MediaType,
            ThumbnailUrl = story.ThumbnailUrl,
            Caption = story.Caption,
            CaptionFormat = story.CaptionFormat,
            BgColor = story.BgColor,
            FontStyle = story.FontStyle,
            DurationSec = story.DurationSec,
            Visibility = story.Visibility,
            BackgroundMusicId = story.BackgroundMusicId,
            BackgroundMusicTitle = story.BackgroundMusic?.Title,
            MusicStartSec = story.MusicStartSec,
            MusicEndSec = story.MusicEndSec,
            ViewCount = story.ViewCount,
            ReactionCount = story.ReactionCount,
            ExpireDatetime = story.ExpireDatetime,
            IsExpired = story.IsExpired,
            IsHighlighted = story.IsHighlighted,
            HighlightName = story.HighlightName,
            RegDatetime = story.RegDatetime,
            HasReacted = hasReacted,
            HasViewed = hasViewed
        };
    }
}
