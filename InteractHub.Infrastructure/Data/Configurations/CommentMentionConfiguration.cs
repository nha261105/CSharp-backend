using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class CommentMentionConfiguration : IEntityTypeConfiguration<CommentMention>
{
    public void Configure(EntityTypeBuilder<CommentMention> builder)
    {
        builder.ToTable("CommentMentions");

        builder.HasKey(m => m.MentionId);

        builder.Property(m => m.MentionId)
            .HasColumnName("mention_id");

        builder.Property(m => m.CommentId)
            .HasColumnName("comment_id")
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
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(m => m.MentionedUser)
            .WithMany()
            .HasForeignKey(m => m.MentionedUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
