using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;
    private readonly IHttpLogger _httpLogger;

    public ResponseLoggingHttpHandler(IModuleLogger logger, IHttpLogger httpLogger)
    {
        _logger = logger;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        await _httpLogger.PrintResponse(response, _logger).ConfigureAwait(false);

        return response;
    }
}