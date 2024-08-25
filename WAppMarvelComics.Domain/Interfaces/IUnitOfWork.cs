using Microsoft.EntityFrameworkCore.Storage;

namespace WAppMarvelComics.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        bool HasActiveTransaction { get; }

        IDbContextTransaction? GetCurrentTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default);

        Task RollbackAsync(IDbContextTransaction transaction);
    }
}