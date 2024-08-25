using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using WAppMarvelComics.API.Models.DTOs;
using WAppMarvelComics.Domain.Aggregates.ComicAggregate;
using WAppMarvelComics.Domain.Custom;
using WAppMarvelComics.Domain.Custom.Models;
using WAppMarvelComics.Domain.Interfaces;
using WAppMarvelComics.Domain.Services;

namespace WAppMarvelComics.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ComicController(IComicService comicService) : ControllerBase
    {
        /// <summary>
        /// Method to get all comics with images.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<DataTableModel<ComicModel>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<string>))]
        public async Task<IActionResult> GetAll([FromQuery] int limit, [FromQuery] int offset, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var comics = await comicService.GetComics(limit, offset, cancellationToken);
                var response = new ApiResponseDto<DataTableModel<ComicModel>?>(comics)
                {
                    ReturnMessage = "Data generated."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ApiResponseDto<string>(message)
                {
                    IsSuccess = false,
                    ReturnMessage = $"System error: " + message
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Method to get a comic by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<ComicDetailModel>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<string>))]
        public async Task<IActionResult> GetById([FromQuery] int id, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var comic = await comicService.GetComicById(id, cancellationToken);
                var response = new ApiResponseDto<ComicDetailModel>(comic)
                {
                    ReturnMessage = "Data generated."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ApiResponseDto<string>(message)
                {
                    IsSuccess = false,
                    ReturnMessage = $"System error: " + message
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Method to get all favorites comics.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<List<ComicDto>?>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<List<ComicDto>?>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<bool>))]
        public async Task<IActionResult> GetFavorites(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var claims = HttpContext.User.Claims;
                string email = claims.First(claim => claim.Type == ClaimTypes.Email).Value;

                var comics = await comicService.GetFavoriteComics(email, cancellationToken);

                if(comics != null && comics.Count != 0)
                {
                    var json = JsonConvert.SerializeObject(comics, Formatting.Indented,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }
                    );
                    var favComics = json.SecureDeserializeObject<List<ComicDto>>();

                    var response = new ApiResponseDto<List<ComicDto>?>(favComics)
                    {
                        ReturnMessage = "Data generated."
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponseDto<List<ComicDto>?>([])
                    {
                        IsSuccess = false,
                        ReturnMessage = "There are no favorites comics for the user."
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ApiResponseDto<bool>(false)
                {
                    IsSuccess = false,
                    ReturnMessage = $"System error: " + message
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Method to add a favorite comic.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<bool>))]
        public async Task<IActionResult> AddFavorite([FromBody] ComicDto request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var claims = HttpContext.User.Claims;
                string email = claims.First(claim => claim.Type == ClaimTypes.Email).Value;

                string json = request.SecureSerializeObject();

                var comic = json.SecureDeserializeObject<Comic>();
                bool result = false;

                if (comic != null)
                {
                    result = await comicService.AddFavoriteComic(comic, email, cancellationToken);
                }

                var response = new ApiResponseDto<bool>(result)
                {
                    IsSuccess = result
                };

                if (result)
                {
                    response.ReturnMessage = "Favorite comic added.";
                    return Ok(response);
                }
                else
                {
                    response.ReturnMessage = "Failed to add favorite comic.";
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ApiResponseDto<bool>(false)
                {
                    IsSuccess = false,
                    ReturnMessage = $"System error: " + message
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}