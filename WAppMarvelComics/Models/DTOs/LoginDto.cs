using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WAppMarvelComics.API.Models.DTOs
{
    public class LoginDto
    {
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