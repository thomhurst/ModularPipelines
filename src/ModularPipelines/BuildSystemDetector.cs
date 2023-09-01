namespace ModularPipelines;

internal class BuildSystemDetector : IBuildSystemDetector
{
    public bool IsRunningOnAzurePipelines
        => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TF_BUILD"));
    
    public bool IsRunningOnTeamCity => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
    
    public bool IsRunningOnGitHubActions => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"));
    
    public bool IsRunningOnJenkins => !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("JENKINS_URL"));
}