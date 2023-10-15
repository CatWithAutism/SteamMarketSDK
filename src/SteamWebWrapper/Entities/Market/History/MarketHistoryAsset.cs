using System.Text.Json.Serialization;
using SteamWebWrapper.Converters;
using SteamWebWrapper.Entities.Market.Common;

namespace SteamWebWrapper.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryAsset
{
    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("contextid")]
    public long Contextid { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("classid")]
    public long Classid { get; set; }

    [JsonPropertyName("instanceid")]
    public long Instanceid { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("status")]
    public long Status { get; set; }

    [JsonPropertyName("original_amount")]
    public long OriginalAmount { get; set; }

    [JsonPropertyName("unowned_id")]
    public long UnownedId { get; set; }

    [JsonPropertyName("unowned_contextid")]
    public long UnownedContextid { get; set; }

    [JsonPropertyName("background_color")]
    public string BackgroundColor { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }

    [JsonPropertyName("icon_url_large")]
    public string IconUrlLarge { get; set; }

    [JsonPropertyName("descriptions")]
    public List<ItemDescription> Descriptions { get; set; }

    [JsonPropertyName("tradable")]
    [JsonConverter(typeof(BoolConverter))]
    public bool Tradable { get; set; }

    [JsonPropertyName("actions")]
    public List<SubjectAction> Actions { get; set; }

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

    [JsonPropertyName("market_actions")]
    public List<SubjectAction> MarketActions { get; set; }

    [JsonPropertyName("commodity")]
    public long Commodity { get; set; }

    [JsonPropertyName("market_tradable_restriction")]
    public long MarketTradableRestriction { get; set; }

    [JsonPropertyName("marketable")]
    [JsonConverter(typeof(BoolConverter))]
    public bool Marketable { get; set; }

    [JsonPropertyName("app_icon")]
    public string AppIcon { get; set; }

    [JsonPropertyName("owner")]
    public long Owner { get; set; }
}