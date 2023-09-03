namespace ModularPipelines.Enums;

public enum Status
{
    NotYetStarted,
    Processing,
    Successful,
    Failed,
    IgnoredFailure,
    PipelineTerminated,
    TimedOut,
    Skipped,
    Unknown
}
