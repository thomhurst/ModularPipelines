using ModularPipelines.Logging;

namespace ModularPipelines.TeamCity;

internal class TeamCity : ITeamCity
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public TeamCity(ITeamCityEnvironmentVariables environmentVariables,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        EnvironmentVariables = environmentVariables;
    }

    public ITeamCityEnvironmentVariables EnvironmentVariables { get; }

    public void StartConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.TeamCity.StartBlock(name));
    }

    public void EndConsoleLogGroup(string name)
    {
        LogToConsole(BuildSystemValues.TeamCity.EndBlock(name));
    }

    public void LogToConsole(string value)
    {
        _moduleLoggerProvider.GetLogger().LogToConsole(value);
    }
}