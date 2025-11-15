using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.GitHub.Options;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Octokit;
using Octokit.Internal;

namespace ModularPipelines.GitHub;

internal class GitHub : IGitHub
{
    private readonly GitHubOptions _options;
    private readonly PipelineOptions _pipelineOptions;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;
    private readonly Lazy<IGitHubClient> _client;

    public IGitHubClient Client => _client.Value;

    public IGitHubRepositoryInfo RepositoryInfo { get; }

    public IGitHubEnvironmentVariables EnvironmentVariables { get; }

    public GitHub(
      IOptions<GitHubOptions> options,
      IOptions<PipelineOptions> pipelineOptions,
      IGitHubEnvironmentVariables environmentVariables,
      IGitHubRepositoryInfo gitHubRepositoryInfo,
      IModuleLoggerProvider moduleLoggerProvider,
      IHttpLogger httpLogger)
    {
        _options = options.Value;
        _pipelineOptions = pipelineOptions.Value;
        _moduleLoggerProvider = moduleLoggerProvider;
        _httpLogger = httpLogger;
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
        ((IConsoleWriter) _moduleLoggerProvider.GetLogger()).LogToConsole(value);
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

        var loggingType = _options.HttpLogging ?? _pipelineOptions.DefaultHttpLogging;

        var connection = new Connection(new ProductHeaderValue("ModularPipelines"),
          new HttpClientAdapter(() =>
          {
              var moduleLogger = _moduleLoggerProvider.GetLogger();

              // Build handler chain from innermost to outermost
              HttpMessageHandler handler = new HttpClientHandler();

              if (loggingType.HasFlag(HttpLoggingType.StatusCode))
              {
                  handler = new StatusCodeLoggingHttpHandler(moduleLogger, _httpLogger)
                  {
                      InnerHandler = handler,
                  };
              }

              if (loggingType.HasFlag(HttpLoggingType.Response))
              {
                  handler = new ResponseLoggingHttpHandler(moduleLogger, _httpLogger)
                  {
                      InnerHandler = handler,
                  };
              }

              if (loggingType.HasFlag(HttpLoggingType.Request))
              {
                  handler = new RequestLoggingHttpHandler(moduleLogger, _httpLogger)
                  {
                      InnerHandler = handler,
                  };
              }

              if (loggingType.HasFlag(HttpLoggingType.Duration))
              {
                  handler = new DurationLoggingHttpHandler(moduleLogger, _httpLogger)
                  {
                      InnerHandler = handler,
                  };
              }

              return handler;
          }));

        var client = new GitHubClient(connection)
        {
            Credentials = new Credentials(token),
        };

        return client;
    }
}