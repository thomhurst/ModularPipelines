using ModularPipelines.Interfaces;
using Octokit;

namespace ModularPipelines.GitHub;

public interface IGitHub : ICollapsableLogging
{
    IGitHubClient Client { get; }

    IGitHubRepositoryInfo RepositoryInfo { get; }

    IGitHubEnvironmentVariables EnvironmentVariables { get; }
}