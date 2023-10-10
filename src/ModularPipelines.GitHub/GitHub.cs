using ModularPipelines.Logging;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public GitHub(IGitHubEnvironmentVariables environmentVariables,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        EnvironmentVariables = environmentVariables;
    }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }

    public void StartConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.GitHub.StartBlock(name));
    }

    public void EndConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.GitHub.EndBlock);
    }

    public void LogToConsole(string value)
    {
        _moduleLoggerProvider.GetLogger().LogToConsole(value);
    }
}