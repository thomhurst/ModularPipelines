using Microsoft.Extensions.Options;
using ModularPipelines.GitHub.Options;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly GitHubOptions _options;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;
    private readonly Lazy<IGitHubClient> _client;
    private readonly ModuleOutputWriter _outputWriter;

    public IGitHubClient Client => _client.Value;

    public IGitHubRepositoryInfo RepositoryInfo { get; }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }

    public GitHub(
      IOptions<GitHubOptions> options,
      IGitHubEnvironmentVariables environmentVariables,
      IGitHubRepositoryInfo gitHubRepositoryInfo,
      IModuleLoggerProvider moduleLoggerProvider,
      IHttpLogger httpLogger,
      IModuleOutputWriterFactory outputWriterFactory)
    {
        _options = options.Value;
        _moduleLoggerProvider = moduleLoggerProvider;
        _httpLogger = httpLogger;
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

        // Create handler chain with logging. SocketsHttpHandler provides connection pooling
        // internally via PooledConnectionLifetime, so we don't need to cache handler instances.
        // Each HttpClientAdapter gets its own handler chain, avoiding thread-safety issues
        // with shared DelegatingHandler.InnerHandler references.
        var connection = new Connection(
            new ProductHeaderValue("ModularPipelines"),
            new HttpClientAdapter(CreateHandler));

        var client = new GitHubClient(connection)
        {
            Credentials = new Credentials(token),
        };

        return client;
    }

    /// <summary>
    /// Creates a new handler chain for HTTP requests.
    /// SocketsHttpHandler provides connection pooling via PooledConnectionLifetime.
    /// </summary>
    private HttpMessageHandler CreateHandler()
    {
        return new RequestLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
        {
            InnerHandler = new ResponseLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
            {
                InnerHandler = new StatusCodeLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
                {
                    InnerHandler = new SocketsHttpHandler
                    {
                        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                        MaxConnectionsPerServer = 20,
                    },
                },
            },
        };
    }
}