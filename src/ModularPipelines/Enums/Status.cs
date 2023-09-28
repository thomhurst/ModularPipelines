using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<Status>))]
public enum Status
{
    NotYetStarted,
    Processing,
    Successful,
    UsedHistory,
    Failed,
    IgnoredFailure,
    PipelineTerminated,
    TimedOut,
    Skipped,
    Unknown,
}
