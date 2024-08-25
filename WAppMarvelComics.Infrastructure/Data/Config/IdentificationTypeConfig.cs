using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Infrastructure.Data.Config
{
    public class IdentificationTypeConfig : IEntityTypeConfiguration<IdentificationType>
    {
        public void Configure(EntityTypeBuilder<IdentificationType> builder)
        {
            builder.ToTable("IdentificationType", "Master");
            builder.HasKey(e => e.IdType).HasName("PK_IdType");

            builder.Property(e => e.IdType)
                .HasMaxLength(4)
                .IsUnicode(false);
            builder.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}