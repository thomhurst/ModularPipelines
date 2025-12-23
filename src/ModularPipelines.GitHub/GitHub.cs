using Microsoft.Extensions.Http.Logging;
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
        _outputWriter = outputWriterFactory.Create("GitHub");
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

        var connection = new Connection(new ProductHeaderValue("ModularPipelines"),
          new HttpClientAdapter(() =>
          {
              var moduleLogger = _moduleLoggerProvider.GetLogger();

              return new RequestLoggingHttpHandler(moduleLogger, _httpLogger)
              {
                  InnerHandler = new ResponseLoggingHttpHandler(moduleLogger, _httpLogger)
                  {
                      InnerHandler = new StatusCodeLoggingHttpHandler(moduleLogger, _httpLogger)
                      {
                          InnerHandler = new HttpClientHandler(),
                      },
                  },
              };
          }));

        var client = new GitHubClient(connection)
        {
            Credentials = new Credentials(token),
        };

        return client;
    }
}