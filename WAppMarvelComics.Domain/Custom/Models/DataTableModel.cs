using System.Text.Json.Serialization;

namespace WAppMarvelComics.Domain.Custom.Models
{
    public class DataTableModel<T>(List<T>? data)
    {
        [JsonPropertyName("items")]
        public List<T>? Items { get; set; } = data;

        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}