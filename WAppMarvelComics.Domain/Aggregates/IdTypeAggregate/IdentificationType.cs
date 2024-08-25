
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WAppMarvelComics.Domain.Interfaces;

namespace WAppMarvelComics.Domain.Aggregates;

public partial class IdentificationType: IAggregateRoot
{
    [Key]
    [MaxLength(4)]
    [JsonProperty("idType")]
    public string IdType { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [NotMapped]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}