using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WAppMarvelComics.API.Models.DTOs
{
    public class ComicDto
    {
        [Required]
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [Required]
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("image")]
        public string? Image { get; set; }
    }
}