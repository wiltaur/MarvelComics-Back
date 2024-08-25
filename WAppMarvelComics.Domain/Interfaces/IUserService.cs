using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Domain.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(User user, CancellationToken cancellationToken = default);
        Task<string> ValidateUser(string email, string password, CancellationToken cancellationToken = default);
    }
}