using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace SteamWebWrapper.Contracts.Converters;

internal class PriceConverter : JsonConverter<float>
{
    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var value = jsonDoc.RootElement.GetRawText();

        var result = Regex.Match(value, @"\d+,\d+|\d+");
        if (result.Success)
        {
            return float.Parse(result.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        throw new InvalidCastException($"We cannot cast this {value} to float");
    }
}