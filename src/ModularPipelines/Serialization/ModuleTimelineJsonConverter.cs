using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using ModularPipelines.Reporting;

namespace ModularPipelines.Serialization;

/// <summary>
/// Preserves timeline JSON field names under the caller's naming policy.
/// </summary>
/// <remarks>
/// Public so JSON source generation in consumer assemblies can instantiate the converter.
/// </remarks>
public sealed class ModuleTimelineJsonConverter : JsonConverter<ModuleTimeline>
{
    private static readonly ConditionalWeakTable<JsonSerializerOptions, ModuleTimelineJsonContext> Contexts = new();

    /// <inheritdoc />
    public override ModuleTimeline? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize(ref reader, GetTypeInfo(options))?.ToTimeline();

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ModuleTimeline value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, new ModuleTimelineJson(value), GetTypeInfo(options));

    private static JsonTypeInfo<ModuleTimelineJson> GetTypeInfo(JsonSerializerOptions options) =>
        Contexts.GetValue(options, static options => new ModuleTimelineJsonContext(new JsonSerializerOptions(options)
        {
            TypeInfoResolver = null,
        }))
            .ModuleTimelineJson;
}

// Retain the original CLR wire names so System.Text.Json can apply the caller's naming policy.
// Source-generated metadata keeps this compatibility path available without reflection.
internal sealed record ModuleTimelineJson
{
    public ModuleTimelineJson()
    {
    }

    public ModuleTimelineJson(ModuleTimeline timeline)
    {
        ModuleName = timeline.Name;
        ModuleTypeName = timeline.TypeName;
        ModuleId = timeline.ModuleId;
        Priority = timeline.Priority;
        ExecutionHint = timeline.ExecutionHint;
        ReadyTime = timeline.ReadyTime;
        QueuedTime = timeline.QueuedTime;
        StartTime = timeline.StartTime;
        EndTime = timeline.EndTime;
        DependencyWaitTime = timeline.DependencyWaitTime;
        QueueWaitTime = timeline.QueueWaitTime;
        ExecutionDuration = timeline.ExecutionDuration;
        WasSkipped = timeline.WasSkipped;
        WasSuccessful = timeline.WasSuccessful;
        Status = timeline.Status;
    }

    public string ModuleName { get; init; } = string.Empty;

    public string ModuleTypeName { get; init; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ModuleId ModuleId { get; init; }

    public ModulePriority Priority { get; init; }

    public ExecutionHint ExecutionHint { get; init; }

    public DateTimeOffset? ReadyTime { get; init; }

    public DateTimeOffset? QueuedTime { get; init; }

    public DateTimeOffset? StartTime { get; init; }

    public DateTimeOffset? EndTime { get; init; }

    public TimeSpan? DependencyWaitTime { get; init; }

    public TimeSpan? QueueWaitTime { get; init; }

    public TimeSpan? ExecutionDuration { get; init; }

    public bool WasSkipped { get; init; }

    public bool WasSuccessful { get; init; }

    public ModuleStatus Status { get; init; }

    public ModuleTimeline ToTimeline() => new()
    {
        Name = ModuleName,
        TypeName = ModuleTypeName,
        ModuleId = ModuleId,
        Priority = Priority,
        ExecutionHint = ExecutionHint,
        ReadyTime = ReadyTime,
        QueuedTime = QueuedTime,
        StartTime = StartTime,
        EndTime = EndTime,
        DependencyWaitTime = DependencyWaitTime,
        QueueWaitTime = QueueWaitTime,
        ExecutionDuration = ExecutionDuration,
        WasSkipped = WasSkipped,
        WasSuccessful = WasSuccessful,
        Status = Status,
    };
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(ModuleTimelineJson))]
internal sealed partial class ModuleTimelineJsonContext : JsonSerializerContext;
