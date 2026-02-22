using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.Distributed.Redis.Coordination;

/// <summary>
/// JSON converter for <see cref="IReadOnlySet{T}"/> of string, which System.Text.Json cannot deserialize by default.
/// </summary>
internal sealed class ReadOnlySetJsonConverter : JsonConverter<IReadOnlySet<string>>
{
    public override IReadOnlySet<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var list = JsonSerializer.Deserialize<List<string>>(ref reader, options) ?? [];
        return new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);
    }

    public override void Write(Utf8JsonWriter writer, IReadOnlySet<string> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.ToList(), options);
    }
}
