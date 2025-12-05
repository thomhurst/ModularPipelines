using ModularPipelines.Context;
using ModularPipelines.Enums;

namespace ModularPipelines;

/// <summary>
/// Detects the current CI/CD build system by examining environment variables.
/// Supports GitHub Actions, Azure Pipelines, TeamCity, GitLab, Jenkins, and others.
/// </summary>
/// <remarks>
/// Detection is performed by checking for the presence of specific environment variables
/// that are set by each build system:
/// - GitHub Actions: GITHUB_ACTIONS
/// - Azure Pipelines: TF_BUILD
/// - TeamCity: TEAMCITY_VERSION
/// - GitLab: GITLAB_CI
/// - Jenkins: JENKINS_URL
/// - Bitbucket: BITBUCKET_BUILD_NUMBER
/// - Travis CI: TRAVIS
/// - AppVeyor: APPVEYOR.
/// </remarks>
/// <example>
/// <code>
/// var detector = new BuildSystemDetector(environmentVariables);
/// var currentSystem = detector.GetCurrentBuildSystem();
///
/// if (currentSystem == BuildSystem.GitHubActions)
/// {
///     // Use GitHub Actions specific features
/// }
/// </code>
/// </example>
internal class BuildSystemDetector : IBuildSystemDetector
{
    public static readonly BuildSystemDetector Instance = new(new EnvironmentVariables());
    private readonly IEnvironmentVariables _environmentVariables;

    private readonly Dictionary<string, BuildSystem> _variablesToBuildSystem = new()
    {
        ["TF_BUILD"] = BuildSystem.AzurePipelines,
        ["TEAMCITY_VERSION"] = BuildSystem.TeamCity,
        ["GITHUB_ACTIONS"] = BuildSystem.GitHubActions,
        ["JENKINS_URL"] = BuildSystem.Jenkins,
        ["GITLAB_CI"] = BuildSystem.GitLab,
        ["BITBUCKET_BUILD_NUMBER"] = BuildSystem.Bitbucket,
        ["TRAVIS"] = BuildSystem.TravisCI,
        ["APPVEYOR"] = BuildSystem.AppVeyor,
    };

    public BuildSystemDetector(IEnvironmentVariables environmentVariables)
    {
        _environmentVariables = environmentVariables;
    }

    public BuildSystem GetCurrentBuildSystem()
    {
        return _variablesToBuildSystem.Keys
                   .Where(environmentVariable => !IsEmptyEnvironmentVariable(environmentVariable))
                   .Select(x => _variablesToBuildSystem[x])
                   .OfType<BuildSystem?>()
                   .FirstOrDefault()
               ?? BuildSystem.Unknown;
    }

    private bool IsEmptyEnvironmentVariable(string environmentVariable) =>
        string.IsNullOrEmpty(_environmentVariables.GetEnvironmentVariable(environmentVariable));
}