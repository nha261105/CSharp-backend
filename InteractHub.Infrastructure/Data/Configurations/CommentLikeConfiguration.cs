using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
{
    public void Configure(EntityTypeBuilder<CommentLike> builder)
    {
        builder.ToTable("CommentLikes");

        builder.HasKey(l => l.LikeId);

        builder.Property(l => l.LikeId)
            .HasColumnName("like_id");

        builder.Property(l => l.CommentId)
            .HasColumnName("comment_id")
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

        builder.HasIndex(l => new { l.CommentId, l.UserId })
            .IsUnique()
            .HasDatabaseName("UQ_CommentLikes_pair");

        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
