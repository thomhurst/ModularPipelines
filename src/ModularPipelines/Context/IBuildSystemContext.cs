using ModularPipelines.Enums;

namespace ModularPipelines.Context;

/// <summary>
/// Detects CI/CD build system environment.
/// </summary>
public interface IBuildSystemContext
{
    /// <summary>
    /// Gets the current build system.
    /// </summary>
    BuildSystem Current =>
        this switch
        {
            { IsAzurePipelines: true } => BuildSystem.AzurePipelines,
            { IsTeamCity: true } => BuildSystem.TeamCity,
            { IsGitHubActions: true } => BuildSystem.GitHubActions,
            { IsJenkins: true } => BuildSystem.Jenkins,
            { IsGitLab: true } => BuildSystem.GitLab,
            { IsBitbucket: true } => BuildSystem.Bitbucket,
            { IsTravisCI: true } => BuildSystem.TravisCI,
            { IsAppVeyor: true } => BuildSystem.AppVeyor,
            _ => BuildSystem.Unknown,
        };

    /// <summary>
    /// Gets a value indicating whether this is running on GitHub Actions.
    /// </summary>
    bool IsGitHubActions { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on Azure Pipelines.
    /// </summary>
    bool IsAzurePipelines { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on TeamCity.
    /// </summary>
    bool IsTeamCity { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on Jenkins.
    /// </summary>
    bool IsJenkins { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on GitLab CI/CD.
    /// </summary>
    bool IsGitLab { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on Bitbucket Pipelines.
    /// </summary>
    bool IsBitbucket { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on Travis CI.
    /// </summary>
    bool IsTravisCI { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on AppVeyor.
    /// </summary>
    bool IsAppVeyor { get; }

    /// <summary>
    /// Gets a value indicating whether this is running on any CI/CD build server.
    /// </summary>
    bool IsBuildServer { get; }
}
