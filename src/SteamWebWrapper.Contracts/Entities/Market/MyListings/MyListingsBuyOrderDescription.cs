using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Entities.Common;

namespace SteamWebWrapper.Contracts.Entities.Market.MyListings;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyListingsBuyOrderDescription
{
    [JsonPropertyName("appid")]
    public long AppId { get; set; }

    [JsonPropertyName("classid")]
    public long ClassId { get; set; }

    [JsonPropertyName("instanceid")]
    public long InstanceId { get; set; }

    [JsonPropertyName("currency")]
    public long Currency { get; set; }

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
    public List<dynamic> OwnerDescriptions { get; set; }

    [JsonPropertyName("owner_actions")]
    public List<SubjectAction> OwnerActions { get; set; }

    [JsonPropertyName("fraudwarnings")]
    public List<dynamic> Fraudwarnings { get; set; }

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

    [JsonPropertyName("market_fee")]
    public dynamic MarketFee { get; set; }

    [JsonPropertyName("market_fee_app")]
    public dynamic MarketFeeApp { get; set; }

    [JsonPropertyName("contained_item")]
    public dynamic ContainedItem { get; set; }

    [JsonPropertyName("market_actions")]
    public List<SubjectAction> MarketActions { get; set; }

    [JsonPropertyName("commodity")]
    public long Commodity { get; set; }

    [JsonPropertyName("market_tradable_restriction")]
    public long MarketTradableRestriction { get; set; }

    [JsonPropertyName("market_marketable_restriction")]
    public dynamic MarketMarketableRestriction { get; set; }

    [JsonPropertyName("marketable")]
    public long Marketable { get; set; }

    [JsonPropertyName("tags")]
    public List<dynamic> Tags { get; set; }

    [JsonPropertyName("item_expiration")]
    public dynamic ItemExpiration { get; set; }

    [JsonPropertyName("market_buy_country_restriction")]
    public dynamic MarketBuyCountryRestriction { get; set; }

    [JsonPropertyName("market_sell_country_restriction")]
    public dynamic MarketSellCountryRestriction { get; set; }
}