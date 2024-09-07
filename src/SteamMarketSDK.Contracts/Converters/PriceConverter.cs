using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SteamMarketSDK.Contracts.Converters;

internal class PriceConverter : JsonConverter<float>
{
	public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var jsonDoc = JsonDocument.ParseValue(ref reader);
		var value = jsonDoc.RootElement.GetRawText();

		var result = Regex.Match(value, @"\d+[,\.]?\d*");
		if (result.Success)
		{
			return float.Parse(NormalizeString(result.Value), NumberStyles.Any, CultureInfo.InvariantCulture);
		}

		throw new InvalidCastException($"We cannot cast this {value} to float");
	}

	public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options) =>
		throw new NotImplementedException();

	private string NormalizeString(string str) => str.Replace(',', '.');
}