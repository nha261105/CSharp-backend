using InteractHub.Core.DTOs.Posts;
using InteractHub.Core.Entities;
using InteractHub.Core.Interfaces.Services;
using InteractHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace InteractHub.Infrastructure.Services;

public class PostsService : IPostService
{

    private readonly AppDbContext _context;
    private readonly INotificationsService _notificationsService;
    private readonly INotificationRealtimeService _notificationRealtimeService;

    public PostsService(
        AppDbContext context,
        INotificationsService notificationsService,
        INotificationRealtimeService notificationRealtimeService)
    {
        _context = context;
        _notificationsService = notificationsService;
        _notificationRealtimeService = notificationRealtimeService;
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
        if (includeOriginal && p.OriginalPostId.HasValue)
        {
            var originalPost = await BuildPostGraphQuery(asNoTracking: true)
                .FirstOrDefaultAsync(op => op.PostId == p.OriginalPostId.Value);
                
            if (originalPost != null)
            {
                originalPostDto = await MapToDtoAsync(originalPost, currentUserId, includeOriginal: false);
            }
        }

        return new PostResponseDto
        {
            PostId = p.PostId,
            UserId = p.User.Id,
            UserName = p.User.UserName ?? string.Empty,
            FullName = p.User.Fullname ?? string.Empty, 
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
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
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

            if (post.UserId != currentUserId)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: post.UserId,
                    notificationType: "PostLiked",
                    senderId: currentUserId,
                    referenceId: post.PostId,
                    referenceType: "Post",
                    message: "đã thích bài viết của bạn",
                    redirectUrl: $"/posts/{post.PostId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(post.UserId, notification);
            }

            return post.LikeCount;
        });
    }

    public async Task<int> UnLikePostAsync(long currentUserId, long postId)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
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
        });
    }

    public async Task<int> SharePostAsync(long currentUserId, long postId)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
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

            if (post.UserId != currentUserId)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: post.UserId,
                    notificationType: "PostShared",
                    senderId: currentUserId,
                    referenceId: post.PostId,
                    referenceType: "Post",
                    message: "đã chia sẻ bài viết của bạn",
                    redirectUrl: $"/posts/{post.PostId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(post.UserId, notification);
            }

            return post.ShareCount;
        });
    }
    public async Task<CommentResponseDto?> AddCommentAsync(long currentUserId, long postId, CreateCommentRequestDto req)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try 
        {
            var comment = new Comment 
            {
                PostId = postId,
                UserId = currentUserId,
                Content = req.Content,
                ContentFormat = req.ContentFormat,
                ImageUrl = req.ImageUrl,
                ParentCommentId = req.ParentCommentId > 0 ? req.ParentCommentId : null,
                LikeCount = 0,
                ReplyCount = 0,
                IsEdited = false,
                IsReported = false,
                Delflg = false,
                RegDatetime = DateTime.UtcNow,
                UpdDatetime = null
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var mentionDtos = new List<CommentMentionResponseDto>();
            if (req.Mentions != null && req.Mentions.Any()) 
            {
                foreach (var m in req.Mentions)
                {
                    var mention = new CommentMention 
                    {
                        CommentId = comment.CommentId,
                        MentionedUserId = m.MentionedUserId,
                        StartPos = m.StartPos ?? 0, 
                        EndPos = m.EndPos ?? 0,
                        RegDatetime = DateTime.UtcNow,
                        Delflg = false
                    };
                    _context.CommentMentions.Add(mention);

                    var mentionedUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == m.MentionedUserId);
                    mentionDtos.Add(new CommentMentionResponseDto
                    {
                        MentionedUserId = m.MentionedUserId,
                        FullName = mentionedUser?.Fullname ?? string.Empty,
                        StartPos = m.StartPos ?? 0,
                        EndPos = m.EndPos ?? 0
                    });
                }
            }

            var post = await _context.Posts.FindAsync(postId);
            if (post != null) 
            {
                post.CommentCount++;
                post.UpdDatetime = DateTime.UtcNow;
            }

            if (req.ParentCommentId.HasValue) 
            {
                var parent = await _context.Comments.FindAsync(req.ParentCommentId.Value);
                if (parent != null) 
                {
                    parent.ReplyCount++;
                    parent.UpdDatetime = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            var currentUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == currentUserId);

            return new CommentResponseDto
            {
                CommentId = comment.CommentId,
                PostId = comment.PostId,
                UserId = currentUserId,
                FullName = currentUser?.Fullname ?? string.Empty,
                AvatarUrl = currentUser?.AvatarUrl ?? string.Empty,
                Content = comment.Content,
                ContentFormat = comment.ContentFormat,
                ImageUrl = comment.ImageUrl,
                ParentCommentId = comment.ParentCommentId,
                LikeCount = 0,
                ReplyCount = 0,
                IsEdited = false,
                RegDatetime = comment.RegDatetime,
                Mentions = mentionDtos
            };
            }
            catch 
            {
                await transaction.RollbackAsync();
                return null;
            }
        }

    public async Task<int> ToggleCommentReactionAsync(long currentUserId, long commentId, string reactionType)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId && !c.Delflg);
        if (comment == null) return -1;

        var existing = await _context.CommentLikes
            .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == currentUserId);

        if (existing != null) 
        {
            if (existing.ReactionType == reactionType) 
            {
                _context.CommentLikes.Remove(existing);
                comment.LikeCount = Math.Max(0, comment.LikeCount - 1);
            } 
            else 
            {
                existing.ReactionType = reactionType;
                existing.RegDatetime = DateTime.UtcNow;
            }
        } 
        else 
        {
            _context.CommentLikes.Add(new CommentLike 
            {
                CommentId = commentId,
                UserId = currentUserId,
                ReactionType = reactionType,
                RegDatetime = DateTime.UtcNow,
                Delflg = false
            });
            comment.LikeCount++;
        }
        
        await _context.SaveChangesAsync();
        return comment.LikeCount;
    }

    public async Task<List<CommentReactionDetailResponseDto>> GetCommentReactionsDetailAsync(long commentId)
    {
        return await _context.CommentLikes
            .Where(l => l.CommentId == commentId && !l.Delflg)
            .Include(l => l.User)
            .GroupBy(l => l.ReactionType)
            .Select(g => new CommentReactionDetailResponseDto 
            {
                ReactionType = g.Key,
                Count = g.Count(),
                Users = g.Select(u => new UserSummaryResponseDto 
                {
                    Id = u.UserId,
                    FullName = u.User.Fullname,
                    AvatarUrl = u.User.AvatarUrl ?? "",
                    UserName = u.User.UserName ?? ""
                }).ToList()
            })
            .ToListAsync();
    }
    public async Task<List<CommentResponseDto>> GetPostCommentsAsync(long postId, int page, int pageSize)
    {
    var comments = await _context.Comments
        .Where(c => c.PostId == postId && !c.Delflg)
        .Include(c => c.User)
        .Include(c => c.CommentMentions)
            .ThenInclude(m => m.MentionedUser)
        .OrderByDescending(c => c.RegDatetime)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .AsNoTracking()
        .ToListAsync();

    return comments.Select(c => new CommentResponseDto
    {
        CommentId = c.CommentId,
        PostId = c.PostId,
        UserId = c.UserId,
        FullName = c.User.Fullname ?? string.Empty,
        AvatarUrl = c.User.AvatarUrl ?? string.Empty,
        Content = c.Content,
        LikeCount = c.LikeCount,
        ReplyCount = c.ReplyCount,
        RegDatetime = c.RegDatetime,
        Mentions = c.CommentMentions.Select(m => new CommentMentionResponseDto
        {
            MentionedUserId = m.MentionedUserId,
            FullName = m.MentionedUser?.Fullname ?? string.Empty,
            StartPos = m.StartPos ?? 0, 
            EndPos = m.EndPos ?? 0     
        }).ToList()
    }).ToList();
    }
    public async Task<List<PostReactionDetailResponseDto>> GetPostReactionsDetailAsync(long postId, int page, int pageSize)
    {
    return await _context.PostLikes
        .Where(l => l.PostId == postId && !l.Delflg)
        .Include(l => l.User)
        .GroupBy(l => l.ReactionType)
        .Select(g => new PostReactionDetailResponseDto 
        {
            ReactionType = g.Key ?? "Like",
            Count = g.Count(),
            Users = g.OrderByDescending(u => u.RegDatetime)
                     .Skip((page - 1) * pageSize)
                     .Take(pageSize)
                     .Select(u => new UserSummaryResponseDto 
                     {
                         Id = u.UserId,
                         FullName = u.User.Fullname ?? string.Empty,
                         AvatarUrl = u.User.AvatarUrl ?? string.Empty,
                         UserName = u.User.UserName ?? string.Empty
                     }).ToList()
        })
        .ToListAsync();
    }
}