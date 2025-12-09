using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularPipelines.OptionsGenerator.TypeDetection;

/// <summary>
/// JSON converter for CliOptionType enum.
/// Uses the centralized CliTypeMapper for consistent type parsing.
/// </summary>
public class CliOptionTypeConverter : JsonConverter<CliOptionType>
{
    public override CliOptionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return CliTypeMapper.FromTypeName(value);
    }

    public override void Write(Utf8JsonWriter writer, CliOptionType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(CliTypeMapper.ToTypeName(value));
    }
}
