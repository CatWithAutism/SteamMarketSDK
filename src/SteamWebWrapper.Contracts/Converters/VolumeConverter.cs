using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Converters;

internal class VolumeConverter : JsonConverter<long>
{
    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var value = jsonDoc.RootElement.GetRawText().Replace(",", "").Replace("\"", "");
        
        return long.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}