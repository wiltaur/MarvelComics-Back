using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WAppMarvelComics.Domain.Aggregates.ComicAggregate;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate
{
    public class ComicFavorite : IAggregateRoot
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Key]
        [MaxLength(50)]
        [JsonProperty("idUser")]
        public string IdUser { get; set; } = null!;

        [Key]
        [MaxLength(4)]
        [JsonProperty("idTypeUser")]
        public string IdTypeUser { get; set; } = null!;

        public virtual User User { get; set; } = null!;
        public virtual Comic Comic { get; set; } = null!;
    }
}