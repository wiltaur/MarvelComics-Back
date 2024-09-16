using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Aggregates.ComicAggregate;
using WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate;
using WAppMarvelComics.Domain.Aggregates.UserAggregate.Specifications;
using WAppMarvelComics.Domain.Custom;
using WAppMarvelComics.Domain.Custom.Models;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Services
{
    public class ComicService(
        IOptions<SettingModel> setting,
        HttpClient httpClient,
        IUnitOfWork unitOfWork,
        IRepository<ComicFavorite> repComicFav,
        IRepository<Comic> repComic,
        IRepository<User> repUser) : IComicService
    {
        public async Task<DataTableModel<ComicModel>> GetComics(int limit, int offset, CancellationToken cancellationToken)
        {
            var builder = new UriBuilder(setting.Value.MarvelGatewayUrl);
            var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
            collection.Add("limit", $"{limit}");
            collection.Add("offset", $"{offset}");
            collection.Add("ts", setting.Value.MarvelGatewayTimestamp);
            collection.Add("apikey", setting.Value.MarvelGatewayApiKey);
            collection.Add("hash", setting.Value.MarvelGatewayHash);

            builder.Query = collection.ToString();

            HttpRequestMessage requestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = builder.Uri
            };

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancellationToken);

            var readContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var comicResponse = JsonConvert.DeserializeObject<ComicResponse>(readContent);

            if (comicResponse != null && comicResponse.Data != null)
            {
                var config = AutoMapperGenericsHelper.InitializeAutomapper();
                var mapper = new Mapper(config);

                var comics = mapper.Map<IEnumerable<Result>?, IEnumerable<ComicModel>?>(comicResponse.Data.Results)?.ToList();

                var dataTableResponse = new DataTableModel<ComicModel>(comics);

                dataTableResponse.Total = Convert.ToInt32(comicResponse.Data.Total);
                dataTableResponse.Offset = Convert.ToInt32(comicResponse.Data.Offset);
                dataTableResponse.Limit = Convert.ToInt32(comicResponse.Data.Limit);
                
                return dataTableResponse;
            }
            else
            {
                var comics = new List<ComicModel>();
                return new DataTableModel<ComicModel>(comics);
            }
        }

        public async Task<ComicDetailModel> GetComicById(int comicId, CancellationToken cancellationToken)
        {
            var comic = new ComicDetailModel();
            var builder = new UriBuilder($"{setting.Value.MarvelGatewayUrl}/{comicId}");
            var collection = System.Web.HttpUtility.ParseQueryString(string.Empty);
            collection.Add("ts", setting.Value.MarvelGatewayTimestamp);
            collection.Add("apikey", setting.Value.MarvelGatewayApiKey);
            collection.Add("hash", setting.Value.MarvelGatewayHash);

            builder.Query = collection.ToString();

            HttpRequestMessage requestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = builder.Uri
            };

            HttpResponseMessage response = await httpClient.SendAsync(requestMessage, cancellationToken);

            var readContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var comicResponse = JsonConvert.DeserializeObject<ComicResponse>(readContent);

            if (comicResponse != null && comicResponse.Data != null)
            {
                var config = AutoMapperGenericsHelper.InitializeAutomapper();
                var mapper = new Mapper(config);
                comic = mapper.Map<Result?, ComicDetailModel>(comicResponse.Data.Results?.FirstOrDefault());
            }
            return comic;
        }

        public async Task<bool> AddFavoriteComic(Comic comic, string emailUser, CancellationToken cancellationToken)
        {
            bool result = false;
            var transaction = await unitOfWork.BeginTransactionAsync();

            try
            {
                var user = await repUser.FirstOrDefaultAsync(new UserByEmailSpec(emailUser), cancellationToken);
                if (user != null)
                {
                    var exitComic = await repComic.GetByIdAsync(comic.Id, cancellationToken);

                    if (exitComic is null)
                        await repComic.AddAsync(comic, cancellationToken);

                    ComicFavorite comicFavorite = new()
                    {
                        Id = comic.Id,
                        IdTypeUser = user.IdType,
                        IdUser = user.IdUser
                    };
                    var comicFavResp = await repComicFav.AddAsync(comicFavorite, cancellationToken);

                    if (comicFavResp != null)
                        result = true;

                    await unitOfWork.CommitAsync(transaction, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackAsync(transaction);
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return result;
        }

        public async Task<List<Comic>> GetFavoriteComics(string emailUser, CancellationToken cancellationToken)
        {
            List<Comic> favComics = [];

            try
            {
                var user = await repUser.FirstOrDefaultAsync(new UserByEmailSpec(emailUser), cancellationToken);
                if (user != null && user.ComicFavorites.Count != 0)
                {
                    favComics.AddRange(await CreateFavoriteList(user.ComicFavorites, cancellationToken));
                }
            }
            catch (Exception ex)
            {
                ExceptionDispatchInfo.Capture(ex).Throw();
            }
            return favComics;
        }

        private async Task<List<Comic>> CreateFavoriteList(ICollection<ComicFavorite> comicFavorites, CancellationToken cancellationToken = default)
        {
            List<Comic> comics = [];
            foreach (var comicFav in comicFavorites)
            {
                var exitComic = await repComic.GetByIdAsync(comicFav.Id, cancellationToken);
                if (exitComic != null)
                {
                    comics.Add(exitComic);
                }
            }
            return comics;
        }
    }
}