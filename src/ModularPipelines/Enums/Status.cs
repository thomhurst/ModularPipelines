namespace ModularPipelines.Enums;

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
    Unknown
}
