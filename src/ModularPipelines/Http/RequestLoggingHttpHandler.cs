using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class RequestLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;
    private readonly IHttpLogger _httpLogger;

    public RequestLoggingHttpHandler(IModuleLogger logger, IHttpLogger httpLogger)
    {
        _logger = logger;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await _httpLogger.PrintRequest(request, _logger);

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}