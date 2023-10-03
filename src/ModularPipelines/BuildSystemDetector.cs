using ModularPipelines.Context;
using ModularPipelines.Enums;

namespace ModularPipelines;

internal class BuildSystemDetector : IBuildSystemDetector
{
    public static readonly BuildSystemDetector Instance = new(new EnvironmentVariables());
    private readonly IEnvironmentVariables _environmentVariables;

    public BuildSystemDetector(IEnvironmentVariables environmentVariables)
    {
        _environmentVariables = environmentVariables;
    }
    
    public BuildSystem GetCurrentBuildSystem()
    {
        if (!IsEmptyEnvironmentVariable("TF_BUILD"))
        {
            return BuildSystem.AzurePipelines;
        }

        if (!IsEmptyEnvironmentVariable("TEAMCITY_VERSION"))
        {
            return BuildSystem.TeamCity;
        }

        if (!IsEmptyEnvironmentVariable("GITHUB_ACTIONS"))
        {
            return BuildSystem.GitHubActions;
        }

        if (!IsEmptyEnvironmentVariable("JENKINS_URL"))
        {
            return BuildSystem.Jenkins;
        }

        if (!IsEmptyEnvironmentVariable("GITLAB_CI") 
            || !IsEmptyEnvironmentVariable("GITLAB_FEATURES") 
            || !IsEmptyEnvironmentVariable("GITLAB_USER_NAME"))
        {
            return BuildSystem.GitLab;
        }

        if (!IsEmptyEnvironmentVariable("BITBUCKET_BUILD_NUMBER"))
        {
            return BuildSystem.Bitbucket;
        }

        if (!IsEmptyEnvironmentVariable("TRAVIS"))
        {
            return BuildSystem.TravisCI;
        }

        if (!IsEmptyEnvironmentVariable("APPVEYOR"))
        {
            return BuildSystem.AppVeyor;
        }

        return BuildSystem.Unknown;
    }

    private bool IsEmptyEnvironmentVariable(string environmentVariable) =>
        string.IsNullOrEmpty(_environmentVariables.GetEnvironmentVariable(environmentVariable));
}