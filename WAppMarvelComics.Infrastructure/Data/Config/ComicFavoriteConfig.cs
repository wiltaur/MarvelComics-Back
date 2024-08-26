using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate;

namespace WAppMarvelComics.Infrastructure.Data.Config
{
    public class ComicFavoriteConfig : IEntityTypeConfiguration<ComicFavorite>
    {
        public void Configure(EntityTypeBuilder<ComicFavorite> builder)
        {
            builder.ToTable("ComicFavorite", "Master");

            builder.HasKey(e => new { e.Id, e.IdUser, e.IdTypeUser });

            builder.Property(e => e.Id)
                .HasColumnType("int");
            builder.Property(e => e.IdUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.IdTypeUser)
                .HasMaxLength(4)
                .IsUnicode(false);

            builder.HasOne(d => d.User).WithMany(p => p.ComicFavorites)
                .HasForeignKey(d => new { d.IdUser, d.IdTypeUser })
                .HasConstraintName("FK_ComicFavorite_User");

            builder.HasOne(d => d.Comic).WithMany(p => p.ComicFavorites)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_ComicFavorite_Comic");
        }
    }
}