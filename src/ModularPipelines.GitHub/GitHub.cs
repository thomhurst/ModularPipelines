using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.GitHub.Options;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly GitHubOptions _options;
    private readonly IHttpMessageHandlerFactory _httpMessageHandlerFactory;
    private readonly Lazy<IGitHubClient> _client;
    private readonly IModuleOutputBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;

    public IGitHubClient Client => _client.Value;

    public IGitHubRepositoryInfo RepositoryInfo { get; }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }

    public GitHub(
      IOptions<GitHubOptions> options,
      IGitHubEnvironmentVariables environmentVariables,
      IGitHubRepositoryInfo gitHubRepositoryInfo,
      IHttpMessageHandlerFactory httpMessageHandlerFactory,
      IConsoleCoordinator consoleCoordinator,
      IBuildSystemFormatterProvider formatterProvider)
    {
        _options = options.Value;
        _httpMessageHandlerFactory = httpMessageHandlerFactory;
        EnvironmentVariables = environmentVariables;
        _client = new Lazy<IGitHubClient>(InitializeClient);
        RepositoryInfo = gitHubRepositoryInfo;
        _buffer = consoleCoordinator.GetUnattributedBuffer();
        _formatter = formatterProvider.GetFormatter();
    }

    public void WriteLine(string message)
    {
        _buffer.WriteLine(message);
    }

    public IDisposable BeginSection(string name)
    {
        return new OutputSection(this, name, _formatter);
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

        var connection = new Connection(new ProductHeaderValue("ModularPipelines"),
          new HttpClientAdapter(() => _httpMessageHandlerFactory.CreateHandler(GitHubHttpClientServiceCollectionExtensions.GitHubClientName)));

        var client = new GitHubClient(connection)
        {
            Credentials = new Credentials(token),
        };

        return client;
    }

    private sealed class OutputSection : IDisposable
    {
        private readonly GitHub _github;
        private readonly string _name;
        private readonly IBuildSystemFormatter _formatter;

        public OutputSection(GitHub github, string name, IBuildSystemFormatter formatter)
        {
            _github = github;
            _name = name;
            _formatter = formatter;

            var startCommand = formatter.GetStartBlockCommand(name);
            if (startCommand != null)
            {
                _github._buffer.WriteLine(startCommand);
            }
        }

        public void Dispose()
        {
            var endCommand = _formatter.GetEndBlockCommand(_name);
            if (endCommand != null)
            {
                _github._buffer.WriteLine(endCommand);
            }
        }
    }
}
