using System.Runtime.ExceptionServices;
using System.Threading;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Aggregates.UserAggregate.Specifications;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Services
{
    public class UserService(IUnitOfWork unitOfWork, IRepository<User> repository, ISecureUtilities secureUtilities) : IUserService
    {
        public async Task<string> ValidateUser(string email, string password, CancellationToken cancellationToken = default)
        {
            string result = string.Empty;
            try
            {
                var userLogin = await repository.FirstOrDefaultAsync(new UserByEmailPwSpec(email, secureUtilities.EncryptSHA256(password)), cancellationToken);

                if (userLogin != null)
                    result = secureUtilities.GenerateJWT(userLogin);
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return result;
        }

        public async Task<bool> RegisterUser(User user, CancellationToken cancellationToken = default)
        {
            bool result = false;
            var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                string passEncrypt = secureUtilities.EncryptSHA256(user.Password);
                user.Password = passEncrypt;
                var userLogin = await repository.AddAsync(user, cancellationToken);

                if (userLogin != null)
                    result = true;

                await unitOfWork.CommitAsync(transaction, cancellationToken);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync(transaction);
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return result;
        }
    }
}