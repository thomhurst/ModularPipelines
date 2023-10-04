using ModularPipelines.Context;
using ModularPipelines.Enums;

namespace ModularPipelines;

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