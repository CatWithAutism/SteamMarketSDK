using System.Text.Json.Serialization;

namespace SteamWebWrapper.Entities.Market;

public class FilterResposne
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("facets")]
    public Facets Facets { get; set; }
}