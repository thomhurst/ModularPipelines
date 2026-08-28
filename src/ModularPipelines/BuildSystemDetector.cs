using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Environment;
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
    private static readonly (string Variable, BuildSystem BuildSystem)[] DetectionOrder =
    [
        ("TF_BUILD", BuildSystem.AzurePipelines),
        ("TEAMCITY_VERSION", BuildSystem.TeamCity),
        ("GITHUB_ACTIONS", BuildSystem.GitHubActions),
        ("JENKINS_URL", BuildSystem.Jenkins),
        ("GITLAB_CI", BuildSystem.GitLab),
        ("BITBUCKET_BUILD_NUMBER", BuildSystem.Bitbucket),
        ("TRAVIS", BuildSystem.TravisCI),
        ("APPVEYOR", BuildSystem.AppVeyor),
    ];

    private readonly IEnvironmentVariablesContext _environmentVariables;
    private readonly Lazy<DetectionResult> _detection;

    public BuildSystemDetector(IEnvironmentVariablesContext environmentVariables)
    {
        _environmentVariables = environmentVariables;
        _detection = new Lazy<DetectionResult>(DetectCurrentBuildSystem);
    }

    public BuildSystem Current => _detection.Value.BuildSystem;

    public string? MatchedEnvironmentVariable => _detection.Value.EnvironmentVariable;

    public BuildSystem GetCurrentBuildSystem() => Current;

    private DetectionResult DetectCurrentBuildSystem()
    {
        foreach (var (variable, buildSystem) in DetectionOrder)
        {
            if (string.IsNullOrEmpty(_environmentVariables.Get(variable)))
            {
                continue;
            }

            return new DetectionResult(buildSystem, variable);
        }

        return new DetectionResult(BuildSystem.Unknown, null);
    }

    private readonly record struct DetectionResult(
        BuildSystem BuildSystem,
        string? EnvironmentVariable);
}
