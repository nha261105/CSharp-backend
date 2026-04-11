using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.PostId);

        builder.Property(p => p.PostId)
            .HasColumnName("post_id");

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(p => p.Content)
            .HasColumnName("content")
            .HasColumnType("text");

        builder.Property(p => p.ContentFormat)
            .HasColumnName("content_format")
            .HasColumnType("text");

        builder.Property(p => p.PostType)
            .HasColumnName("post_type")
            .HasMaxLength(30)
            .HasDefaultValue("Text");

        builder.Property(p => p.Visibility)
            .HasColumnName("visibility")
            .HasMaxLength(20)
            .HasDefaultValue("Public");

        builder.Property(p => p.LocationName)
            .HasColumnName("location_name")
            .HasMaxLength(200);

        builder.Property(p => p.LocationLat)
            .HasColumnName("location_lat")
            .HasColumnType("decimal(10,7)");

        builder.Property(p => p.LocationLng)
            .HasColumnName("location_lng")
            .HasColumnType("decimal(10,7)");

        builder.Property(p => p.Feeling)
            .HasColumnName("feeling")
            .HasMaxLength(100);

        builder.Property(p => p.OriginalPostId)
            .HasColumnName("original_post_id");

        builder.Property(p => p.BackgroundMusicId)
            .HasColumnName("background_music_id");

        builder.Property(p => p.MusicStartSec)
            .HasColumnName("music_start_sec")
            .HasDefaultValue(0);

        builder.Property(p => p.MusicEndSec)
            .HasColumnName("music_end_sec");

        builder.Property(p => p.LikeCount)
            .HasColumnName("like_count")
            .HasDefaultValue(0);

        builder.Property(p => p.CommentCount)
            .HasColumnName("comment_count")
            .HasDefaultValue(0);

        builder.Property(p => p.ShareCount)
            .HasColumnName("share_count")
            .HasDefaultValue(0);

        builder.Property(p => p.IsEdited)
            .HasColumnName("is_edited")
            .HasDefaultValue(false);

        builder.Property(p => p.IsPinned)
            .HasColumnName("is_pinned")
            .HasDefaultValue(false);

        builder.Property(p => p.IsReported)
            .HasColumnName("is_reported")
            .HasDefaultValue(false);

        builder.Property(p => p.ReportCount)
            .HasColumnName("report_count")
            .HasDefaultValue(0);

        builder.Property(p => p.AllowComment)
            .HasColumnName("allow_comment")
            .HasDefaultValue(true);

        builder.Property(p => p.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(p => p.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(p => p.UpdDatetime)
            .HasColumnName("upd_datetime");

        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Posts_user_id");

        builder.HasIndex(p => p.RegDatetime)
            .HasDatabaseName("IX_Posts_reg_datetime");

        builder.HasIndex(p => p.Visibility)
            .HasDatabaseName("IX_Posts_visibility");

        builder.HasOne(p => p.OriginalPost)
            .WithMany(p => p.SharedPosts)
            .HasForeignKey(p => p.OriginalPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.BackgroundMusic)
            .WithMany(m => m.Posts)
            .HasForeignKey(p => p.BackgroundMusicId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(p => p.PostMedias)
            .WithOne(m => m.Post)
            .HasForeignKey(m => m.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PostLikes)
            .WithOne(l => l.Post)
            .HasForeignKey(l => l.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PostShares)
            .WithOne(s => s.Post)
            .HasForeignKey(s => s.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PostHashtags)
            .WithOne(h => h.Post)
            .HasForeignKey(h => h.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PostMentions)
            .WithOne(m => m.Post)
            .HasForeignKey(m => m.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.PostReports)
            .WithOne(r => r.Post)
            .HasForeignKey(r => r.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
