namespace ModularPipelines.Build.Settings;

/// <summary>
/// General settings for the build pipeline.
/// </summary>
public record PipelineSettings
{
    /// <summary>
    /// Whether this checkout was already built in Release by the CI build step.
    /// </summary>
    public bool BuildAlreadyCompleted { get; init; }

    /// <summary>
    /// The version used by the native CI build, reused when packing its outputs.
    /// </summary>
    public string? PackageVersion { get; init; }

    /// <summary>
    /// The target framework for running tests. Defaults to "net11.0".
    /// </summary>
    public string TestFramework { get; init; } = "net11.0";

    /// <summary>
    /// The default number of retry attempts for modules. Defaults to 0 (no retries).
    /// </summary>
    public int DefaultRetryCount { get; init; }
}
