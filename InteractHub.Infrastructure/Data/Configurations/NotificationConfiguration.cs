using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.NotificationId);

        builder.Property(n => n.NotificationId)
            .HasColumnName("notification_id");

        builder.Property(n => n.RecipientId)
            .HasColumnName("recipient_id")
            .IsRequired();

        builder.Property(n => n.SenderId)
            .HasColumnName("sender_id");

        builder.Property(n => n.NotificationType)
            .HasColumnName("notification_type")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(n => n.ReferenceId)
            .HasColumnName("reference_id");

        builder.Property(n => n.ReferenceType)
            .HasColumnName("reference_type")
            .HasMaxLength(50);

        builder.Property(n => n.Message)
            .HasColumnName("message")
            .HasMaxLength(500);

        builder.Property(n => n.IsRead)
            .HasColumnName("is_read")
            .HasDefaultValue(false);

        builder.Property(n => n.ReadDatetime)
            .HasColumnName("read_datetime");

        builder.Property(n => n.RedirectUrl)
            .HasColumnName("redirect_url")
            .HasMaxLength(500);

        builder.Property(n => n.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(n => n.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(n => new { n.RecipientId, n.IsRead })
            .HasDatabaseName("IX_Notifications_recipient");
    }
}
