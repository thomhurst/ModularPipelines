using Microsoft.Extensions.Options;
using ModularPipelines.GitHub.Options;
using ModularPipelines.Logging;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
  private readonly GitHubOptions _options;
  private readonly IModuleLoggerProvider _moduleLoggerProvider;
  private readonly Lazy<IGitHubClient> _client;

  public IGitHubClient Client => _client.Value;

  public IGitHubRepositoryInfo RepositoryInfo { get; }
  
  public IGitHubEnvironmentVariables EnvironmentVariables { get; }

  public GitHub(
    IOptions<GitHubOptions> options,
    IGitHubEnvironmentVariables environmentVariables,
    IGitHubRepositoryInfo gitHubRepositoryInfo,
    IModuleLoggerProvider moduleLoggerProvider)
  {
    _options = options.Value;
    _moduleLoggerProvider = moduleLoggerProvider;
    EnvironmentVariables = environmentVariables;
    
    _client = new Lazy<IGitHubClient>(InitializeClient);
    RepositoryInfo = gitHubRepositoryInfo;
  }
  
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
  
  // PRIVATE METHODS
  // --------------------------------------------------------------------------------

  /// <summary>
  /// Initializes the GitHub client.
  /// </summary>
  /// <returns>An instance of <see cref="IGitHubClient"/>.</returns>
  private IGitHubClient InitializeClient()
  {
    var token = _options.AccessToken 
                ?? EnvironmentVariables.Token
                ?? throw new ArgumentException("No GitHub access token or GITHUB_TOKEN found in environment variables.");
    
    var client = new GitHubClient(new ProductHeaderValue("ModularPipelines"),
      new InMemoryCredentialStore(new Credentials(token)));

    return client;
  }
}