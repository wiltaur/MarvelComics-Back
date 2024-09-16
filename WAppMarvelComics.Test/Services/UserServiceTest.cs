using AutoFixture;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Moq;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Aggregates.UserAggregate.Specifications;
using WAppMarvelComics.Domain.Interfaces;
using WAppMarvelComics.Domain.Services;
using Xunit;

#nullable disable

namespace WAppMarvelComics.Test.Services
{
    public class UserServiceTest
    {
        Fixture _autoData;
        private UserService _currentDomain;
        private readonly Mock<IRepository<User>> _dRepository = new();
        private readonly Mock<ISecureUtilities> _dSecUtilities = new();
        private readonly Mock<IUnitOfWork> _dUnitOfWork = new();

        public UserServiceTest()
        {
            _autoData = new Fixture();
            _autoData.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _autoData.Behaviors.Remove(b));
            _autoData.Behaviors.Add(new OmitOnRecursionBehavior(1));

            _currentDomain = new(_dUnitOfWork.Object, _dRepository.Object, _dSecUtilities.Object);
        }

        [Fact]
        public async Task ValidateUserTest()
        {
            //Arrange
            var result = _autoData.Create<User>();

            var token = "UserToken";

            _dSecUtilities
                .Setup(_ => _.EncryptSHA256(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            _dSecUtilities
                .Setup(_ => _.GenerateJWT(It.IsAny<User>()))
                .Returns(token);

            _dRepository
                .Setup(_ => _.FirstOrDefaultAsync(It.IsAny<UserByEmailPwSpec>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            //Act 
            var response = await _currentDomain.ValidateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>());

            //Assert
            Assert.Equal(token, response);
        }

        [Fact]
        public async Task ValidateUserErrorTest()
        {
            //Arrange
            _dRepository
                .Setup(_ => _.FirstOrDefaultAsync(It.IsAny<UserByEmailPwSpec>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error"));

            //Act 
            try
            {
                await _currentDomain.ValidateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>());
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Equal("Error", ex.Message);
            }
        }

        [Fact]
        public async Task RegisterUserTest()
        {
            //Arrange
            var result = _autoData.Create<User>();

            _dUnitOfWork
                .Setup(_ => _.BeginTransactionAsync())
                .ReturnsAsync(It.IsAny<IDbContextTransaction>());

            _dUnitOfWork
                .Setup(_ => _.CommitAsync(It.IsAny<IDbContextTransaction>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _dSecUtilities
                .Setup(_ => _.EncryptSHA256(It.IsAny<string>()))
                .Returns(It.IsAny<string>());

            _dRepository
                .Setup(_ => _.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            //Act 
            var response = await _currentDomain.RegisterUser(result, It.IsAny<CancellationToken>());

            //Assert
            Assert.True(response);
        }

        [Fact]
        public async Task RegisterUserErrorTest()
        {
            //Arrange
            var result = _autoData.Create<User>();

            _dUnitOfWork
                .Setup(_ => _.BeginTransactionAsync())
                .ReturnsAsync(It.IsAny<IDbContextTransaction>());

            _dUnitOfWork
                .Setup(_ => _.RollbackAsync(It.IsAny<IDbContextTransaction>()))
                .Returns(Task.CompletedTask);

            _dSecUtilities
                .Setup(_ => _.EncryptSHA256(It.IsAny<string>()))
                 .Throws(new Exception("Error"));

            //Act 
            try
            {
                await _currentDomain.RegisterUser(result, It.IsAny<CancellationToken>());
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Equal("Error", ex.Message);
            }
        }
    }
}