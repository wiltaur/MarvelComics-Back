using Newtonsoft.Json;

namespace WAppMarvelComics.Domain.Custom.Models
{
    public class ComicDetailModel: ComicModel
    {
        [JsonProperty("printPrice")]
        public float PrintPrice { get; set; }

        [JsonProperty("publishDate")]
        public string? PublishDate { get; set; }

        [JsonProperty("creators")]
        public ICollection<CreatorR> Creators { get; set; } = new List<CreatorR>();
    }

    public class CreatorR
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("role")]
        public string Role { get; set; } = string.Empty;
    }
}