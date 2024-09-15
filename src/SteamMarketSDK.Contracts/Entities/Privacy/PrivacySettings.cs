using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Privacy;

public class PrivacySettings
{
	[JsonPropertyName("PrivacyFriendsList")]
	public PrivacyStatus PrivacyFriendsList { get; set; }

	[JsonPropertyName("PrivacyInventory")] public PrivacyStatus PrivacyInventory { get; set; }

	[JsonPropertyName("PrivacyInventoryGifts")]
	public PrivacyStatus PrivacyInventoryGifts { get; set; }

	[JsonPropertyName("PrivacyOwnedGames")]
	public PrivacyStatus PrivacyOwnedGames { get; set; }

	[JsonPropertyName("PrivacyPlaytime")] public PrivacyStatus PrivacyPlaytime { get; set; }

	[JsonPropertyName("PrivacyProfile")] public PrivacyStatus PrivacyProfile { get; set; }
}