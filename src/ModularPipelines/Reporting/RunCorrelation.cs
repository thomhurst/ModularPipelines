using ModularPipelines.Enums;

namespace ModularPipelines.Reporting;

/// <summary>
/// Identifies the source environment for a pipeline run.
/// </summary>
public sealed record RunCorrelation
{
    /// <summary>
    /// Gets the Git commit associated with the run, when available.
    /// </summary>
    public string? GitSha { get; init; }

    /// <summary>
    /// Gets the Git branch associated with the run, when available.
    /// </summary>
    public string? GitBranch { get; init; }

    /// <summary>
    /// Gets the machine that executed the run.
    /// </summary>
    public string? Hostname { get; init; }

    /// <summary>
    /// Gets the CI run URL, when available.
    /// </summary>
    public string? CiRunUrl { get; init; }

    /// <summary>
    /// Gets the detected build system.
    /// </summary>
    public BuildSystem BuildSystem { get; init; } = BuildSystem.Unknown;
}
