using System.Reflection;
using ModularPipelines.Context;

namespace ModularPipelines;

internal class BuildSystemDetector : IBuildSystemDetector
{
    public static readonly BuildSystemDetector Instance = new(new EnvironmentVariables());
    private readonly IEnvironmentVariables _environmentVariables;

    public BuildSystemDetector(IEnvironmentVariables environmentVariables)
    {
        _environmentVariables = environmentVariables;
    }

    public bool IsKnownBuildAgent => typeof(BuildSystemDetector)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(propertyInfo => propertyInfo.Name.StartsWith("IsRunningOn"))
            .Select(propertyInfo => propertyInfo.GetValue(this))
            .OfType<bool>()
            .Any(x => x);

    public bool IsRunningOnAzurePipelines => !IsEmptyEnvironmentVariable("TF_BUILD");

    public bool IsRunningOnTeamCity => !IsEmptyEnvironmentVariable("TEAMCITY_VERSION");

    public bool IsRunningOnGitHubActions => !IsEmptyEnvironmentVariable("GITHUB_ACTIONS");

    public bool IsRunningOnJenkins => !IsEmptyEnvironmentVariable("JENKINS_URL");

    public bool IsRunningOnGitLab => !IsEmptyEnvironmentVariable("GITLAB_CI") || !IsEmptyEnvironmentVariable("GITLAB_FEATURES") || !IsEmptyEnvironmentVariable("GITLAB_USER_NAME");

    public bool IsRunningOnBitbucket => !IsEmptyEnvironmentVariable("BITBUCKET_BUILD_NUMBER");

    public bool IsRunningOnTravisCI => !IsEmptyEnvironmentVariable("TRAVIS");

    public bool IsRunningOnAppVeyor => !IsEmptyEnvironmentVariable("APPVEYOR");

    private bool IsEmptyEnvironmentVariable(string environmentVariable) =>
        string.IsNullOrEmpty(_environmentVariables.GetEnvironmentVariable(environmentVariable));
}