using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class HashtagConfiguration : IEntityTypeConfiguration<Hashtag>
{
    public void Configure(EntityTypeBuilder<Hashtag> builder)
    {
        builder.ToTable("Hashtags");

        builder.HasKey(h => h.HashtagId);

        builder.Property(h => h.HashtagId)
            .HasColumnName("hashtag_id");

        builder.Property(h => h.TagName)
            .HasColumnName("tag_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(h => h.TagName)
            .IsUnique()
            .HasDatabaseName("UQ_Hashtags_tagName");

        builder.HasIndex(h => new { h.TrendingScore, h.IsTrending })
            .HasDatabaseName("IX_Hashtags_trending");

        builder.Property(h => h.PostCount)
            .HasColumnName("post_count")
            .HasDefaultValue(0);

        builder.Property(h => h.TrendingScore)
            .HasColumnName("trending_score")
            .HasColumnType("decimal(10,2)")
            .HasDefaultValue(0);

        builder.Property(h => h.IsTrending)
            .HasColumnName("is_trending")
            .HasDefaultValue(false);

        builder.Property(h => h.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(h => h.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(h => h.UpdDatetime)
            .HasColumnName("upd_datetime");
    }
}
