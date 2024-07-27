using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<IdentificationType> IdentificationTypes => Set<IdentificationType>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
         return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}