using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Http;

namespace ModularPipelines.GitHub;

/// <summary>
/// Extension methods for registering GitHub HTTP client with the DI container.
/// </summary>
internal static class GitHubHttpClientServiceCollectionExtensions
{
    /// <summary>
    /// The name used for the GitHub HTTP client registered with IHttpClientFactory.
    /// </summary>
    internal const string GitHubClientName = "ModularPipelines_GitHub";

    /// <summary>
    /// Registers a named HTTP client for GitHub with logging delegating handlers.
    /// Uses IHttpClientFactory pattern to benefit from proper connection pooling
    /// and handler lifecycle management.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddGitHubHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(GitHubClientName)
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 20,
            })
            .AddHttpMessageHandler<RequestLoggingHttpHandler>()
            .AddHttpMessageHandler<ResponseLoggingHttpHandler>()
            .AddHttpMessageHandler<StatusCodeLoggingHttpHandler>();

        return services;
    }
}
