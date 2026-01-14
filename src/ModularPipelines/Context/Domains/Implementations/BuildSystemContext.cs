using ModularPipelines.Context.Domains.Environment;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Adapter that wraps <see cref="IBuildSystemDetector"/> to provide the <see cref="IBuildSystemContext"/> interface.
/// </summary>
internal class BuildSystemContext : IBuildSystemContext
{
    private readonly IBuildSystemDetector _detector;

    /// <summary>
    /// Initializes a new instance of the <see cref="BuildSystemContext"/> class.
    /// </summary>
    /// <param name="detector">The build system detector to wrap.</param>
    public BuildSystemContext(IBuildSystemDetector detector)
    {
        _detector = detector;
    }

    /// <inheritdoc />
    public bool IsGitHubActions => _detector.IsRunningOnGitHubActions;

    /// <inheritdoc />
    public bool IsAzurePipelines => _detector.IsRunningOnAzurePipelines;

    /// <inheritdoc />
    public bool IsTeamCity => _detector.IsRunningOnTeamCity;

    /// <inheritdoc />
    public bool IsJenkins => _detector.IsRunningOnJenkins;

    /// <inheritdoc />
    public bool IsBuildServer => _detector.IsKnownBuildAgent;
}
