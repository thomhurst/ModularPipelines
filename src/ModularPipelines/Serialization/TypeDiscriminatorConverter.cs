using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Serialization;

internal class TypeDiscriminatorConverter<T> : JsonConverter<T>
    where T : ITypeDiscriminator
{
    private readonly IEnumerable<Type> _types;

    public TypeDiscriminatorConverter()
    {
        var type = typeof(T);

        _types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p is { IsClass: true, IsAbstract: false })
            .ToList();
    }

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

        var type = _types.FirstOrDefault(x => x.FullName == typeProperty.GetString());

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