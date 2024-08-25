using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Infrastructure.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "Master");
            builder.HasKey(e => new { e.IdUser, e.IdType }).HasName("PK_User");

            builder.HasIndex(e => e.Email, "UC_Email").IsUnique();

            builder.Property(e => e.IdUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            builder.Property(e => e.IdType)
                .HasMaxLength(4)
                .IsUnicode(false);
            builder.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserIdType");
        }
    }
}