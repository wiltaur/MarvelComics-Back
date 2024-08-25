using Newtonsoft.Json;

namespace WAppMarvelComics.Domain.Custom.Models
{
    public class ComicModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("image")]
        public string Image { get; set; } = null!;
    }
}