using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Entities.Common;

namespace SteamWebWrapper.Contracts.Entities.Market.MyListings;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyListingsAsset
{
    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long AppId { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("classid")]
    public long ClassId { get; set; }

    [JsonPropertyName("instanceid")]
    public long InstanceId { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("status")]
    public long Status { get; set; }

    [JsonPropertyName("original_amount")]
    public long OriginalAmount { get; set; }

    [JsonPropertyName("unowned_id")]
    public long UnownedId { get; set; }

    [JsonPropertyName("unowned_contextid")]
    public long UnownedContextId { get; set; }

    [JsonPropertyName("background_color")]
    public string BackgroundColor { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }

    [JsonPropertyName("icon_url_large")]
    public string IconUrlLarge { get; set; }

    [JsonPropertyName("descriptions")]
    public List<SubjectDescription> Descriptions { get; set; }

    [JsonPropertyName("tradable")]
    public long Tradable { get; set; }

    [JsonPropertyName("actions")]
    public List<SubjectAction> Actions { get; set; }

    [JsonPropertyName("owner_descriptions")]
    public List<SubjectDescription> OwnerDescriptions { get; set; }

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
    public long Marketable { get; set; }
    
    [JsonPropertyName("app_icon")]
    public Uri AppIcon { get; set; }
    
    [JsonPropertyName("owner")]
    public long? Owner { get; set; }
}