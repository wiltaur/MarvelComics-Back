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
    public class UserController(IUserService userService) : ControllerBase
    {

        /// <summary>
        /// Method to login and obtain the token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<string>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<string>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<string>))]
        public async Task<IActionResult> ValidateUser([FromBody] LoginDto request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                string tokenResponse = await userService.ValidateUser(request.Email, request.Password, cancellationToken);

                if (string.IsNullOrEmpty(tokenResponse))
                {
                    var responseError = new ApiResponseDto<string>(tokenResponse)
                    {
                        IsSuccess = false,
                        ReturnMessage = $"Email or password incorrect."
                    };
                    return BadRequest(responseError);
                }
                else
                {
                    var responseOk = new ApiResponseDto<string>(tokenResponse)
                    {
                        IsSuccess = true,
                        ReturnMessage = $"Logged."
                    };
                    return Ok(responseOk);
                }
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
        /// Method to register an user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponseDto<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponseDto<bool>))]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto request, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                string json = request.SecureSerializeObject();

                var user = json.SecureDeserializeObject<User>();

                bool resultRegister = false;

                if (user != null)
                {
                    resultRegister = await userService.RegisterUser(user, cancellationToken);
                }

                var response = new ApiResponseDto<bool>(resultRegister)
                {
                    IsSuccess = resultRegister
                };

                if (resultRegister)
                {
                    response.ReturnMessage = "User Registered.";
                    return Ok(response);
                }
                else
                {
                    response.ReturnMessage = "Failed to register user.";
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