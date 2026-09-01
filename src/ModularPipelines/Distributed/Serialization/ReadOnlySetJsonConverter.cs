using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Serialization;

/// <summary>
/// Serializes read-only capability sets as plain JSON string arrays.
/// </summary>
public sealed class ReadOnlySetJsonConverter : JsonConverter<IReadOnlySet<Capability>>
{
    /// <inheritdoc />
    public override IReadOnlySet<Capability> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException("Expected a JSON array.");
        }

        var values = new List<Capability>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Expected a string value.");
            }

            values.Add(new Capability(reader.GetString()!));
        }

        if (reader.TokenType != JsonTokenType.EndArray)
        {
            throw new JsonException("Unexpected end of JSON array.");
        }

        return values.ToFrozenSet();
    }

    /// <inheritdoc />
    public override void Write(
        Utf8JsonWriter writer,
        IReadOnlySet<Capability> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var item in value)
        {
            writer.WriteStringValue(item.Name);
        }

        writer.WriteEndArray();
    }
}
