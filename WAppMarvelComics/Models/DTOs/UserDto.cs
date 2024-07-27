using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WAppMarvelComics.API.Models.DTOs
{
    public class UserDto
    {
        [Required]
        [MaxLength(50)]
        [JsonPropertyName("idUser")]
        public string IdUser { get; set; } = null!;

        [Required]
        [MaxLength(4)]
        [JsonPropertyName("idType")]
        public string IdType { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}