using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class PostReportConfiguration : IEntityTypeConfiguration<PostReport>
{
    public void Configure(EntityTypeBuilder<PostReport> builder)
    {
        builder.ToTable("PostReports");

        builder.HasKey(r => r.ReportId);

        builder.Property(r => r.ReportId)
            .HasColumnName("report_id");

        builder.Property(r => r.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(r => r.ReporterId)
            .HasColumnName("reporter_id")
            .IsRequired();

        builder.Property(r => r.Reason)
            .HasColumnName("reason")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasColumnName("description")
            .HasMaxLength(1000);

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasMaxLength(20)
            .HasDefaultValue("Pending");

        builder.Property(r => r.ReviewedById)
            .HasColumnName("reviewed_by_id");

        builder.Property(r => r.ReviewNote)
            .HasColumnName("review_note")
            .HasMaxLength(500);

        builder.Property(r => r.ActionTaken)
            .HasColumnName("action_taken")
            .HasMaxLength(100);

        builder.Property(r => r.ReviewDatetime)
            .HasColumnName("review_datetime");

        builder.Property(r => r.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(r => r.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(r => r.UpdDatetime)
            .HasColumnName("upd_datetime");

        builder.HasOne(r => r.Reporter)
            .WithMany()
            .HasForeignKey(r => r.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ReviewedBy)
            .WithMany()
            .HasForeignKey(r => r.ReviewedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
