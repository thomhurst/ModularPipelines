namespace ModularPipelines.TeamCity;

public interface ITeamCity
{
    ITeamCityEnvironmentVariables EnvironmentVariables { get; }
}