using System.Text.Json.Serialization;

namespace WAppMarvelComics.API.Models.DTOs
{
    public class ApiResponseDto<T>(T data)
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = data;

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; } = true;

        [JsonPropertyName("returnMessage")]
        public string ReturnMessage { get; set; } = "";
    }
}