using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Common;

public class SubjectDescription
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}