using ModularPipelines.Context;

using ModularPipelines.Enums;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Adapter that wraps <see cref="IBuildSystemDetector"/> to provide the <see cref="IBuildSystemContext"/> interface.
/// </summary>
internal class BuildSystemContext(IBuildSystemDetector detector) : IBuildSystemContext
{
    /// <inheritdoc />
    public BuildSystem Current => detector.Current;

    /// <inheritdoc />
    public bool IsGitHubActions => detector.IsRunningOnGitHubActions;

    /// <inheritdoc />
    public bool IsAzurePipelines => detector.IsRunningOnAzurePipelines;

    /// <inheritdoc />
    public bool IsTeamCity => detector.IsRunningOnTeamCity;

    /// <inheritdoc />
    public bool IsJenkins => detector.IsRunningOnJenkins;

    /// <inheritdoc />
    public bool IsGitLab => detector.IsRunningOnGitLab;

    /// <inheritdoc />
    public bool IsBitbucket => detector.IsRunningOnBitbucket;

    /// <inheritdoc />
    public bool IsTravisCI => detector.IsRunningOnTravisCI;

    /// <inheritdoc />
    public bool IsAppVeyor => detector.IsRunningOnAppVeyor;

    /// <inheritdoc />
    public bool IsBuildServer => detector.IsKnownBuildAgent;
}
