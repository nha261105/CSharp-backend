using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AspNetUsers");

        // CUSTOM FIELD: CLASS -> COLUMN
        builder.Property(u => u.Fullname).HasColumnName("full_name").HasMaxLength(150).IsRequired();

        builder.Property(u => u.Gender).HasColumnName("gender").HasMaxLength(20);

        builder.Property(u => u.DateOfBirth).HasColumnName("date_of_birth");

        builder.Property(u => u.AvatarUrl).HasColumnName("avatar_url").HasMaxLength(500);

        builder.Property(u => u.CoverPhotoUrl).HasColumnName("cover_photo_url").HasMaxLength(500);

        builder.Property(u => u.Bio)
            .HasColumnName("bio")
            .HasMaxLength(500);

        builder.Property(u => u.WebsiteUrl)
            .HasColumnName("website_url")
            .HasMaxLength(300);

        builder.Property(u => u.Location)
            .HasColumnName("location")
            .HasMaxLength(200);

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        builder.Property(u => u.IsPrivateAccount)
            .HasColumnName("is_private_account")
            .HasDefaultValue(false);

        builder.Property(u => u.LastLoginDateTime)
            .HasColumnName("last_login_datetime");

        builder.Property(u => u.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(u => u.RegDateTime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.UpDatetime)
            .HasColumnName("upd_datetime");

        // RELATIONSHIP

        builder.HasOne(u => u.Profile).WithOne(p => p.User).HasForeignKey<UserProfile>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Posts).WithOne(p => p.User).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SentFriendships)
            .WithOne(f => f.Requester)
            .HasForeignKey(f => f.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ReceivedFriendships)
            .WithOne(f => f.Addressee)
            .HasForeignKey(f => f.AddresseeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.SentNotifications)
            .WithOne(n => n.Sender)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.ReceivedNotifications)
            .WithOne(n => n.Recipient)
            .HasForeignKey(n => n.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}