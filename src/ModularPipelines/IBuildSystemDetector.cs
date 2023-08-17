namespace ModularPipelines;

internal interface IBuildSystemDetector
{
    bool IsRunningOnAzurePipelines { get; }
    bool IsRunningOnTeamCity { get; }
    bool IsRunningOnGitHubActions { get; }
    bool IsRunningOnJenkins { get; }
}