using System.Text.Json;
using System.Text.Json.Serialization;

namespace TheFullStackTeam.Common.Converters;

public class GuidJsonConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var GuidString = reader.GetString();
            if (Guid.TryParse(GuidString, out var Guid))
            {
                return Guid;
            }
            throw new JsonException($"Invalid Guid format: {GuidString}");
        }
        throw new JsonException($"Unexpected token parsing Guid. Expected String, got {reader.TokenType}.");
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
