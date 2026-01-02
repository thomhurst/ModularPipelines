using ModularPipelines.Http;
using ModularPipelines.Logging;

namespace ModularPipelines.GitHub;

/// <summary>
/// Creates HttpMessageHandler instances with configured logging middleware.
/// </summary>
internal class HttpMessageHandlerFactory : IHttpMessageHandlerFactory
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;

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
        // Build the handler chain with logging handlers
        // The chain processes requests from outer to inner: Request -> Response -> StatusCode -> HttpClientHandler
        return new RequestLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
        {
            InnerHandler = new ResponseLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
            {
                InnerHandler = new StatusCodeLoggingHttpHandler(_moduleLoggerProvider, _httpLogger)
                {
                    InnerHandler = new HttpClientHandler(),
                },
            },
        };
    }
}
