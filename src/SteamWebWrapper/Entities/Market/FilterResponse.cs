using System.Text.Json.Serialization;

namespace SteamWebWrapper.Entities.Market;

public class FilterResponse
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("facets")]
    public required FilterCategory FilterCategory { get; set; }
}