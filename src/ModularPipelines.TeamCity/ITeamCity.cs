using ModularPipelines.Logging;

namespace ModularPipelines.TeamCity;

public interface ITeamCity : IModuleOutputWriter
{
    ITeamCityEnvironmentVariables EnvironmentVariables { get; }
}