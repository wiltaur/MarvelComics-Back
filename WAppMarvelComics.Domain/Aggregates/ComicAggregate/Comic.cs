using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Aggregates.ComicAggregate
{
    public class Comic : IAggregateRoot
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("description")]
        public string? Description { get; set; }

        [MaxLength(250)]
        [JsonProperty("image")]
        public string? Image { get; set; }

        public virtual ICollection<ComicFavorite> ComicFavorites { get; set; } = new List<ComicFavorite>();
    }
}