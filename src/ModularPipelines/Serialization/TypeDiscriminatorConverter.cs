using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Serialization;

internal class TypeDiscriminatorConverter<T> : JsonConverter<T>
    where T : ITypeDiscriminator
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);

        if (!jsonDocument.RootElement.TryGetProperty(nameof(ITypeDiscriminator.TypeDiscriminator), out var typeProperty))
        {
            throw new JsonException();
        }

        var type = Type.GetType(typeProperty.GetString()!);

        if (type == null)
        {
            throw new JsonException();
        }

        var result = (T) jsonDocument.Deserialize(type, GetOptions(options))!;

        return result;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object) value, GetOptions(options));
    }

    private JsonSerializerOptions GetOptions(JsonSerializerOptions options)
    {
        var newOptions = new JsonSerializerOptions(options);

        newOptions.Converters.Remove(this);

        return newOptions;
    }
}