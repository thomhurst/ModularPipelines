using System.Text.Json.Serialization;

namespace ModularPipelines.Enums;

/// <summary>
/// A module's status.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<ModuleStatus>))]
public enum ModuleStatus
{
    /// <summary>
    /// Not yet started.
    /// </summary>
    NotStarted,

    /// <summary>
    /// Currently processing.
    /// </summary>
    Running,

    /// <summary>
    /// Successful.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The module failed.
    /// </summary>
    Failed,

    /// <summary>
    /// The module failed, but the failure was ignored.
    /// </summary>
    FailureIgnored,

    /// <summary>
    /// The module was skipped.
    /// </summary>
    Skipped,

    /// <summary>
    /// The module timed out.
    /// </summary>
    TimedOut,

    /// <summary>
    /// The module was cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// The module did not run because a required dependency failed.
    /// </summary>
    DependencyFailed,

    /// <summary>
    /// The module result was reconstructed from a previous run.
    /// </summary>
    RestoredFromHistory,

    /// <summary>
    /// The module result was restored from the fingerprint cache.
    /// </summary>
    RestoredFromCache,

    /// <summary>
    /// Unknown module status.
    /// </summary>
    Unknown,
}
