using ModularPipelines.Context;

namespace ModularPipelines;

internal class BuildSystemDetector : IBuildSystemDetector
{
    private readonly IEnvironmentContext _environment;

    public BuildSystemDetector(IEnvironmentContext environment)
    {
        _environment = environment;
    }
    
    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TF_BUILD"));
    
    public bool IsRunningOnTeamCity => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("TEAMCITY_VERSION"));
    
    public bool IsRunningOnGitHubActions => !string.IsNullOrEmpty(_environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_ACTIONS"));
    
    public bool IsRunningOnJenkins => !string.IsNullOrWhiteSpace(_environment.EnvironmentVariables.GetEnvironmentVariable("JENKINS_URL"));
}