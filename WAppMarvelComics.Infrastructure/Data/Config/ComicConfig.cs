using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAppMarvelComics.Domain.Aggregates.ComicAggregate;

namespace WAppMarvelComics.Infrastructure.Data.Config
{
    public class ComicConfig : IEntityTypeConfiguration<Comic>
    {
        public void Configure(EntityTypeBuilder<Comic> builder)
        {
            builder.ToTable("Comic", "Master");

            builder.HasKey(e => new { e.Id });

            builder.Property(e => e.Id)
                .HasColumnType("int");
            builder.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
            builder.Property(e => e.Description)
                .IsUnicode(false);
            builder.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
        }
    }
}