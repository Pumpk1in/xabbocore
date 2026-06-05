using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xabbo.Core.Serialization;

internal class StringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            // Habbo's JSON exporter emits unquoted values for content that looks numeric, including
            // hex colour codes that happen to end in 'E' followed by digits (e.g. "4481E8" is exported
            // as 4.481E8 in scientific notation). Read the raw token text and strip the artificial
            // decimal point to recover the original hex representation.
            byte[] raw = reader.HasValueSequence
                ? BuffersExtensions.ToArray(reader.ValueSequence)
                : reader.ValueSpan.ToArray();

            string text = Encoding.UTF8.GetString(raw);
            int dot = text.IndexOf('.');
            return dot < 0 ? text : text.Remove(dot, 1);
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
