using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");

        builder.HasKey(p => p.ProfileId);

        builder.Property(p => p.ProfileId)
            .HasColumnName("profile_id");

        builder.Property(p => p.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.RelationshipStatus)
            .HasColumnName("relationship_status")
            .HasMaxLength(30);

        builder.Property(p => p.WorkPlace)
            .HasColumnName("work_place")
            .HasMaxLength(200);

        builder.Property(p => p.Position)
            .HasColumnName("position")
            .HasMaxLength(150);

        builder.Property(p => p.Education)
            .HasColumnName("education")
            .HasMaxLength(200);

        builder.Property(p => p.Hometown)
            .HasColumnName("hometown")
            .HasMaxLength(200);

        builder.Property(p => p.CurrentCity)
            .HasColumnName("current_city")
            .HasMaxLength(200);

        builder.Property(p => p.FacebookLink)
            .HasColumnName("facebook_link")
            .HasMaxLength(300);

        builder.Property(p => p.InstagramLink)
            .HasColumnName("instagram_link")
            .HasMaxLength(300);

        builder.Property(p => p.TwitterLink)
            .HasColumnName("twitter_link")
            .HasMaxLength(300);

        builder.Property(p => p.FollowerCount)
            .HasColumnName("follower_count")
            .HasDefaultValue(0);

        builder.Property(p => p.FollowingCount)
            .HasColumnName("following_count")
            .HasDefaultValue(0);

        builder.Property(p => p.PostCount)
            .HasColumnName("post_count")
            .HasDefaultValue(0);

        builder.Property(p => p.FriendCount)
            .HasColumnName("friend_count")
            .HasDefaultValue(0);

        builder.Property(p => p.PrivacyPosts)
            .HasColumnName("privacy_posts")
            .HasMaxLength(20)
            .HasDefaultValue("Public");

        builder.Property(p => p.PrivacyFriends)
            .HasColumnName("privacy_friends")
            .HasMaxLength(20)
            .HasDefaultValue("Public");

        builder.Property(p => p.PrivacyPhotos)
            .HasColumnName("privacy_photos")
            .HasMaxLength(20)
            .HasDefaultValue("Public");

        builder.Property(p => p.NotificationEmailFlg)
            .HasColumnName("notification_email_flg")
            .HasDefaultValue(true);

        builder.Property(p => p.NotificationPushFlg)
            .HasColumnName("notification_push_flg")
            .HasDefaultValue(true);

        builder.Property(p => p.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(p => p.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(p => p.UpdDatetime)
            .HasColumnName("upd_datetime");
    }
}
