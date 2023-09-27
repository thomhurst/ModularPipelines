namespace ModularPipelines.TeamCity;

internal class TeamCity : ITeamCity
{
    public TeamCity(ITeamCityEnvironmentVariables environmentVariables)
    {
        EnvironmentVariables = environmentVariables;
    }

    public ITeamCityEnvironmentVariables EnvironmentVariables { get; }
}