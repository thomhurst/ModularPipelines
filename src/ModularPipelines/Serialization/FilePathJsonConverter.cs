using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Serialization;

[ExcludeFromCodeCoverage]
internal class FilePathJsonConverter : JsonConverter<FilePath>
{
    public override FilePath? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return new FilePath(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, FilePath value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Path);
    }
}
