using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.FileSystem;

namespace ModularPipelines.JsonUtils;

[ExcludeFromCodeCoverage]
public class FolderPathJsonConverter : JsonConverter<Folder>
{
    public override Folder? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        return new Folder(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, Folder value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Path);
    }
}