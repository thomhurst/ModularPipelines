using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

internal class SmartCollapsableLogging : ICollapsableLogging
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IBuildSystemDetector _buildSystemDetector;

    private IModuleLogger ModuleLogger => _moduleLoggerProvider.GetLogger();
    
    public SmartCollapsableLogging(IModuleLoggerProvider moduleLoggerProvider, IBuildSystemDetector buildSystemDetector)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _buildSystemDetector = buildSystemDetector;
    }
    
    public void LogToConsole(string value)
    {
        ModuleLogger.LogToConsole(value);
    }

    public void StartConsoleLogGroup(string name)
    {
        switch (_buildSystemDetector.GetCurrentBuildSystem())
        {
            case BuildSystem.GitHubActions: 
                ModuleLogger.LogToConsole(BuildSystemValues.GitHub.StartBlock(name));
                break;
            case BuildSystem.TeamCity: 
                ModuleLogger.LogToConsole(BuildSystemValues.TeamCity.StartBlock(name));
                break;
            case BuildSystem.AzurePipelines: 
                ModuleLogger.LogToConsole(BuildSystemValues.AzurePipelines.StartBlock(name));
                break;
            case BuildSystem.Jenkins:
            case BuildSystem.GitLab:
            case BuildSystem.Bitbucket:
            case BuildSystem.TravisCI:
            case BuildSystem.AppVeyor:
            case BuildSystem.Unknown:
            default:
                break;
        }
    }

    public void EndConsoleLogGroup(string name)
    {
        switch (_buildSystemDetector.GetCurrentBuildSystem())
        {
            case BuildSystem.GitHubActions: 
                ModuleLogger.LogToConsole(BuildSystemValues.GitHub.EndBlock);
                break;
            case BuildSystem.TeamCity: 
                ModuleLogger.LogToConsole(BuildSystemValues.TeamCity.EndBlock(name));
                break;
            case BuildSystem.AzurePipelines: 
                ModuleLogger.LogToConsole(BuildSystemValues.AzurePipelines.EndBlock);
                break;
            case BuildSystem.Jenkins:
            case BuildSystem.GitLab:
            case BuildSystem.Bitbucket:
            case BuildSystem.TravisCI:
            case BuildSystem.AppVeyor:
            case BuildSystem.Unknown:
            default:
                break;
        }
    }
}