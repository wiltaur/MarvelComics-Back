using System.Runtime.ExceptionServices;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Services
{
    public class IdentificationTypeService(IRepository<IdentificationType> repository) : IIdentificationTypeService
    {
        public async Task<List<IdentificationType>> GetAllIdTypes(CancellationToken cancellationToken = default)
        {
            List<IdentificationType>? idTypes = [];
            try
            {
                idTypes = await repository.ListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return idTypes;
        }
    }
}