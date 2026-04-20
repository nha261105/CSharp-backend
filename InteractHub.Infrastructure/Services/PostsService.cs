using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace InteractHub.Infrastructure.Services;

public class PostsService : IPostService
{

    private readonly AppDbContext _context;

    public PostsService(AppDbContext context)
    {
        _context = context;
    }

    private async Task<PostResponseDto> MapToDtoAsync(Post p, long? currentUserId, bool includeOriginal = true)
    {
        var isLikeByMe = false;
        if (currentUserId.HasValue)
        {
            isLikeByMe = await _context.PostLikes
                .AsNoTracking()
                .AnyAsync(l => l.PostId == p.PostId && l.UserId == currentUserId.Value && !l.Delflg);
        }

        var originalPostDto = (PostResponseDto?)null;
        if (includeOriginal && p.OriginalPost != null && p.OriginalPost.User != null)
        {
            originalPostDto = await MapToDtoAsync(p.OriginalPost, currentUserId, includeOriginal: false);
        }

        return new PostResponseDto
        {
            PostId = p.PostId,
            UserId = p.User.Id,
            UserName = p.User.UserName ?? string.Empty,
            AvatarUrl = p.User.AvatarUrl ?? string.Empty,
            Content = p.Content ?? string.Empty,
            PostType = p.PostType,
            Visibility = p.Visibility,
            LikeCount = p.LikeCount,
            CommentCount = p.CommentCount,
            ShareCount = p.ShareCount,
            IsLikeByMe = isLikeByMe,
            IsPinned = p.IsPinned,
            AllowComment = p.AllowComment,
            LocationName = p.LocationName,
            RegDateTime = p.RegDatetime,
            BackgroundMusicId = p.BackgroundMusicId,
            Medias = p.PostMedias
                .OrderBy(media => media.SortOrder)
                .Select(media => new PostMediaResponseDto
                {
                    MediaUrl = media.MediaUrl,
                    MediaType = media.MediaType,
                    ThumbnailUrl = media.ThumbnailUrl
                })
                .ToList(),
            OriginalPost = originalPostDto
        };
    }

    private IQueryable<Post> BuildPostGraphQuery(bool asNoTracking)
    {
        var query = _context.Posts
            .Include(p => p.User)
            .Include(p => p.PostMedias)
            .Include(p => p.OriginalPost)
                .ThenInclude(op => op!.User);

        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<List<PostResponseDto>> GetListPostPageAsync(long? currentUserId, int page, int pageSize)
    {
        var posts = await BuildPostGraphQuery(asNoTracking: true)
            .Where(p => !p.Delflg)
            .OrderByDescending(p => p.RegDatetime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new List<PostResponseDto>();
        foreach (var post in posts)
        {
            result.Add(await MapToDtoAsync(post, currentUserId));
        }

        return result;

    }

    public async Task<List<PostResponseDto>> GetListUserPagePostAsync(long? currentUserId, long targetUserId, int page, int pageSize)
    {
        var posts = await BuildPostGraphQuery(asNoTracking: true)
            .Where(p => p.UserId == targetUserId && !p.Delflg)
            .OrderByDescending(p => p.RegDatetime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new List<PostResponseDto>();
        foreach (var post in posts)
        {
            result.Add(await MapToDtoAsync(post, currentUserId));
        }

        return result;
    }

    public async Task<PostResponseDto?> GetPostWithIdAsync(long? currentUserId, long postId)
    {
        var post = await BuildPostGraphQuery(asNoTracking: true)
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

        if (post == null)
        {
            return null;
        }

        return await MapToDtoAsync(post, currentUserId);
    }

    public async Task<PostResponseDto> CreatePostAsync(long currentUserId, CreatePostRequestDto req)
    {
        var post = new Post
        {
            UserId = currentUserId,
            PostType = req.PostType,
            Visibility = req.Visibility,
            MusicStartSec = req.MusicStartSec,
            IsPinned = req.IsPinned,
            AllowComment = req.AllowComment,
            Content = req.Content,
            ContentFormat = req.ContentFormat,
            LocationName = req.LocationName,
            LocationLat = req.LocationLat,
            LocationLng = req.LocationLng,
            Feeling = req.Feeling,
            OriginalPostId = req.OriginalPostId,
            BackgroundMusicId = req.BackgroundMusicId,
            MusicEndSec = req.MusicEndSec,
            RegDatetime = DateTime.UtcNow,
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var createdPost = await BuildPostGraphQuery(asNoTracking: true)
            .FirstAsync(p => p.PostId == post.PostId);

        return await MapToDtoAsync(createdPost, currentUserId);
    }

    public async Task<PostResponseDto?> UpdatePostAsnc(long currentUserId, long PostId, UpdatePostRequestDto req)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == PostId && p.UserId == currentUserId && !p.Delflg);

        if (post == null)
        {
            return null;
        }

        post.Content = req.Content;
        post.ContentFormat = req.ContentFormat;
        post.Visibility = req.Visibility ?? post.Visibility;
        post.LocationName = req.LocationName;
        post.IsEdited = true;
        post.UpdDatetime = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedPost = await BuildPostGraphQuery(asNoTracking: true)
            .FirstAsync(p => p.PostId == post.PostId);

        return await MapToDtoAsync(updatedPost, currentUserId);
    }

    public async Task<bool> DeletePostAsync(long currentUserId, long PostId)
    {
        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.UserId == currentUserId && p.PostId == PostId && !p.Delflg);

        if (post == null)
        {
            return false;
        }

        post.Delflg = true;
        post.UpdDatetime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return true;

    }

    public async Task<int> LikePostAsync(long currentUserId, long postId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

        if (post == null)
        {
            return -1;
        }

        var existingLike = await _context.PostLikes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == currentUserId);

        if (existingLike != null)
        {
            if (!existingLike.Delflg)
            {
                return post.LikeCount;
            }

            existingLike.Delflg = false;
            existingLike.RegDatetime = DateTime.UtcNow;
        }
        else
        {
            _context.PostLikes.Add(new PostLike
            {
                PostId = postId,
                UserId = currentUserId,
                ReactionType = "Like",
                RegDatetime = DateTime.UtcNow,
            });
        }

        post.LikeCount += 1;
        post.UpdDatetime = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return post.LikeCount;
    }

    public async Task<int> UnLikePostAsync(long currentUserId, long postId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

        if (post == null)
        {
            return -1;
        }

        var existingLike = await _context.PostLikes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == currentUserId && !l.Delflg);

        if (existingLike == null)
        {
            return post.LikeCount;
        }

        existingLike.Delflg = true;
        if (post.LikeCount > 0)
        {
            post.LikeCount -= 1;
        }
        post.UpdDatetime = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return post.LikeCount;
    }

    public async Task<int> SharePostAsync(long currentUserId, long postId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var post = await _context.Posts
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

        if (post == null)
        {
            return -1;
        }

        _context.PostShares.Add(new PostShare
        {
            PostId = postId,
            UserId = currentUserId,
            Visibility = post.Visibility,
            RegDatetime = DateTime.UtcNow,
        });

        post.ShareCount += 1;
        post.UpdDatetime = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return post.ShareCount;
    }
}