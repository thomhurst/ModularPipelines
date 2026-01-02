using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class RequestLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IHttpLogger _httpLogger;

    public RequestLoggingHttpHandler(IModuleLoggerProvider loggerProvider, IHttpLogger httpLogger)
    {
        _loggerProvider = loggerProvider;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var logger = _loggerProvider.GetLogger();
        await _httpLogger.PrintRequest(request, logger).ConfigureAwait(false);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        return response;
    }
}