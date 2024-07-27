using Newtonsoft.Json;

namespace WAppMarvelComics.API.Models.DTOs
{
    public class IdTypesResponseDto
    {
        [JsonProperty("idType")]
        public string IdType { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}