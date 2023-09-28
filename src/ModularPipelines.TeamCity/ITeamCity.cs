using ModularPipelines.Interfaces;

namespace ModularPipelines.TeamCity;

public interface ITeamCity : ICollapsableLogging
{
    ITeamCityEnvironmentVariables EnvironmentVariables { get; }
}