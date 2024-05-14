using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;

    public ResponseLoggingHttpHandler(IModuleLogger logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        await HttpLogger.PrintResponse(response, _logger);

        return response;
    }
}