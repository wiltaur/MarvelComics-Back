using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Aggregates.ComicAggregate;
using WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate;
using WAppMarvelComics.Domain.Custom.Models;
using WAppMarvelComics.Domain.Interfaces;
using WAppMarvelComics.Domain.Services;
using Xunit;

#nullable disable

namespace WAppMarvelComics.Test.Services
{
    public class ComicServiceTest
    {
        Fixture _autoData;
        private ComicService _currentDomain;
        private readonly Mock<IRepository<User>> _dRepoUser = new();
        private readonly Mock<IRepository<Comic>> _dRepoComic = new();
        private readonly Mock<IRepository<ComicFavorite>> _dRepoComicFav = new();
        private readonly Mock<IUnitOfWork> _dUnitOfWork = new();
        private readonly Mock<IOptions<SettingModel>> _dSettings = new();
        private readonly Mock<HttpMessageHandler> _httpClient = new();

        public ComicServiceTest()
        {
            _autoData = new Fixture();
            _autoData.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _autoData.Behaviors.Remove(b));
            _autoData.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task GetComicsTest(int index, bool expected)
        {
            //Arrange
            var setting = _autoData.Create<SettingModel>();

            setting.MarvelGatewayUrl = "https://example.com/api/stuff";

            _dSettings.Setup(s => s.Value).Returns(setting);

            // Configurar el comportamiento esperado del HttpClient mock
            ComicResponse result;

            var expectedData = String.Empty;

            switch (index)
            {
                case 1:
                    result = _autoData.Create<ComicResponse>();
                    result.Data.Total = result.Data.Results.Count.ToString();
                    result.Data.Offset = "1";
                    result.Data.Limit = "1";

                    expectedData = JsonSerializer.Serialize(result);
                    break;
                case 2:
                    result = new();
                    expectedData = JsonSerializer.Serialize(result);
                    break;
            }

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedData)
            };

            _httpClient.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var client = new HttpClient(_httpClient.Object);

            _currentDomain = new(_dSettings.Object, client, _dUnitOfWork.Object, _dRepoComicFav.Object, _dRepoComic.Object, _dRepoUser.Object);

            //Act 
            var response = await _currentDomain.GetComics(1, 1, It.IsAny<CancellationToken>());

            var respFinal = response != null && response.Total > 0;

            //Assert
            Assert.Equal(expected, respFinal);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task GetComicByIdTest(int index, bool expected)
        {
            //Arrange
            var setting = _autoData.Create<SettingModel>();

            setting.MarvelGatewayUrl = "https://example.com/api/stuff";

            _dSettings.Setup(s => s.Value).Returns(setting);

            // Configurar el comportamiento esperado del HttpClient mock
            ComicResponse result;

            var expectedData = String.Empty;

            switch (index)
            {
                case 1:
                    result = _autoData.Create<ComicResponse>();
                    result.Data.Total = result.Data.Results.Count.ToString();
                    result.Data.Offset = "1";
                    result.Data.Limit = "1";

                    expectedData = JsonSerializer.Serialize(result);
                    break;
                case 2:
                    result = new();
                    expectedData = JsonSerializer.Serialize(result);
                    break;
            }

            var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedData)
            };

            _httpClient.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            var client = new HttpClient(_httpClient.Object);

            _currentDomain = new(_dSettings.Object, client, _dUnitOfWork.Object, _dRepoComicFav.Object, _dRepoComic.Object, _dRepoUser.Object);

            //Act 
            var response = await _currentDomain.GetComicById(1, It.IsAny<CancellationToken>());

            var respFinal = response != null && !string.IsNullOrEmpty(response.Title);

            //Assert
            Assert.Equal(expected, respFinal);
        }
    }
}