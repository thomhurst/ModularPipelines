using ModularPipelines.Logging;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public IGitHubClient Client { get; }
    
    public IGitHubRepositoryInfo RepositoryInfo { get; }

    public GitHub(
        IGitHubEnvironmentVariables environmentVariables,
        IGitHubRepositoryInfo gitHubRepositoryInfo,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        EnvironmentVariables = environmentVariables;
        
        var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("No GITHUB_TOKEN found");
        }
        
        Client = new GitHubClient(new ProductHeaderValue("ModularPipelines"),
            new InMemoryCredentialStore(new Credentials(token)));

        RepositoryInfo = gitHubRepositoryInfo;
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