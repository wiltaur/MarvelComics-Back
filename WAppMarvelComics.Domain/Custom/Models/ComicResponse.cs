using WAppMarvelComics.Domain.Aggregates.ComicFavoriteAggregate;

namespace WAppMarvelComics.Domain.Custom.Models
{
    public class ComicResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Copyright { get; set; } = string.Empty;
        public string AttributionText { get; set; } = string.Empty;
        public string AttributionHTML { get; set; } = string.Empty;
        public Data? Data { get; set; }
        public string Etag { get; set; } = string.Empty;
    }

    public class Data
    {
        public string Offset { get; set; } = string.Empty;
        public string Limit { get; set; } = string.Empty;
        public string Total { get; set; } = string.Empty;
        public List<Result>? Results { get; set; }
    }

    public class Result
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VariantDescription { get; set; } = string.Empty;
        public Thumbnail? Thumbnail { get; set; }
        public virtual ICollection<PriceR> Prices { get; set; } = new List<PriceR>();
        public virtual ICollection<DateR> Dates { get; set; } = new List<DateR>();
        public Creator? Creators { get; set; }
    }

    public class Thumbnail
    {
        public string Path { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
    }

    public class PriceR
    {
        public string Type { get; set; } = string.Empty;
        public float Price { get; set; }
    }

    public class DateR
    {
        public string Type { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }

    public class Creator
    {
        public int Available { get; set; }
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }

    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

    }
}