using ModularPipelines.Enums;

namespace ModularPipelines;

internal class SmartCollapsableLoggingStringBlockProvider : ISmartCollapsableLoggingStringBlockProvider
{
    private readonly IBuildSystemDetector _buildSystemDetector;

    private readonly Dictionary<BuildSystem, LogBlockMarkers?> _markers = new()
    {
        [BuildSystem.GitHubActions] = new(BuildSystemValues.GitHub.StartBlock, str => BuildSystemValues.GitHub.EndBlock),
        [BuildSystem.TeamCity] = new(BuildSystemValues.TeamCity.StartBlock, BuildSystemValues.TeamCity.EndBlock),
        [BuildSystem.AzurePipelines] = new(BuildSystemValues.AzurePipelines.StartBlock, str => BuildSystemValues.AzurePipelines.EndBlock),
        [BuildSystem.Jenkins] = null,
        [BuildSystem.GitLab] = null,
        [BuildSystem.Bitbucket] = null,
        [BuildSystem.TravisCI] = null,
        [BuildSystem.AppVeyor] = null,
        [BuildSystem.Unknown] = null,
    };

    public SmartCollapsableLoggingStringBlockProvider(IBuildSystemDetector buildSystemDetector)
    {
        _buildSystemDetector = buildSystemDetector;
    }
    
    public string? GetStartConsoleLogGroup(string name)
    {
        return _markers.GetValueOrDefault(_buildSystemDetector.GetCurrentBuildSystem())?.Start(name);
    }

    public string? GetEndConsoleLogGroup(string name)
    {
        return _markers.GetValueOrDefault(_buildSystemDetector.GetCurrentBuildSystem())?.End(name);
    }

    private record LogBlockMarkers(Func<string, string> Start, Func<string, string> End);
}