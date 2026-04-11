using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class StoryConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        builder.ToTable("Stories");

        builder.HasKey(s => s.StoryId);

        builder.Property(s => s.StoryId)
            .HasColumnName("story_id");

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.MediaUrl)
            .HasColumnName("media_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.MediaType)
            .HasColumnName("media_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(s => s.ThumbnailUrl)
            .HasColumnName("thumbnail_url")
            .HasMaxLength(500);

        builder.Property(s => s.Caption)
            .HasColumnName("caption")
            .HasMaxLength(500);

        builder.Property(s => s.CaptionFormat)
            .HasColumnName("caption_format")
            .HasColumnType("text");

        builder.Property(s => s.BgColor)
            .HasColumnName("bg_color")
            .HasMaxLength(20);

        builder.Property(s => s.FontStyle)
            .HasColumnName("font_style")
            .HasMaxLength(50);

        builder.Property(s => s.DurationSec)
            .HasColumnName("duration_sec")
            .HasDefaultValue(5);

        builder.Property(s => s.Visibility)
            .HasColumnName("visibility")
            .HasMaxLength(20)
            .HasDefaultValue("Friends");

        builder.Property(s => s.BackgroundMusicId)
            .HasColumnName("background_music_id");

        builder.Property(s => s.MusicStartSec)
            .HasColumnName("music_start_sec")
            .HasDefaultValue(0);

        builder.Property(s => s.MusicEndSec)
            .HasColumnName("music_end_sec");

        builder.Property(s => s.ViewCount)
            .HasColumnName("view_count")
            .HasDefaultValue(0);

        builder.Property(s => s.ReactionCount)
            .HasColumnName("reaction_count")
            .HasDefaultValue(0);

        builder.Property(s => s.ExpireDatetime)
            .HasColumnName("expire_datetime")
            .IsRequired();

        builder.Property(s => s.IsExpired)
            .HasColumnName("is_expired")
            .HasDefaultValue(false);

        builder.Property(s => s.IsHighlighted)
            .HasColumnName("is_highlighted")
            .HasDefaultValue(false);

        builder.Property(s => s.HighlightName)
            .HasColumnName("highlight_name")
            .HasMaxLength(100);

        builder.Property(s => s.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(s => s.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(s => s.UserId)
            .HasDatabaseName("IX_Stories_user_id");

        builder.HasIndex(s => s.ExpireDatetime)
            .HasDatabaseName("IX_Stories_expire");

        builder.HasOne(s => s.BackgroundMusic)
            .WithMany(m => m.Stories)
            .HasForeignKey(s => s.BackgroundMusicId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(s => s.StoryViews)
            .WithOne(v => v.Story)
            .HasForeignKey(v => v.StoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.StoryReactions)
            .WithOne(r => r.Story)
            .HasForeignKey(r => r.StoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
