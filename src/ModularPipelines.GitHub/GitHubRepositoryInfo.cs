using System.Diagnostics.CodeAnalysis;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

// ReSharper disable ConvertToPrimaryConstructor
namespace ModularPipelines.GitHub;

[ExcludeFromCodeCoverage]
internal record GitHubRepositoryInfo : IGitHubRepositoryInfo, IInitializer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GitHubRepositoryInfo> _logger;

    public bool IsInitialized { get; private set; }

    public string? Url { get; private set; }

    public string? Endpoint { get; private set; }

    public string? Owner { get; private set; }

    public string? RepositoryName { get; private set; }

    public GitHubRepositoryInfo(IServiceProvider serviceProvider, ILogger<GitHubRepositoryInfo> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        var scope = _serviceProvider.CreateAsyncScope();
        await using var scopeLifetime = scope.ConfigureAwait(false);
        var environment = scope.ServiceProvider.GetRequiredService<IGitHubEnvironmentVariables>();
        string? remoteUrl = null;

        try
        {
            var git = scope.ServiceProvider.GetRequiredService<IGit>();

            var options = new GitRemoteOptions
            {
                Arguments = ["get-url", "origin"],
            };

            var executionOptions = new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = false,
                Logging = scope.ServiceProvider
                .GetRequiredService<IOptions<LoggerFilterOptions>>()
                .Value
                .MinLevel == LogLevel.Debug
                ? CommandLoggingOptions.Diagnostic
                : CommandLoggingOptions.Silent,
            };

            var remote = await git.Commands.Remotes.RemoteAsync(options, executionOptions).ConfigureAwait(false);
            remoteUrl = remote.StandardOutput;

            if (string.IsNullOrEmpty(remoteUrl))
            {
                _logger.LogWarning("Error when detecting GitHub git repository: {Error}", remote.StandardError);
            }
        }
        catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogWarning(e, "Error when detecting GitHub git repository");
        }

        if (!GitHubRepositoryUrlParser.TryParse(remoteUrl, out var owner, out var repositoryName, environment.ServerUrl))
        {
            if (string.IsNullOrWhiteSpace(environment.Repository))
            {
                return;
            }

            remoteUrl = $"{(environment.ServerUrl ?? "https://github.com").TrimEnd('/')}/{environment.Repository}";
            if (!GitHubRepositoryUrlParser.TryParse(remoteUrl, out owner, out repositoryName, environment.ServerUrl))
            {
                return;
            }
        }

        Url = remoteUrl;
        Endpoint = "github";
        Owner = owner;
        RepositoryName = repositoryName;
        IsInitialized = true;
    }
}
