using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.Search;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class SearchMarketItemDescription
{
    [JsonPropertyName("appid")]
    public long AppId { get; set; }

    [JsonPropertyName("classid")]
    public string ClassId { get; set; }

    [JsonPropertyName("instanceid")]
    public long InstanceId { get; set; }

    [JsonPropertyName("background_color")]
    public string BackgroundColor { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }

    [JsonPropertyName("tradable")]
    public long Tradable { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("name_color")]
    public string NameColor { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("market_name")]
    public string MarketName { get; set; }

    [JsonPropertyName("market_hash_name")]
    public string MarketHashName { get; set; }

    [JsonPropertyName("commodity")]
    public long Commodity { get; set; }
}