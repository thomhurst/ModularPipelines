using System.Collections.Concurrent;
using ModularPipelines.Http;
using ModularPipelines.Logging;

namespace ModularPipelines.GitHub;

/// <summary>
/// Creates HttpMessageHandler instances with configured logging middleware.
/// SocketsHttpHandler instances are cached per name to prevent socket exhaustion.
/// Logging handlers are created per-scope to respect DI lifetimes.
/// </summary>
internal class HttpMessageHandlerFactory : IHttpMessageHandlerFactory
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;

    /// <summary>
    /// Static cache for SocketsHttpHandler instances to prevent socket exhaustion.
    /// These are the underlying transport handlers that manage connection pooling.
    /// Must be static to ensure handlers are shared across all scopes.
    /// </summary>
    private static readonly ConcurrentDictionary<string, Lazy<SocketsHttpHandler>> SocketHandlers = new();

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
        // Get or create the shared SocketsHttpHandler for connection pooling.
        // The socket handler is cached to prevent socket exhaustion.
        var socketHandler = SocketHandlers.GetOrAdd(
            name,
            _ => new Lazy<SocketsHttpHandler>(() => new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
                MaxConnectionsPerServer = 20,
            })).Value;

        // Build the handler chain with logging handlers.
        // Logging handlers are created per-scope to use the current scope's loggers.
        // The chain processes requests from outer to inner: Request -> Response -> StatusCode -> SocketsHttpHandler
        return new RequestLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
        {
            InnerHandler = new ResponseLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
            {
                InnerHandler = new StatusCodeLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
                {
                    InnerHandler = socketHandler,
                },
            },
        };
    }
}
