using Microsoft.AspNetCore.Mvc;
using System.Net;
using WAppMarvelComics.API.Models.DTOs;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController(ISecureUtilities secureUtilities) : ControllerBase
    {

        /// <summary>
        /// Method to validate the token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<bool>))]
        public async Task<IActionResult> ValidateToken([FromQuery] string token)
        {
            bool tokenResponse = await secureUtilities.ValidateJWT(token);

            var response = new ApiResponseDto<bool>(tokenResponse)
            {
                IsSuccess = tokenResponse
            };

            if (tokenResponse)
            {
                response.ReturnMessage = $"Token correct.";
                return Ok(response);
            }
            else
            {
                response.ReturnMessage = $"Token incocorrect.";
                return BadRequest(response);
            }
        }
    }
}