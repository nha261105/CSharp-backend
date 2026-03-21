using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostLikeConfiguration : IEntityTypeConfiguration<PostLike>
{
    public void Configure(EntityTypeBuilder<PostLike> builder)
    {
        builder.ToTable("PostLikes");

        builder.HasKey(l => l.LikeId);

        builder.Property(l => l.LikeId)
            .HasColumnName("like_id");

        builder.Property(l => l.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(l => l.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(l => l.ReactionType)
            .HasColumnName("reaction_type")
            .HasMaxLength(20)
            .HasDefaultValue("Like");

        builder.Property(l => l.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(l => l.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(l => new { l.PostId, l.UserId })
            .IsUnique()
            .HasDatabaseName("UQ_PostLikes_pair");

        builder.HasOne(l => l.User)
            .WithMany(u => u.PostLikes)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
