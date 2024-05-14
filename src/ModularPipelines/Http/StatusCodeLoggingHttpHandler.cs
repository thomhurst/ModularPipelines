using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class StatusCodeLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;

    public StatusCodeLoggingHttpHandler(IModuleLogger logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            HttpLogger.PrintStatusCode(httpResponseMessage.StatusCode, _logger);

            return httpResponseMessage;
        }
        catch (HttpRequestException e)
        {
            HttpLogger.PrintStatusCode(e.StatusCode, _logger);
            throw;
        }
    }
}