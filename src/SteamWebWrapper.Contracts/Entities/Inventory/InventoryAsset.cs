using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Inventory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class InventoryAsset
{
    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("assetid")]
    public string AssetId { get; set; }

    [JsonPropertyName("classid")]
    public string ClassId { get; set; }

    [JsonPropertyName("instanceid")]
    public long InstanceId { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }
}