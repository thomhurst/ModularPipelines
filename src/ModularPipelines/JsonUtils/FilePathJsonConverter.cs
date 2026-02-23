using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.JsonUtils;

[ExcludeFromCodeCoverage]
public class FilePathJsonConverter : JsonConverter<File>
{
    public override File? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return new File(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, File value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Path);
    }
}
