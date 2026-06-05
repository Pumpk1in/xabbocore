using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Xabbo.Core.Serialization;

/// <summary>
/// Reads a JSON value that may be either a single object or an array of objects into a <see cref="List{T}"/>.
/// Habbo's JSON exporter collapses single-element lists into the bare object, so this converter restores them.
/// </summary>
internal sealed class SingleOrArrayConverter<T> : JsonConverter<List<T>>
{
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return [];

        var itemInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            List<T> list = [];
            reader.Read();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                T? item = JsonSerializer.Deserialize(ref reader, itemInfo);
                if (item is not null)
                    list.Add(item);
                reader.Read();
            }
            return list;
        }

        T? single = JsonSerializer.Deserialize(ref reader, itemInfo);
        return single is null ? [] : [single];
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        var itemInfo = (JsonTypeInfo<T>)options.GetTypeInfo(typeof(T));
        writer.WriteStartArray();
        foreach (var item in value)
            JsonSerializer.Serialize(writer, item, itemInfo);
        writer.WriteEndArray();
    }
}
