namespace WAppMarvelComics.Domain.Custom.Models
{
    public sealed class SettingModel
    {
        public string JwtKey { get; set; } = null!;
        public string MarvelGatewayUrl { get; set; } = null!;
        public string MarvelGatewayApiKey { get; set; } = null!;
        public string MarvelGatewayTimestamp { get; set; } = null!;
        public string MarvelGatewayHash { get; set; } = null!;
    }
}