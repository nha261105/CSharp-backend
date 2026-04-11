using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostMediaConfiguration : IEntityTypeConfiguration<PostMedia>
{
    public void Configure(EntityTypeBuilder<PostMedia> builder)
    {
        builder.ToTable("PostMedia");

        builder.HasKey(m => m.MediaId);

        builder.Property(m => m.MediaId)
            .HasColumnName("media_id");

        builder.Property(m => m.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(m => m.MediaUrl)
            .HasColumnName("media_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(m => m.MediaType)
            .HasColumnName("media_type")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.ThumbnailUrl)
            .HasColumnName("thumbnail_url")
            .HasMaxLength(500);

        builder.Property(m => m.FileName)
            .HasColumnName("file_name")
            .HasMaxLength(255);

        builder.Property(m => m.FileSizeKb)
            .HasColumnName("file_size_kb");

        builder.Property(m => m.WidthPx)
            .HasColumnName("width_px");

        builder.Property(m => m.HeightPx)
            .HasColumnName("height_px");

        builder.Property(m => m.DurationSeconds)
            .HasColumnName("duration_seconds");

        builder.Property(m => m.SortOrder)
            .HasColumnName("sort_order")
            .HasDefaultValue(0);

        builder.Property(m => m.ProcessingStatus)
            .HasColumnName("processing_status")
            .HasMaxLength(20)
            .HasDefaultValue("Ready");

        builder.Property(m => m.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(m => m.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
