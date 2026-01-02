using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class StatusCodeLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IHttpLogger _httpLogger;

    public StatusCodeLoggingHttpHandler(IModuleLoggerProvider loggerProvider, IHttpLogger httpLogger)
    {
        _loggerProvider = loggerProvider;
        _httpLogger = httpLogger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var logger = _loggerProvider.GetLogger();
            _httpLogger.PrintStatusCode(httpResponseMessage.StatusCode, logger);

            return httpResponseMessage;
        }
        catch (HttpRequestException e)
        {
            var logger = _loggerProvider.GetLogger();
            _httpLogger.PrintStatusCode(e.StatusCode, logger);
            throw;
        }
    }
}
