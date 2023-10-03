using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;

namespace ModularPipelines;

internal class SmartCollapsableLogging : ICollapsableLogging, IInternalCollapsableLogging
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
        var startConsoleLogGroup = GetStartConsoleLogGroup(name);
        
        if (startConsoleLogGroup != null)
        {
            ModuleLogger.LogToConsole(startConsoleLogGroup);
        }
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

    public void EndConsoleLogGroup(string name)
    {
        var endConsoleLogGroup = GetEndConsoleLogGroup(name);
        
        if (endConsoleLogGroup != null)
        {
            ModuleLogger.LogToConsole(endConsoleLogGroup);
        }
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

    public void StartConsoleLogGroupInternal(string name)
    {
        var startConsoleLogGroup = GetStartConsoleLogGroup(name);
        
        if (startConsoleLogGroup != null)
        {
            Console.WriteLine(startConsoleLogGroup);
        }
    }

    public void EndConsoleLogGroupInternal(string name)
    {
        var endConsoleLogGroup = GetEndConsoleLogGroup(name);
        
        if (endConsoleLogGroup != null)
        {
            Console.WriteLine(endConsoleLogGroup);
        }
    }

    public void LogToConsoleInternal(string value)
    {
        Console.WriteLine(value);
    }
}