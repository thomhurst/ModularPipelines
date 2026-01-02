using Microsoft.Extensions.Options;
using ModularPipelines.GitHub.Options;
using ModularPipelines.Logging;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly GitHubOptions _options;
    private readonly IHttpMessageHandlerFactory _httpMessageHandlerFactory;
    private readonly Lazy<IGitHubClient> _client;
    private readonly ModuleOutputWriter _outputWriter;

    public IGitHubClient Client => _client.Value;

    public IGitHubRepositoryInfo RepositoryInfo { get; }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }

    public GitHub(
      IOptions<GitHubOptions> options,
      IGitHubEnvironmentVariables environmentVariables,
      IGitHubRepositoryInfo gitHubRepositoryInfo,
      IHttpMessageHandlerFactory httpMessageHandlerFactory,
      IModuleLoggerProvider moduleLoggerProvider,
      IModuleOutputWriterFactory outputWriterFactory)
    {
        _options = options.Value;
        _httpMessageHandlerFactory = httpMessageHandlerFactory;
        EnvironmentVariables = environmentVariables;

        _client = new Lazy<IGitHubClient>(InitializeClient);
        RepositoryInfo = gitHubRepositoryInfo;
        _outputWriter = outputWriterFactory.Create("GitHub", moduleLoggerProvider.GetLogger());
    }

    public void WriteLine(string message)
    {
        _outputWriter.WriteLine(message);
    }

    public void WriteLineDirect(string message)
    {
        _outputWriter.WriteLineDirect(message);
    }

    public IDisposable BeginSection(string name)
    {
        return _outputWriter.BeginSection(name);
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

        // Use IHttpMessageHandlerFactory to create handlers with logging configured via IHttpClientFactory
        var connection = new Connection(
            new ProductHeaderValue("ModularPipelines"),
            new HttpClientAdapter(() => _httpMessageHandlerFactory.CreateHandler(GitHubHttpClientNames.GitHub)));

        var client = new GitHubClient(connection)
        {
            Credentials = new Credentials(token),
        };

        return client;
    }
}