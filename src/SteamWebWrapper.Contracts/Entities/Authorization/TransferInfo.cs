using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

public class TransferInfo
{
    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("params")]
    public Params Params { get; set; }
}