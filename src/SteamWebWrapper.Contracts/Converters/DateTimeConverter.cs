using System.Text.Json;
using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Converters;

internal class DateTimeConverter : JsonConverter<DateTime>
{
    public override bool CanConvert(Type t) => t == typeof(DateTime);

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var value = jsonDoc.RootElement.GetInt64();
        return DateTimeOffset.FromUnixTimeSeconds(value).DateTime;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}