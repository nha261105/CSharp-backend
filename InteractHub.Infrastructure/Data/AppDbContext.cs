using InteractHub.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InteractHub.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<User, Role, long>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // USER
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    // MUSIC
    public DbSet<MusicTrack> MusicTracks => Set<MusicTrack>();

    // POST
    public DbSet<Post> Posts => Set<Post>();

    public DbSet<PostMedia> PostMedias => Set<PostMedia>();

    public DbSet<PostLike> PostLikes => Set<PostLike>();

    public DbSet<PostShare> PostShares => Set<PostShare>();

    public DbSet<PostHashtag> PostHashtags => Set<PostHashtag>();

    public DbSet<PostMention> PostMentions => Set<PostMention>();

    // COMMENT
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
    public DbSet<CommentMention> CommentMentions => Set<CommentMention>();

    // Hashtags
    public DbSet<Hashtag> Hashtags => Set<Hashtag>();

    // FRIENDSHIPS
    public DbSet<Friendship> Friendships => Set<Friendship>();

    // STORIES
    public DbSet<Story> Stories => Set<Story>();
    public DbSet<StoryView> StoryViews => Set<StoryView>();
    public DbSet<StoryReaction> StoryReactions => Set<StoryReaction>();

    // NOTIFICATIONS
    public DbSet<Notification> Notifications => Set<Notification>();

    // MODERATION
    public DbSet<PostReport> PostReports => Set<PostReport>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly
        );
    }
}