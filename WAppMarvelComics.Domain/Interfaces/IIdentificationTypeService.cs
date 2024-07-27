using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Domain.Interfaces
{
    public interface IIdentificationTypeService
    {
        Task<List<IdentificationType>> GetAllIdTypes(CancellationToken cancellationToken = default);
    }
}