using ModularPipelines.Logging;
using Octokit;

namespace ModularPipelines.GitHub;

public interface IGitHub : IModuleOutputWriter
{
    IGitHubClient Client { get; }

    IGitHubRepositoryInfo RepositoryInfo { get; }

    IGitHubEnvironmentVariables EnvironmentVariables { get; }
}