using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

/// <summary>
/// Carries correlation metadata populated while a run report is completed.
/// </summary>
public sealed class RunReportEnrichmentContext
{
    /// <summary>
    /// Initializes a run report enrichment context.
    /// </summary>
    /// <param name="runId">The current run identifier.</param>
    /// <param name="pipelineIdentity">The stable pipeline identity.</param>
    /// <param name="hostname">The machine executing the run.</param>
    /// <param name="buildSystem">The detected build system.</param>
    public RunReportEnrichmentContext(
        string runId,
        string pipelineIdentity,
        string hostname,
        BuildSystem buildSystem)
    {
        RunId = runId;
        PipelineIdentity = pipelineIdentity;
        Hostname = hostname;
        BuildSystem = buildSystem;
    }

    /// <summary>
    /// Gets the current run identifier.
    /// </summary>
    public string RunId { get; }

    /// <summary>
    /// Gets the stable pipeline identity used for retained history.
    /// </summary>
    public string PipelineIdentity { get; }

    /// <summary>
    /// Gets or sets the Git commit associated with the run.
    /// </summary>
    public string? GitSha { get; set; }

    /// <summary>
    /// Gets or sets the Git branch associated with the run.
    /// </summary>
    public string? GitBranch { get; set; }

    /// <summary>
    /// Gets or sets the machine that executed the run.
    /// </summary>
    public string? Hostname { get; set; }

    /// <summary>
    /// Gets or sets the CI run URL.
    /// </summary>
    public string? CiRunUrl { get; set; }

    /// <summary>
    /// Gets or sets the detected build system.
    /// </summary>
    public BuildSystem BuildSystem { get; set; }

    internal RunReportEnrichmentContext Copy() =>
        new(RunId, PipelineIdentity, Hostname ?? string.Empty, BuildSystem)
        {
            GitSha = GitSha,
            GitBranch = GitBranch,
            Hostname = Hostname,
            CiRunUrl = CiRunUrl,
        };
}
