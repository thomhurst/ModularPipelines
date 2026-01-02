using System.Collections.Concurrent;
using ModularPipelines.Http;
using ModularPipelines.Logging;

namespace ModularPipelines.GitHub;

/// <summary>
/// Creates and caches HttpMessageHandler instances with configured logging middleware.
/// Handlers are cached per name to prevent socket exhaustion from creating multiple handler instances.
/// </summary>
internal class HttpMessageHandlerFactory : IHttpMessageHandlerFactory
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;
    private readonly ConcurrentDictionary<string, Lazy<HttpMessageHandler>> _handlers = new();

    public HttpMessageHandlerFactory(
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpLogger httpLogger)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc />
    public HttpMessageHandler CreateHandler(string name)
    {
        // Cache handlers by name to prevent socket exhaustion.
        // Each handler chain shares a single SocketsHttpHandler (via HttpClientHandler)
        // which provides connection pooling.
        return _handlers.GetOrAdd(name, _ => new Lazy<HttpMessageHandler>(CreateHandlerCore)).Value;
    }

    private HttpMessageHandler CreateHandlerCore()
    {
        // Build the handler chain with logging handlers
        // The chain processes requests from outer to inner: Request -> Response -> StatusCode -> SocketsHttpHandler
        // Using SocketsHttpHandler for better connection pooling and performance
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
