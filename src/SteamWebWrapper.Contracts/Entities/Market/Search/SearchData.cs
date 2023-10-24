using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.Search;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class SearchData
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("search_descriptions")]
    public bool SearchDescriptions { get; set; }

    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }

    [JsonPropertyName("pagesize")]
    public long PageSize { get; set; }

    [JsonPropertyName("prefix")]
    public string Prefix { get; set; }

    [JsonPropertyName("class_prefix")]
    public string ClassPrefix { get; set; }
}