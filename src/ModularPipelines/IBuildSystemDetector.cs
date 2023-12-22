using ModularPipelines.Enums;

namespace ModularPipelines;

/// <summary>
/// A helper to determine the type of build agent the pipeline is currently running on.
/// </summary>
public interface IBuildSystemDetector
{
    /// <summary>
    /// Gets a value indicating whether the current build agent is Azure Pipelines.
    /// </summary>
    bool IsRunningOnAzurePipelines => Is(BuildSystem.AzurePipelines);

    /// <summary>
    /// Gets a value indicating whether the current build agent is TeamCity.
    /// </summary>
    bool IsRunningOnTeamCity => Is(BuildSystem.TeamCity);

    /// <summary>
    /// Gets a value indicating whether the current build agent is GitHub actions.
    /// </summary>
    bool IsRunningOnGitHubActions => Is(BuildSystem.GitHubActions);

    /// <summary>
    /// Gets a value indicating whether the current build agent is Jenkins.
    /// </summary>
    bool IsRunningOnJenkins => Is(BuildSystem.Jenkins);

    /// <summary>
    /// Gets a value indicating whether the current build agent is GitLab.
    /// </summary>
    bool IsRunningOnGitLab => Is(BuildSystem.GitLab);

    /// <summary>
    /// Gets a value indicating whether the current build agent is Bitbucket.
    /// </summary>
    bool IsRunningOnBitbucket => Is(BuildSystem.Bitbucket);

    /// <summary>
    /// Gets a value indicating whether the current build agent is TravisCI.
    /// </summary>
    bool IsRunningOnTravisCI => Is(BuildSystem.TravisCI);

    /// <summary>
    /// Gets a value indicating whether the current build agent is AppVeyor.
    /// </summary>
    bool IsRunningOnAppVeyor => Is(BuildSystem.AppVeyor);

    /// <summary>
    /// Gets a value indicating whether the current build agent is known.
    /// </summary>
    bool IsKnownBuildAgent => !Is(BuildSystem.Unknown);

    /// <summary>
    /// Gets the current build agent type, if known.
    /// </summary>
    /// <returns>The build system type.</returns>
    BuildSystem GetCurrentBuildSystem();

    /// <summary>
    /// Gets a value indicating whether the current build agent is the type specified.
    /// </summary>
    /// <returns>True if it is the current build agent type.</returns>
    bool Is(BuildSystem buildSystem) => GetCurrentBuildSystem() == buildSystem;
}