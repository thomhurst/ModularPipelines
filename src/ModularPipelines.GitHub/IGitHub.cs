using ModularPipelines.Interfaces;

namespace ModularPipelines.GitHub;

public interface IGitHub : ICollapsableLogging
{
    IGitHubEnvironmentVariables EnvironmentVariables { get; }
}