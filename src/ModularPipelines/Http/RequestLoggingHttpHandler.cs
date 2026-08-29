using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class RequestLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IHttpLogger _httpLogger;

    public RequestLoggingHttpHandler(IModuleLoggerAccessor loggerAccessor, IHttpLogger httpLogger)
    {
        _loggerAccessor = loggerAccessor;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var logger = _loggerAccessor.Logger;
        await _httpLogger
            .PrintRequest(request, logger, cancellationToken)
            .ConfigureAwait(false);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        return response;
    }
}
