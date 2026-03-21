using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class StoryReactionConfiguration : IEntityTypeConfiguration<StoryReaction>
{
    public void Configure(EntityTypeBuilder<StoryReaction> builder)
    {
        builder.ToTable("StoryReactions");

        builder.HasKey(r => r.ReactionId);

        builder.Property(r => r.ReactionId)
            .HasColumnName("reaction_id");

        builder.Property(r => r.StoryId)
            .HasColumnName("story_id")
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(r => r.ReactionType)
            .HasColumnName("reaction_type")
            .HasMaxLength(20)
            .HasDefaultValue("Like");

        builder.Property(r => r.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(r => r.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasIndex(r => new { r.StoryId, r.UserId })
            .IsUnique()
            .HasDatabaseName("UQ_StoryReactions_pair");

        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
