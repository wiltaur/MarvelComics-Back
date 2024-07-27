using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WAppMarvelComics.API.Models.DTOs;
using WAppMarvelComics.Domain.Aggregates;
using WAppMarvelComics.Domain.Custom;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentificationTypeController(IIdentificationTypeService idTypeService) : ControllerBase
    {

        /// <summary>
        /// Method to register an user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<List<IdTypesResponseDto>?>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<string>))]
        public async Task<IActionResult> GetIdTypes(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var idTypes = await idTypeService.GetAllIdTypes(cancellationToken);

                if (idTypes.Count != 0)
                {
                    string json = idTypes.SecureSerializeObject();

                    var idTypesResp = json.SecureDeserializeObject<List<IdTypesResponseDto>>();

                    var response = new ApiResponseDto<List<IdTypesResponseDto>?>(idTypesResp)
                    {
                        IsSuccess = true,
                        ReturnMessage = $"Identification Types generated."
                    };
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponseDto<bool>(false)
                    {
                        IsSuccess = false,
                        ReturnMessage = $"Not found Identification Types."
                    };
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                var response = new ApiResponseDto<string>(message)
                {
                    IsSuccess = false,
                    ReturnMessage = $"System error."
                };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }
}