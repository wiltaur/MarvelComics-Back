using WAppMarvelComics.Domain.Aggregates;

namespace WAppMarvelComics.Domain.Interfaces
{
    public interface ISecureUtilities
    {
        string EncryptSHA256(string text);
        string GenerateJWT(User user);
        Task<bool> ValidateJWT(string token);
    }
}