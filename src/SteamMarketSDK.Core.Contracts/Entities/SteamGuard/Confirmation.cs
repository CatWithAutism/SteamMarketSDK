using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.SteamGuard;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class Confirmation
{
	public enum EMobileConfirmationType
	{
		Invalid = 0,
		Test = 1,
		Trade = 2,
		MarketListing = 3,
		FeatureOptOut = 4,
		PhoneNumberChange = 5,
		AccountRecovery = 6
	}

	[JsonPropertyName("accept")] public string Accept { get; set; }

	[JsonPropertyName("cancel")] public string Cancel { get; set; }

	[JsonPropertyName("type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public EMobileConfirmationType ConfType { get; set; } = EMobileConfirmationType.Invalid;

	[JsonPropertyName("creator_id")] public ulong Creator { get; set; }

	[JsonPropertyName("headline")] public string Headline { get; set; }

	[JsonPropertyName("icon")] public string Icon { get; set; }

	[JsonPropertyName("id")] public ulong Id { get; set; }

	[JsonPropertyName("nonce")] public ulong Key { get; set; }

	[JsonPropertyName("summary")] public List<String> Summary { get; set; }
}

public class ConfirmationsResponse
{
	[JsonPropertyName("conf")] public Confirmation[] Confirmations { get; set; }

	[JsonPropertyName("message")] public string Message { get; set; }

	[JsonPropertyName("needauth")] public bool NeedAuthentication { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }
}