using System.Text.Json;
using System.Text.Json.Serialization;
using ModularPipelines.Enums;

namespace ModularPipelines.JsonUtils;

internal sealed class RunHistoryModuleStatusJsonConverter : JsonConverter<ModuleStatus>
{
    private static readonly IReadOnlyDictionary<string, ModuleStatus> LegacyNames =
        new Dictionary<string, ModuleStatus>(StringComparer.Ordinal)
        {
            ["NotYetStarted"] = ModuleStatus.NotStarted,
            ["Processing"] = ModuleStatus.Running,
            ["Successful"] = ModuleStatus.Succeeded,
            ["UsedHistory"] = ModuleStatus.RestoredFromHistory,
            ["IgnoredFailure"] = ModuleStatus.FailureIgnored,
            ["PipelineTerminated"] = ModuleStatus.Cancelled,
            ["CachedResult"] = ModuleStatus.RestoredFromCache,
        };

    public override ModuleStatus Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("A module status must be a JSON string.");
        }

        var name = reader.GetString();
        if (name is not null
            && (LegacyNames.TryGetValue(name, out var legacyStatus)
                || Enum.TryParse(name, ignoreCase: false, out legacyStatus)))
        {
            return legacyStatus;
        }

        throw new JsonException($"Unknown module status '{name}'.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        ModuleStatus value,
        JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToString());
}
