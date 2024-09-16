using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Interfaces;
using WAppMarvelComics.Domain.Services;
using Xunit;

#nullable disable

namespace WAppMarvelComics.Test.Services
{
    public class IdTypeServiceTest
    {
        Fixture _autoData;
        private IdentificationTypeService _currentDomain;
        private readonly Mock<IRepository<IdentificationType>> _dRepository = new();

        public IdTypeServiceTest()
        {
            _autoData = new Fixture();
            _autoData.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _autoData.Behaviors.Remove(b));
            _autoData.Behaviors.Add(new OmitOnRecursionBehavior(1));

            _currentDomain = new(_dRepository.Object);
        }

        [Fact]
        public async Task GetAllIdTypesTest()
        {
            //Arrange
            var result = _autoData.Create<List<IdentificationType>>();
            var cToken = _autoData.Create<CancellationToken>();

            _dRepository
                .Setup(_ => _.ListAsync(cToken))
                .ReturnsAsync(result);

            //Act 
            var response = await _currentDomain.GetAllIdTypes(cToken);

            //Assert
            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetAllIdTypesErrorTest()
        {
            //Arrange
            var cToken = _autoData.Create<CancellationToken>();

            _dRepository
                .Setup(_ => _.ListAsync(cToken))
                .ThrowsAsync(new Exception("Error"));

            //Act 
            try
            {
                await _currentDomain.GetAllIdTypes(cToken);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.Equal("Error", ex.Message);
            }
        }
    }
}
