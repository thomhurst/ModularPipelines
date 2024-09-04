using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// A module's status.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<Status>))]
public enum Status
{
    /// <summary>
    /// Not yet started.
    /// </summary>
    NotYetStarted,

    /// <summary>
    /// Currently processing.
    /// </summary>
    Processing,

    /// <summary>
    /// Successful.
    /// </summary>
    Successful,

    /// <summary>
    /// The module result was reconstructed from a previous run.
    /// </summary>
    UsedHistory,

    /// <summary>
    /// The module failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The module failed, but the failure was ignored.
    /// </summary>
    IgnoredFailure,

    /// <summary>
    /// The pipeline has been terminated due to failing modules.
    /// </summary>
    PipelineTerminated,

    /// <summary>
    /// The module timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The module was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// The module was retried.
    /// </summary>
    Retried,
    
    /// <summary>
    /// Unknown module status.
    /// </summary>
    Unknown,
}