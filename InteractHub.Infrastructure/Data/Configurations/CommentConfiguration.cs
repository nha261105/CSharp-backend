using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.CommentId);

        builder.Property(c => c.CommentId)
            .HasColumnName("comment_id");

        builder.Property(c => c.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(c => c.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(c => c.ParentCommentId)
            .HasColumnName("parent_comment_id");

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(c => c.ContentFormat)
            .HasColumnName("content_format")
            .HasColumnType("text");

        builder.Property(c => c.ImageUrl)
            .HasColumnName("image_url")
            .HasMaxLength(500);

        builder.Property(c => c.LikeCount)
            .HasColumnName("like_count")
            .HasDefaultValue(0);

        builder.Property(c => c.ReplyCount)
            .HasColumnName("reply_count")
            .HasDefaultValue(0);

        builder.Property(c => c.IsEdited)
            .HasColumnName("is_edited")
            .HasDefaultValue(false);

        builder.Property(c => c.IsReported)
            .HasColumnName("is_reported")
            .HasDefaultValue(false);

        builder.Property(c => c.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(c => c.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdDatetime)
            .HasColumnName("upd_datetime");

        builder.HasIndex(c => c.PostId)
            .HasDatabaseName("IX_Comments_post_id");

        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("IX_Comments_user_id");

        builder.HasIndex(c => c.ParentCommentId)
            .HasDatabaseName("IX_Comments_parent_id");

        builder.HasOne(c => c.ParentComment)
            .WithMany(c => c.Replies)
            .HasForeignKey(c => c.ParentCommentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.CommentLikes)
            .WithOne(l => l.Comment)
            .HasForeignKey(l => l.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CommentMentions)
            .WithOne(m => m.Comment)
            .HasForeignKey(m => m.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
