using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Http;

/// <summary>
/// Extension methods for registering logging and resilience HttpClients with the DI container.
/// </summary>
internal static class HttpClientServiceCollectionExtensions
{
    /// <summary>
    /// Registers the framework HTTP client and handlers used by other integrations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddLoggingHttpClients(this IServiceCollection services)
    {
        // Register the handlers as transient so they can be resolved per-request
        services.AddTransient<RequestLoggingHttpHandler>();
        services.AddTransient<ResponseLoggingHttpHandler>();
        services.AddTransient<DurationLoggingHttpHandler>();
        services.AddTransient<StatusCodeLoggingHttpHandler>();
        services.AddTransient<ResilienceHttpHandler>();

        services.AddHttpClient(HttpClientNames.Default)
            .AddHttpMessageHandler<ResilienceHttpHandler>();

        return services;
    }
}
