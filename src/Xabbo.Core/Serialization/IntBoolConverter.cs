using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xabbo.Core.Serialization;

internal class IntBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number => reader.GetInt32() != 0,
            JsonTokenType.String => reader.GetString() switch
            {
                "1" or "true" => true,
                "0" or "false" or "" or null => false,
                _ => throw new JsonException("Invalid bool value.")
            },
            _ => throw new JsonException("Invalid bool token.")
        };
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}
