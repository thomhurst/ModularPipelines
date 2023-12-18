using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class StatusCodeLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public StatusCodeLoggingHttpHandler(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            HttpLogger.PrintStatusCode(httpResponseMessage.StatusCode, _moduleLoggerProvider.GetLogger());

            return httpResponseMessage;
        }
        catch (HttpRequestException e)
        {
            HttpLogger.PrintStatusCode(e.StatusCode, _moduleLoggerProvider.GetLogger());
            throw;
        }
    }
}