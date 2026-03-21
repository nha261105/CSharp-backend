using InteractHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InteractHub.Infrastructure.Data.Configurations;

public class MusicTrackConfiguration : IEntityTypeConfiguration<MusicTrack>
{
    public void Configure(EntityTypeBuilder<MusicTrack> builder)
    {
        builder.ToTable("MusicTracks");

        builder.HasKey(m => m.MusicId);

        builder.Property(m => m.MusicId)
            .HasColumnName("music_id");

        builder.Property(m => m.Title)
            .HasColumnName("title")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Artist)
            .HasColumnName("artist")
            .HasMaxLength(150);

        builder.Property(m => m.AudioUrl)
            .HasColumnName("audio_url")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(m => m.ThumbnailUrl)
            .HasColumnName("thumbnail_url")
            .HasMaxLength(300);

        builder.Property(m => m.DurationSec)
            .HasColumnName("duration_sec");

        builder.Property(m => m.IsLicensed)
            .HasColumnName("is_licensed")
            .HasDefaultValue(true);

        builder.Property(m => m.Source)
            .HasColumnName("source")
            .HasMaxLength(50)
            .HasDefaultValue("Internal");

        builder.Property(m => m.Delflg)
            .HasColumnName("delflg")
            .HasDefaultValue(false);

        builder.Property(m => m.RegDatetime)
            .HasColumnName("reg_datetime")
            .HasDefaultValueSql("GETDATE()");

        builder.Property(m => m.UpdDatetime)
            .HasColumnName("upd_datetime");
    }
}
