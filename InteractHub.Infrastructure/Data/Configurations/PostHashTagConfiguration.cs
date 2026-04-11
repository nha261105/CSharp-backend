using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostHashtagConfiguration : IEntityTypeConfiguration<PostHashtag>
{
    public void Configure(EntityTypeBuilder<PostHashtag> builder)
    {
        builder.ToTable("PostHashtags");

        builder.HasKey(ph => ph.PostHashtagId);

        builder.Property(ph => ph.PostHashtagId)
            .HasColumnName("post_hashtag_id");

        builder.Property(ph => ph.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(ph => ph.HashtagId)
            .HasColumnName("hashtag_id")
            .IsRequired();

        builder.HasIndex(ph => new { ph.PostId, ph.HashtagId })
            .IsUnique()
            .HasDatabaseName("UQ_PostHashtags_pair");

        builder.Property(ph => ph.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(ph => ph.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(ph => ph.Hashtag)
            .WithMany(h => h.PostHashtags)
            .HasForeignKey(ph => ph.HashtagId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
