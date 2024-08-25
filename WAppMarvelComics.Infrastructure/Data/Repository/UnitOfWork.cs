using Microsoft.EntityFrameworkCore.Storage;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Infrastructure.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;
        private bool _disposed = false;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public bool HasActiveTransaction => _currentTransaction != null;

        public IDbContextTransaction? GetCurrentTransaction() => _currentTransaction;

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();

            return _currentTransaction;
        }

        public async Task CommitAsync(IDbContextTransaction transaction, CancellationToken cancellationToken = default)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync(IDbContextTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            try
            {
                await transaction.RollbackAsync();
            }
            finally
            {
                _currentTransaction = null;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _currentTransaction?.Dispose();
                    _dbContext.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}