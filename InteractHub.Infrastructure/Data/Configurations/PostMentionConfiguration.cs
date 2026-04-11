using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostMentionConfiguration : IEntityTypeConfiguration<PostMention>
{
    public void Configure(EntityTypeBuilder<PostMention> builder)
    {
        builder.ToTable("PostMentions");

        builder.HasKey(m => m.MentionId);

        builder.Property(m => m.MentionId)
            .HasColumnName("mention_id");

        builder.Property(m => m.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(m => m.MentionedUserId)
            .HasColumnName("mentioned_user_id")
            .IsRequired();

        builder.Property(m => m.StartPos)
            .HasColumnName("start_pos");

        builder.Property(m => m.EndPos)
            .HasColumnName("end_pos");

        builder.Property(m => m.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(m => m.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(m => m.MentionedUser)
            .WithMany()
            .HasForeignKey(m => m.MentionedUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
