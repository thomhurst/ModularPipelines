using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class StatusCodeLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;
    private readonly IHttpLogger _httpLogger;

    public StatusCodeLoggingHttpHandler(IModuleLogger logger, IHttpLogger httpLogger)
    {
        _logger = logger;
        _httpLogger = httpLogger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            _httpLogger.PrintStatusCode(httpResponseMessage.StatusCode, _logger);

            return httpResponseMessage;
        }
        catch (HttpRequestException e)
        {
            _httpLogger.PrintStatusCode(e.StatusCode, _logger);
            throw;
        }
    }
}