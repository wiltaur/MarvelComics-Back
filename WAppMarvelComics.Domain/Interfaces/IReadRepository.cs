using Ardalis.Specification;

namespace WAppMarvelComics.Domain.Interfaces;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}