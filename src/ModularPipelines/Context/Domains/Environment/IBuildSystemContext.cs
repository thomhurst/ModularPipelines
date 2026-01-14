namespace ModularPipelines.Context.Domains.Environment;

/// <summary>
/// Detects CI/CD build system environment.
/// </summary>
public interface IBuildSystemContext
{
    /// <summary>
    /// True if running on GitHub Actions.
    /// </summary>
    bool IsGitHubActions { get; }

    /// <summary>
    /// True if running on Azure Pipelines.
    /// </summary>
    bool IsAzurePipelines { get; }

    /// <summary>
    /// True if running on TeamCity.
    /// </summary>
    bool IsTeamCity { get; }

    /// <summary>
    /// True if running on Jenkins.
    /// </summary>
    bool IsJenkins { get; }

    /// <summary>
    /// True if running on GitLab CI/CD.
    /// </summary>
    bool IsGitLab { get; }

    /// <summary>
    /// True if running on Bitbucket Pipelines.
    /// </summary>
    bool IsBitbucket { get; }

    /// <summary>
    /// True if running on Travis CI.
    /// </summary>
    bool IsTravisCI { get; }

    /// <summary>
    /// True if running on AppVeyor.
    /// </summary>
    bool IsAppVeyor { get; }

    /// <summary>
    /// True if running on any CI/CD build server.
    /// </summary>
    bool IsBuildServer { get; }
}
