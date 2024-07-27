using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Aggregates;

public class User: IAggregateRoot
{
    [Key]
    [MaxLength(50)]
    [JsonProperty("idUser")]
    public string IdUser { get; set; } = null!;

    [Key]
    [MaxLength(4)]
    [JsonProperty("idType")]
    public string IdType { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [JsonProperty("email")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [JsonProperty("password")]
    public string Password { get; set; } = null!;

    public virtual IdentificationType IdTypeNavigation { get; set; } = null!;
}