using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostShareConfiguration : IEntityTypeConfiguration<PostShare>
{
    public void Configure(EntityTypeBuilder<PostShare> builder)
    {
        builder.ToTable("PostShares");

        builder.HasKey(s => s.ShareId);

        builder.Property(s => s.ShareId)
            .HasColumnName("share_id");

        builder.Property(s => s.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.ShareContent)
            .HasColumnName("share_content")
            .HasMaxLength(1000);

        builder.Property(s => s.Visibility)
            .HasColumnName("visibility")
            .HasMaxLength(20)
            .HasDefaultValue("Public");

        builder.Property(s => s.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(s => s.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.HasOne(s => s.User)
            .WithMany(u => u.PostShares)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
