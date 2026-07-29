using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Serializes case-insensitive read-only string sets used by distributed messages.
/// </summary>
public sealed class ReadOnlySetJsonConverter : JsonConverter<IReadOnlySet<string>>
{
    /// <inheritdoc />
    public override IReadOnlySet<string> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a JSON array.");
        }

        var values = new List<string>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string value.");
            }

            values.Add(reader.GetString()!);
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Unexpected end of JSON array.");
        }

        return values.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        IReadOnlySet<string> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
    }
}
