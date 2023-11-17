using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.ItemOrdersActivity;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class Activity
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActivityType Type { get; set; }

    [JsonPropertyName("quantity")]
    public long Quantity { get; set; }

    [JsonPropertyName("price")]
    [JsonConverter(typeof(PriceConverter))]
    public float Price { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }

    [JsonPropertyName("avatar_buyer")]
    public string AvatarBuyer { get; set; }

    [JsonPropertyName("avatar_medium_buyer")]
    public string AvatarMediumBuyer { get; set; }

    [JsonPropertyName("persona_buyer")]
    public string PersonaBuyer { get; set; }

    [JsonPropertyName("avatar_seller")]
    public string AvatarSeller { get; set; }

    [JsonPropertyName("avatar_medium_seller")]
    public string AvatarMediumSeller { get; set; }

    [JsonPropertyName("persona_seller")]
    public string PersonaSeller { get; set; }
}