using WAppMarvelComics.Domain.Aggregates.ComicAggregate;
using WAppMarvelComics.Domain.Custom.Models;

namespace WAppMarvelComics.Domain.Interfaces
{
    public interface IComicService
    {
        Task<DataTableModel<ComicModel>> GetComics(int limit, int offset, CancellationToken cancellationToken);
        Task<ComicDetailModel> GetComicById(int comicId, CancellationToken cancellationToken);
        Task<bool> AddFavoriteComic(Comic comic, string emailUser, CancellationToken cancellationToken);
        Task<List<Comic>> GetFavoriteComics(string emailUser, CancellationToken cancellationToken);
    }
}