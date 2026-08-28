using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class StatusCodeLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IHttpLogger _httpLogger;

    public StatusCodeLoggingHttpHandler(IModuleLoggerAccessor loggerAccessor, IHttpLogger httpLogger)
    {
        _loggerAccessor = loggerAccessor;
        _httpLogger = httpLogger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var logger = _loggerAccessor.Logger;
            _httpLogger.PrintStatusCode(httpResponseMessage.StatusCode, logger);

            return httpResponseMessage;
        }
        catch (HttpRequestException e)
        {
            var logger = _loggerAccessor.Logger;
            _httpLogger.PrintStatusCode(e.StatusCode, logger);
            throw;
        }
    }
}
