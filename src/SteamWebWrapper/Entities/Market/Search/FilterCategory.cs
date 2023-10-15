using System.Text.Json.Serialization;

namespace SteamWebWrapper.Entities.Market.Search;

public class FilterCategory
{
    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("localized_name")]
    public required string LocalizedName { get; set; }

    [JsonPropertyName("tags")]
    public required Dictionary<string, FilterItem> FilterItems { get; set; }
}