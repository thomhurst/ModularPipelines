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
            throw new JsonException(
                $"Expected JSON object start token but found '{reader.TokenType}' when deserializing type '{typeToConvert.FullName}'.");
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);

        if (!jsonDocument.RootElement.TryGetProperty(nameof(ITypeDiscriminator.TypeDiscriminator), out var typeProperty))
        {
            throw new JsonException(
                $"Missing required property '{nameof(ITypeDiscriminator.TypeDiscriminator)}' when deserializing type '{typeToConvert.FullName}'. " +
                $"Ensure the JSON contains a type discriminator property.");
        }

        var typeDiscriminatorValue = typeProperty.GetString();
        var type = Type.GetType(typeDiscriminatorValue!);

        if (type == null)
        {
            throw new JsonException(
                $"Could not resolve type from discriminator value '{typeDiscriminatorValue}' when deserializing type '{typeToConvert.FullName}'. " +
                $"Ensure the type exists and is properly qualified.");
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