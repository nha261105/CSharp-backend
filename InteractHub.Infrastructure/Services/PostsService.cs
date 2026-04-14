using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql.PostgresTypes;


namespace InteractHub.Infrastructure.Services;

public class PostsService : IPostService
{

    private readonly AppDbContext _context;

    public PostsService(AppDbContext context)
    {
        _context = context;
    }

    public PostResponseDto MaptoDto(Post p, User u)
    {
        return new PostResponseDto
        {
            PostId = p.PostId,
            UserId = u.Id,
            UserName = u.UserName ?? string.Empty,
            AvatarUrl = u.AvatarUrl ?? string.Empty,
            Content = p.Content ?? string.Empty,
            PostType = p.PostType,
            Visibility = p.Visibility,
            LikeCount = p.LikeCount,
            CommentCount = p.CommentCount,
            ShareCount = p.ShareCount,
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
            OriginalPost = p.OriginalPost != null && p.OriginalPost.User != null
                ? MaptoDto(p.OriginalPost, p.OriginalPost.User)
                : null
        };
    }
    public async Task<List<PostResponseDto>> GetListPostPageAsync(long? currentUserId, int page, int pageSize)
    {
        var query = await _context.Posts
            .AsNoTracking()
            .Include(p => p.User)
            .Include(p => p.PostMedias)
            .Include(p => p.OriginalPost)
                .ThenInclude(p => p!.User)
            .Where(p => !p.Delflg)
            .OrderByDescending(p => p.RegDatetime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var posts = new List<PostResponseDto>();

        foreach (var post in query)
        {
            posts.Add(MaptoDto(post, post.User));
        }

        return posts;

    }

    public async Task<List<PostResponseDto>> GetListUserPagePostAsync(long? currentUserId, long targetUserId, int page, int pageSize)
    {
        var query = await _context.Posts
                    .AsNoTracking()
                    .Include(p => p.User)
                    .Include(p => p.PostMedias)
                    .Include(p => p.OriginalPost)
                        .ThenInclude(op => op!.User)
                    .Where(p => p.User.Id == targetUserId && !p.Delflg)
                    .OrderByDescending(p => p.RegDatetime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        var current_user_posts = new List<PostResponseDto>();

        foreach(var current in query) {
            current_user_posts.Add(MaptoDto(current,current.User));
        }
        return current_user_posts;
    }

    public async Task<PostResponseDto> GetPostWithIdAsync(long? currentUserId, long postId)
    {
        
        throw new NotImplementedException();
    }

    public async Task<PostResponseDto> CreatePostAsync(long currentUserId, CreatePostRequestDto req)
    {
        throw new NotImplementedException();
    }

    public async Task<PostResponseDto> UpdatePostAsnc(long currentUserId, long PostId, UpdatePostRequestDto req)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeletePostAsync(long currentUserId, long PostId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> LikePostAsync(long currentUserId, long postId)
    {
        throw new NotImplementedException();
    }
    public async Task<int> UnLikePostAsync(long currentUserId, long postId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SharePostAsync(long currentUserId, long postId)
    {
        throw new NotImplementedException();
    }
}