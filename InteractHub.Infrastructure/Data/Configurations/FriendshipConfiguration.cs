using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.ToTable("Friendships");

        builder.HasKey(f => f.FriendshipId);

        builder.Property(f => f.FriendshipId)
            .HasColumnName("friendship_id");

        builder.Property(f => f.RequesterId)
            .HasColumnName("requester_id")
            .IsRequired();

        builder.Property(f => f.AddresseeId)
            .HasColumnName("addressee_id")
            .IsRequired();

        builder.Property(f => f.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("Pending");

        builder.Property(f => f.ActionUserId)
            .HasColumnName("action_user_id");

        builder.Property(f => f.IsBlocked)
            .HasColumnName("is_blocked")
            .HasDefaultValue(false);

        builder.Property(f => f.BlockedById)
            .HasColumnName("blocked_by_id");

        builder.Property(f => f.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(f => f.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(f => f.UpdDatetime)
            .HasColumnName("upd_datetime");

        builder.HasIndex(f => new { f.RequesterId, f.AddresseeId })
            .IsUnique()
            .HasDatabaseName("UQ_Friendships_pair");

        builder.HasIndex(f => new { f.RequesterId, f.Status })
            .HasDatabaseName("IX_Friendships_requester");

        builder.HasIndex(f => new { f.AddresseeId, f.Status })
            .HasDatabaseName("IX_Friendships_addressee");

        builder.HasOne(f => f.ActionUser)
            .WithMany()
            .HasForeignKey(f => f.ActionUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.BlockedBy)
            .WithMany()
            .HasForeignKey(f => f.BlockedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
