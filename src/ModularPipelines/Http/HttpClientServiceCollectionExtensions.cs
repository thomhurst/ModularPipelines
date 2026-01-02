using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Http;

/// <summary>
/// Extension methods for registering logging HttpClients with the DI container.
/// </summary>
internal static class HttpClientServiceCollectionExtensions
{
    /// <summary>
    /// Registers all 16 combinations of logging HttpClients (2^4 from the 4 HttpLoggingType flags).
    /// This allows sharing HttpClient instances through IHttpClientFactory instead of creating
    /// new ServiceProviders per logging type configuration.
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

        // Register all 16 combinations of logging types (0-15)
        // HttpLoggingType is a flags enum with: Request=1, Response=2, StatusCode=4, Duration=8
        for (var i = 0; i <= 15; i++)
        {
            var loggingType = (HttpLoggingType)i;
            var clientName = HttpClientNames.GetClientName(loggingType);

            var builder = services.AddHttpClient(clientName);

            // Add handlers based on the flags
            if (loggingType.HasFlag(HttpLoggingType.Request))
            {
                builder.AddHttpMessageHandler<RequestLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.Response))
            {
                builder.AddHttpMessageHandler<ResponseLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.Duration))
            {
                builder.AddHttpMessageHandler<DurationLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.StatusCode))
            {
                builder.AddHttpMessageHandler<StatusCodeLoggingHttpHandler>();
            }
        }

        return services;
    }
}
