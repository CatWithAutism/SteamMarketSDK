using System.Text.Json.Serialization;

namespace SteamWebWrapper.Entities.Market.Common;

public class ItemDescription
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("color")]
    public string Color { get; set; }
}