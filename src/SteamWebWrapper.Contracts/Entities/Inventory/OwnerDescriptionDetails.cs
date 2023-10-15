using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Inventory;

public class OwnerDescriptionDetails
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("color")]
    public string Color { get; set; }
}