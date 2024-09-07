using SteamMarketSDK.Common.Utils;
using SteamMarketSDK.Contracts.Entities.Market.MyHistory;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Converters;

internal class MyHistoryAssetFlatter : JsonConverter<List<MyHistoryAsset>>
{
	public override bool CanConvert(Type t) => t == typeof(List<MyHistoryAsset>);

	public override List<MyHistoryAsset> Read(ref Utf8JsonReader reader, Type typeToConvert,
		JsonSerializerOptions options)
	{
		using var jsonDoc = JsonDocument.ParseValue(ref reader);

		var value = jsonDoc.RootElement.GetRawText();
		if (value.IsNullOrEmpty() || value == "[]")
		{
			return new List<MyHistoryAsset>();
		}

		var data = JsonSerializer
			.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, MyHistoryAsset>>>>(value, options);
		if (data == null)
		{
			throw new ArgumentException($"We cannot serialize this data - {value}");
		}

		return data
			.SelectMany(
				gameContext => gameContext.Value.SelectMany(
					contextAsset => contextAsset.Value
						.Select(pair => pair.Value))).ToList();
	}

	public override void Write(Utf8JsonWriter writer, List<MyHistoryAsset> value, JsonSerializerOptions options) =>
		throw new NotImplementedException();
}