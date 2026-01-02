using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IHttpLogger _httpLogger;

    public ResponseLoggingHttpHandler(IModuleLoggerProvider loggerProvider, IHttpLogger httpLogger)
    {
        _loggerProvider = loggerProvider;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        // Buffer the response content so it can be read multiple times.
        // Without this, reading the body for logging consumes the stream,
        // making it unreadable by subsequent code. See issue #1610.
        await response.Content.LoadIntoBufferAsync().ConfigureAwait(false);

        var logger = _loggerProvider.GetLogger();
        await _httpLogger.PrintResponse(response, logger).ConfigureAwait(false);

        return response;
    }
}
