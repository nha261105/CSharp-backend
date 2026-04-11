using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class StoryViewConfiguration : IEntityTypeConfiguration<StoryView>
{
    public void Configure(EntityTypeBuilder<StoryView> builder)
    {
        builder.ToTable("StoryViews");

        builder.HasKey(v => v.ViewId);

        builder.Property(v => v.ViewId)
            .HasColumnName("view_id");

        builder.Property(v => v.StoryId)
            .HasColumnName("story_id")
            .IsRequired();

        builder.Property(v => v.ViewerId)
            .HasColumnName("viewer_id")
            .IsRequired();

        builder.Property(v => v.ViewDuration)
            .HasColumnName("view_duration");

        builder.Property(v => v.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(v => v.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(v => new { v.StoryId, v.ViewerId })
            .IsUnique()
            .HasDatabaseName("UQ_StoryViews_pair");

        builder.HasOne(v => v.Viewer)
            .WithMany()
            .HasForeignKey(v => v.ViewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
