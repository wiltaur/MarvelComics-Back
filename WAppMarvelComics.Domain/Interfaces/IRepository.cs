using Ardalis.Specification;

namespace WAppMarvelComics.Domain.Interfaces;

// from Ardalis.Specification
public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}