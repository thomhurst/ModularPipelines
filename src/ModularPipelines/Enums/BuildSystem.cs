namespace ModularPipelines.Enums;

/// <summary>
/// Build agent types
/// </summary>
public enum BuildSystem
{
    /// <summary>
    /// Azure Pipelines
    /// </summary>
    AzurePipelines,

    /// <summary>
    /// TeamCity
    /// </summary>
    TeamCity,

    /// <summary>
    /// GitHub actions
    /// </summary>
    GitHubActions,

    /// <summary>
    /// Jenkins
    /// </summary>
    Jenkins,

    /// <summary>
    /// GitLab
    /// </summary>
    GitLab,

    /// <summary>
    /// Bitbucket
    /// </summary>
    Bitbucket,

    /// <summary>
    /// TravisCI
    /// </summary>
    TravisCI,

    /// <summary>
    /// AppVeyor
    /// </summary>
    AppVeyor,

    /// <summary>
    /// The build agent type couldn't be detected
    /// </summary>
    Unknown,
}