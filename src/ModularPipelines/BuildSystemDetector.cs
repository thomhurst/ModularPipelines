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
/// When no known build system is detected, a <c>CI</c> environment variable set to any value other than
/// <c>false</c> or <c>0</c> still marks the pipeline as running on a build server.
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

    public bool IsCI => Current != BuildSystem.Unknown || CiVariableOnlyValue is not null;

    public string? CiVariableOnlyValue
    {
        get
        {
            if (Current != BuildSystem.Unknown)
            {
                return null;
            }

            var ci = _environmentVariables.Get("CI");
            return IsTruthy(ci) ? ci!.Trim() : null;
        }
    }

    public BuildSystem GetCurrentBuildSystem() => Current;

    internal static bool IsTruthy(string? value)
    {
        var trimmed = value?.Trim();
        return !string.IsNullOrEmpty(trimmed)
               && !string.Equals(trimmed, "false", StringComparison.OrdinalIgnoreCase)
               && !string.Equals(trimmed, "0", StringComparison.Ordinal);
    }

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
