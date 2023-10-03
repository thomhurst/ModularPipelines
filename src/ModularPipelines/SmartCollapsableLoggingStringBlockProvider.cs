using ModularPipelines.Enums;

namespace ModularPipelines;

internal class SmartCollapsableLoggingStringBlockProvider : ISmartCollapsableLoggingStringBlockProvider
{
    private readonly IBuildSystemDetector _buildSystemDetector;

    public SmartCollapsableLoggingStringBlockProvider(IBuildSystemDetector buildSystemDetector)
    {
        _buildSystemDetector = buildSystemDetector;
    }
    
    public string? GetStartConsoleLogGroup(string name)
    {
        return _buildSystemDetector.GetCurrentBuildSystem() switch
        {
            BuildSystem.GitHubActions => BuildSystemValues.GitHub.StartBlock(name),
            BuildSystem.TeamCity => BuildSystemValues.TeamCity.StartBlock(name),
            BuildSystem.AzurePipelines => BuildSystemValues.AzurePipelines.StartBlock(name),
            BuildSystem.Jenkins => null,
            BuildSystem.GitLab => null,
            BuildSystem.Bitbucket => null,
            BuildSystem.TravisCI => null,
            BuildSystem.AppVeyor => null,
            BuildSystem.Unknown => null,
            _ => null,
        };
    }

    public string? GetEndConsoleLogGroup(string name)
    {
        return _buildSystemDetector.GetCurrentBuildSystem() switch
        {
            BuildSystem.GitHubActions => BuildSystemValues.GitHub.EndBlock,
            BuildSystem.TeamCity => BuildSystemValues.TeamCity.EndBlock(name),
            BuildSystem.AzurePipelines => BuildSystemValues.AzurePipelines.EndBlock,
            BuildSystem.Jenkins => null,
            BuildSystem.GitLab => null,
            BuildSystem.Bitbucket => null,
            BuildSystem.TravisCI => null,
            BuildSystem.AppVeyor => null,
            BuildSystem.Unknown => null,
            _ => null,
        };
    }
}