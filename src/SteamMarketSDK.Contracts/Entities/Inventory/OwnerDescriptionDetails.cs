using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Inventory;

public class OwnerDescriptionDetails
{
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("color")]
	public string Color { get; set; }

	[JsonPropertyName("type")] public string Type { get; set; }

	[JsonPropertyName("value")] public string Value { get; set; }
}