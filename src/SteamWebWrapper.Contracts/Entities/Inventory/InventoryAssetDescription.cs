using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;
using SteamWebWrapper.Contracts.Entities.Common;

namespace SteamWebWrapper.Contracts.Entities.Inventory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class InventoryAssetDescription
{
    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("classid")]
    public long ClassId { get; set; }

    [JsonPropertyName("instanceid")]
    public long Instanceid { get; set; }

    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("background_color")]
    public string BackgroundColor { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("icon_url_large")]
    public string IconUrlLarge { get; set; }

    [JsonPropertyName("descriptions")]
    public List<OwnerDescriptionDetails> Descriptions { get; set; }

    [JsonPropertyName("tradable")]
    [JsonConverter(typeof(BoolConverter))]
    public bool Tradable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("actions")]
    public List<SubjectAction> Actions { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("owner_descriptions")]
    public List<OwnerDescriptionDetails> OwnerDescriptions { get; set; }

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("market_actions")]
    public List<SubjectAction> MarketActions { get; set; }

    [JsonPropertyName("commodity")]
    public long Commodity { get; set; }

    [JsonPropertyName("market_tradable_restriction")]
    public long MarketTradableRestriction { get; set; }

    [JsonPropertyName("marketable")]
    [JsonConverter(typeof(BoolConverter))]
    public bool Marketable { get; set; }

    [JsonPropertyName("tags")]
    public List<Tag> Tags { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("market_buy_country_restriction")]
    public string MarketBuyCountryRestriction { get; set; }
}