using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Common;

public class SubjectAction
{
    [JsonPropertyName("link")]
    public string Link { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}