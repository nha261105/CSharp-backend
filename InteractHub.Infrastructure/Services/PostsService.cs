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
        string? myReactionType = null;
        if (currentUserId.HasValue)
        {
            var myLike = await _context.PostLikes
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.PostId == p.PostId && l.UserId == currentUserId.Value && !l.Delflg);
            if (myLike != null)
            {
                isLikeByMe = true;
                myReactionType = myLike.ReactionType;
            }
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
            Feeling = p.Feeling,
            LikeCount = p.LikeCount,
            CommentCount = p.CommentCount,
            ShareCount = p.ShareCount,
            IsLikeByMe = isLikeByMe,
            MyReactionType = myReactionType,
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
            OriginalPost = originalPostDto,
            Mentions = p.PostMentions
                .Where(m => !m.Delflg)
                .Select(m => new PostMentionResponseDto
                {
                    MentionedUserId = m.MentionedUserId,
                    FullName = m.MentionedUser.Fullname ?? string.Empty,
                    AvatarUrl = m.MentionedUser.AvatarUrl,
                    StartPos = m.StartPos,
                    EndPos = m.EndPos
                })
                .ToList()
        };
    }

    private IQueryable<Post> BuildPostGraphQuery(bool asNoTracking)
    {
        var query = _context.Posts
            .Include(p => p.User)
            .Include(p => p.PostMedias)
            .Include(p => p.PostMentions)
                .ThenInclude(m => m.MentionedUser)
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

    public async Task<PostDetailResponseDto?> GetPostDetailAsync(long? currentUserId, long postId)
    {
        const int commentsPageSize = 10;
        const int reactionsUsersLimit = 5;

        // ── [1] Load bài viết cùng User, Medias, OriginalPost ────────────────
        var post = await BuildPostGraphQuery(asNoTracking: true)
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

        if (post == null) return null;

        // ── [2] Kiểm tra user hiện tại đã like chưa ──────────────────────────
        var isLikedByMe = false;
        string? myReactionType = null;
        if (currentUserId.HasValue)
        {
            var myLike = await _context.PostLikes
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == currentUserId.Value && !l.Delflg);
            if (myLike != null)
            {
                isLikedByMe = true;
                myReactionType = myLike.ReactionType;
            }
        }

        // ── [3] OriginalPost (tái dụng MapToDtoAsync, không đệ quy) ──────────
        PostResponseDto? originalPostDto = null;
        if (post.OriginalPostId.HasValue)
        {
            var originalPost = await BuildPostGraphQuery(asNoTracking: true)
                .FirstOrDefaultAsync(op => op.PostId == post.OriginalPostId.Value && !op.Delflg);

            if (originalPost != null)
            {
                originalPostDto = await MapToDtoAsync(originalPost, currentUserId, includeOriginal: false);
            }
        }

        // ── [4] Reactions bài viết — load tất cả, group in-memory ────────────
        // Lý do load hết: cần Count() chính xác per type + top N users cùng lúc.
        // Với project student scale này là chấp nhận được.
        var allPostLikes = await _context.PostLikes
            .Where(l => l.PostId == postId && !l.Delflg)
            .Include(l => l.User)
            .AsNoTracking()
            .ToListAsync();

        var postReactionDtos = allPostLikes
            .GroupBy(l => l.ReactionType)
            .Select(g => new PostReactionDetailResponseDto
            {
                ReactionType = g.Key ?? "Like",
                Count = g.Count(),
                Users = g.OrderByDescending(u => u.RegDatetime)
                         .Take(reactionsUsersLimit)
                         .Select(u => new UserSummaryResponseDto
                         {
                             Id = u.UserId,
                             FullName = u.User.Fullname ?? string.Empty,
                             AvatarUrl = u.User.AvatarUrl ?? string.Empty,
                             UserName = u.User.UserName
                         }).ToList()
            }).ToList();

        // ── [5] Comments trang 1 ─────────────────────────────────────────────
        var comments = await _context.Comments
            .Where(c => c.PostId == postId && !c.Delflg)
            .Include(c => c.User)
            .Include(c => c.CommentMentions)
                .ThenInclude(m => m.MentionedUser)
            .OrderByDescending(c => c.RegDatetime)
            .Take(commentsPageSize)
            .AsNoTracking()
            .ToListAsync();

        // ── [6] Batch load reactions cho tất cả comments (1 query duy nhất) ──
        var commentIds = comments.Select(c => c.CommentId).ToList();

        var allCommentLikes = await _context.CommentLikes
            .Where(l => commentIds.Contains(l.CommentId) && !l.Delflg)
            .Include(l => l.User)
            .AsNoTracking()
            .ToListAsync();

        // Group: CommentId → List<CommentReactionDetailResponseDto>
        var reactionsByCommentId = allCommentLikes
            .GroupBy(l => l.CommentId)
            .ToDictionary(
                g => g.Key,
                g => g.GroupBy(l => l.ReactionType)
                      .Select(rg => new CommentReactionDetailResponseDto
                      {
                          ReactionType = rg.Key,
                          Count = rg.Count(),
                          Users = rg.Take(reactionsUsersLimit)
                                    .Select(u => new UserSummaryResponseDto
                                    {
                                        Id = u.UserId,
                                        FullName = u.User.Fullname ?? string.Empty,
                                        AvatarUrl = u.User.AvatarUrl ?? string.Empty,
                                        UserName = u.User.UserName
                                    }).ToList()
                      }).ToList()
            );

        // ── [7] Map comments → CommentWithReactionsDto ───────────────────────
        var commentDtos = comments.Select(c => new CommentWithReactionsDto
        {
            CommentId = c.CommentId,
            PostId = c.PostId,
            UserId = c.UserId,
            FullName = c.User.Fullname ?? string.Empty,
            AvatarUrl = c.User.AvatarUrl ?? string.Empty,
            Content = c.Content,
            ContentFormat = c.ContentFormat,
            ImageUrl = c.ImageUrl,
            ParentCommentId = c.ParentCommentId,
            LikeCount = c.LikeCount,
            MyReactionType = currentUserId.HasValue ? allCommentLikes.FirstOrDefault(l => l.CommentId == c.CommentId && l.UserId == currentUserId.Value)?.ReactionType : null,
            ReplyCount = c.ReplyCount,
            IsEdited = c.IsEdited,
            RegDatetime = c.RegDatetime,
            Mentions = c.CommentMentions
                .Select(m => new CommentMentionResponseDto
                {
                    MentionedUserId = m.MentionedUserId,
                    FullName = m.MentionedUser?.Fullname ?? string.Empty,
                    StartPos = m.StartPos ?? 0,
                    EndPos = m.EndPos ?? 0
                }).ToList(),
            Reactions = reactionsByCommentId.TryGetValue(c.CommentId, out var reactions)
                ? reactions
                : new List<CommentReactionDetailResponseDto>()
        }).ToList();

        // ── [8] Assemble PostDetailResponseDto ───────────────────────────────
        return new PostDetailResponseDto
        {
            PostId = post.PostId,
            UserId = post.User.Id,
            UserName = post.User.UserName ?? string.Empty,
            FullName = post.User.Fullname ?? string.Empty,
            AvatarUrl = post.User.AvatarUrl ?? string.Empty,
            Content = post.Content ?? string.Empty,
            PostType = post.PostType,
            Visibility = post.Visibility,
            Feeling = post.Feeling,
            LikeCount = post.LikeCount,
            CommentCount = post.CommentCount,
            ShareCount = post.ShareCount,
            IsLikeByMe = isLikedByMe,
            MyReactionType = myReactionType,
            IsPinned = post.IsPinned,
            AllowComment = post.AllowComment,
            LocationName = post.LocationName,
            RegDateTime = post.RegDatetime,
            BackgroundMusicId = post.BackgroundMusicId,
            Medias = post.PostMedias
                .OrderBy(m => m.SortOrder)
                .Select(m => new PostMediaResponseDto
                {
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType,
                    ThumbnailUrl = m.ThumbnailUrl
                }).ToList(),
            OriginalPost = originalPostDto,
            PostReactions = postReactionDtos,
            Comments = commentDtos,
            CommentsPage = 1,
            // Dùng CommentCount đã được lưu trong Post để tránh count query thêm.
            // CommentCount được cập nhật mỗi khi có comment mới (xem AddCommentAsync).
            CommentsHasMore = post.CommentCount > commentsPageSize
        };
    }

    public async Task<PostResponseDto> CreatePostAsync(long currentUserId, CreatePostRequestDto req)
    {
        // Validate bài gốc khi tạo shared post
        if (req.OriginalPostId.HasValue)
        {
            var originalExists = await _context.Posts
                .AnyAsync(p => p.PostId == req.OriginalPostId.Value && !p.Delflg);

            if (!originalExists)
                throw new ArgumentException("Bài viết gốc không tồn tại hoặc đã bị xóa.");
        }

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
        await _context.SaveChangesAsync(); // cần PostId trước khi tạo PostMedia

        // Bulk insert PostMedia nếu client gửi kèm danh sách media
        if (req.Medias != null && req.Medias.Count > 0)
        {
            var mediaEntities = req.Medias
                .Select((m, index) => new PostMedia
                {
                    PostId = post.PostId,
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType,
                    SortOrder = m.SortOrder > 0 ? m.SortOrder : index,
                    ThumbnailUrl = m.ThumbnailUrl,
                    ProcessingStatus = "Ready",
                    RegDatetime = DateTime.UtcNow,
                })
                .ToList();

            _context.PostMedias.AddRange(mediaEntities);
            await _context.SaveChangesAsync();
        }

        // Bulk insert PostMention nếu client gửi kèm danh sách mention
        if (req.Mentions != null && req.Mentions.Count > 0)
        {
            // Validate: chỉ insert những userId thực sự tồn tại trong DB
            var requestedIds = req.Mentions.Select(m => m.MentionedUserId).Distinct().ToList();
            var validIds = await _context.Users
                .Where(u => requestedIds.Contains(u.Id) && !u.Delflg)
                .Select(u => u.Id)
                .ToListAsync();

            var mentionEntities = req.Mentions
                .Where(m => validIds.Contains(m.MentionedUserId))
                .Select(m => new PostMention
                {
                    PostId = post.PostId,
                    MentionedUserId = m.MentionedUserId,
                    StartPos = m.StartPos,
                    EndPos = m.EndPos,
                    RegDatetime = DateTime.UtcNow,
                    Delflg = false
                })
                .ToList();

            if (mentionEntities.Count > 0)
            {
                _context.PostMentions.AddRange(mentionEntities);
                await _context.SaveChangesAsync();
            }
        }

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

    public async Task<(int likeCount, string? reactionType)> TogglePostReactionAsync(
        long currentUserId, long postId, string reactionType)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var post = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

            if (post == null) return (-1, null);

            var existing = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == currentUserId);

            string? resultReactionType;

            if (existing != null)
            {
                if (!existing.Delflg && existing.ReactionType == reactionType)
                {
                    // Cùng type -> toggle off (xóa mềm reaction)
                    existing.Delflg = true;
                    post.LikeCount = Math.Max(0, post.LikeCount - 1);
                    resultReactionType = null; // đã bỏ reaction
                }
                else
                {
                    // Khác type -> đổi sang type mới HOẶC đã unlike giờ like lại
                    if (existing.Delflg) 
                    {
                        post.LikeCount += 1;
                        existing.Delflg = false;
                    }
                    existing.ReactionType = reactionType;
                    existing.RegDatetime = DateTime.UtcNow;
                    resultReactionType = reactionType;
                }
            }
            else
            {
                // Chưa react -> thêm mới
                _context.PostLikes.Add(new PostLike
                {
                    PostId = postId,
                    UserId = currentUserId,
                    ReactionType = reactionType,
                    RegDatetime = DateTime.UtcNow,
                    Delflg = false,
                });
                post.LikeCount += 1;
                resultReactionType = reactionType;
            }

            post.UpdDatetime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Gửi notification chỉ khi react mới (không khi toggle off hoặc đổi type)
            if (resultReactionType != null && post.UserId != currentUserId)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: post.UserId,
                    notificationType: "PostReacted",
                    senderId: currentUserId,
                    referenceId: post.PostId,
                    referenceType: "Post",
                    message: "đã bày tỏ cảm xúc về bài viết của bạn",
                    redirectUrl: $"/posts/{post.PostId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(post.UserId, notification);
            }

            return (post.LikeCount, resultReactionType);
        });
    }

    public async Task<PostResponseDto?> SharePostAsync(long currentUserId, long postId, SharePostRequestDto req)
    {
        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            // [1] Kiểm tra bài gốc tồn tại
            var originalPost = await _context.Posts
                .FirstOrDefaultAsync(p => p.PostId == postId && !p.Delflg);

            if (originalPost == null) return null;

            // [2] Tạo bài Post mới trên feed của người share
            var sharedPost = new Post
            {
                UserId = currentUserId,
                PostType = "Shared",
                Visibility = req.Visibility,
                Content = req.Content,
                OriginalPostId = postId,
                AllowComment = true,
                RegDatetime = DateTime.UtcNow,
            };
            _context.Posts.Add(sharedPost);
            await _context.SaveChangesAsync(); // cần PostId để bước sau có thể reference

            // [3] Ghi PostShare tracking (lịch sử ai share bài nào)
            _context.PostShares.Add(new PostShare
            {
                PostId = postId,
                UserId = currentUserId,
                Visibility = req.Visibility,
                RegDatetime = DateTime.UtcNow,
            });

            // [4] Tăng ShareCount trên bài gốc
            originalPost.ShareCount += 1;
            originalPost.UpdDatetime = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // [5] Gửi notification cho chủ bài gốc
            if (originalPost.UserId != currentUserId)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: originalPost.UserId,
                    notificationType: "PostShared",
                    senderId: currentUserId,
                    referenceId: originalPost.PostId,
                    referenceType: "Post",
                    message: "đã chia sẻ bài viết của bạn",
                    redirectUrl: $"/posts/{originalPost.PostId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(originalPost.UserId, notification);
            }

            // [6] Load và trả về PostResponseDto của bài Post MỚI vừa tạo
            var createdPost = await BuildPostGraphQuery(asNoTracking: true)
                .FirstAsync(p => p.PostId == sharedPost.PostId);

            return await MapToDtoAsync(createdPost, currentUserId);
        });
    }
    public async Task<CommentResponseDto?> AddCommentAsync(long currentUserId, long postId, CreateCommentRequestDto req)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
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

            // Gửi notification cho chủ bài viết (nếu không phải chính mình)
            if (post != null && post.UserId != currentUserId)
            {
                var notification = await _notificationsService.CreateNotificationAsync(
                    recipientId: post.UserId,
                    notificationType: "PostCommented",
                    senderId: currentUserId,
                    referenceId: postId,
                    referenceType: "Post",
                    message: "đã bình luận về bài viết của bạn",
                    redirectUrl: $"/posts/{postId}");

                await _notificationRealtimeService.PushNotificationCreatedAsync(post.UserId, notification);
            }

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
        });
    }

    public async Task<CommentResponseDto?> UpdateCommentAsync(
        long currentUserId, long postId, long commentId, UpdateCommentRequestDto req)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.CommentMentions)
                .ThenInclude(m => m.MentionedUser)
            .FirstOrDefaultAsync(c => c.CommentId == commentId && c.PostId == postId && !c.Delflg);

        if (comment == null) return null;

        if (comment.UserId != currentUserId)
            throw new UnauthorizedAccessException("Bạn không có quyền sửa bình luận này.");

        comment.Content = req.Content;
        comment.ContentFormat = req.ContentFormat;
        comment.IsEdited = true;
        comment.UpdDatetime = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CommentResponseDto
        {
            CommentId = comment.CommentId,
            PostId = comment.PostId,
            UserId = comment.UserId,
            FullName = comment.User.Fullname ?? string.Empty,
            AvatarUrl = comment.User.AvatarUrl ?? string.Empty,
            Content = comment.Content,
            ContentFormat = comment.ContentFormat,
            ImageUrl = comment.ImageUrl,
            ParentCommentId = comment.ParentCommentId,
            LikeCount = comment.LikeCount,
            ReplyCount = comment.ReplyCount,
            IsEdited = true,
            RegDatetime = comment.RegDatetime,
            Mentions = comment.CommentMentions.Select(m => new CommentMentionResponseDto
            {
                MentionedUserId = m.MentionedUserId,
                FullName = m.MentionedUser?.Fullname ?? string.Empty,
                StartPos = m.StartPos ?? 0,
                EndPos = m.EndPos ?? 0
            }).ToList()
        };
    }

    public async Task<bool> DeleteCommentAsync(long currentUserId, long postId, long commentId)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.CommentId == commentId && c.PostId == postId && !c.Delflg);

            if (comment == null) return false;

            if (comment.UserId != currentUserId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa bình luận này.");

            // Soft delete comment
            comment.Delflg = true;
            comment.UpdDatetime = DateTime.UtcNow;

            // Giảm CommentCount trên bài viết
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                post.CommentCount = Math.Max(0, post.CommentCount - 1);
                post.UpdDatetime = DateTime.UtcNow;
            }

            // Nếu là reply → giảm ReplyCount của comment cha
            if (comment.ParentCommentId.HasValue)
            {
                var parent = await _context.Comments.FindAsync(comment.ParentCommentId.Value);
                if (parent != null)
                {
                    parent.ReplyCount = Math.Max(0, parent.ReplyCount - 1);
                    parent.UpdDatetime = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
        });
    }

    public async Task<int> ToggleCommentReactionAsync(long currentUserId, long postId, long commentId, string reactionType)
    {
        // Validate: comment tồn tại VÀ thuộc đúng post được chỉ định
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c => c.CommentId == commentId && c.PostId == postId && !c.Delflg);

        if (comment == null) return -1;

        var existing = await _context.CommentLikes
            .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == currentUserId);

        if (existing != null)
        {
            if (existing.ReactionType == reactionType)
            {
                // Same reaction → toggle off (remove)
                _context.CommentLikes.Remove(existing);
                comment.LikeCount = Math.Max(0, comment.LikeCount - 1);
            }
            else
            {
                // Different reaction → switch type
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
    public async Task<List<CommentResponseDto>> GetPostCommentsAsync(long? currentUserId, long postId, int page, int pageSize)
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

    var commentIds = comments.Select(c => c.CommentId).ToList();
    var allCommentLikes = currentUserId.HasValue
        ? await _context.CommentLikes
            .Where(l => commentIds.Contains(l.CommentId) && l.UserId == currentUserId.Value && !l.Delflg)
            .AsNoTracking()
            .ToListAsync()
        : new List<CommentLike>();

    return comments.Select(c => new CommentResponseDto
    {
        CommentId = c.CommentId,
        PostId = c.PostId,
        UserId = c.UserId,
        FullName = c.User.Fullname ?? string.Empty,
        AvatarUrl = c.User.AvatarUrl ?? string.Empty,
        Content = c.Content,
        LikeCount = c.LikeCount,
        MyReactionType = currentUserId.HasValue ? allCommentLikes.FirstOrDefault(l => l.CommentId == c.CommentId)?.ReactionType : null,
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

    public async Task<List<CommentResponseDto>> GetCommentRepliesAsync(long? currentUserId, long postId, long commentId, int page, int pageSize)
    {
        var replies = await _context.Comments
            .Where(c => c.PostId == postId && c.ParentCommentId == commentId && !c.Delflg)
            .Include(c => c.User)
            .Include(c => c.CommentMentions)
                .ThenInclude(m => m.MentionedUser)
            .OrderBy(c => c.RegDatetime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var commentIds = replies.Select(c => c.CommentId).ToList();
        var allCommentLikes = currentUserId.HasValue
            ? await _context.CommentLikes
                .Where(l => commentIds.Contains(l.CommentId) && l.UserId == currentUserId.Value && !l.Delflg)
                .AsNoTracking()
                .ToListAsync()
            : new List<CommentLike>();

        return replies.Select(c => new CommentResponseDto
        {
            CommentId = c.CommentId,
            PostId = c.PostId,
            UserId = c.UserId,
            FullName = c.User.Fullname ?? string.Empty,
            AvatarUrl = c.User.AvatarUrl ?? string.Empty,
            Content = c.Content,
            ContentFormat = c.ContentFormat,
            ImageUrl = c.ImageUrl,
            ParentCommentId = c.ParentCommentId,
            LikeCount = c.LikeCount,
            MyReactionType = currentUserId.HasValue ? allCommentLikes.FirstOrDefault(l => l.CommentId == c.CommentId)?.ReactionType : null,
            ReplyCount = c.ReplyCount,
            IsEdited = c.IsEdited,
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