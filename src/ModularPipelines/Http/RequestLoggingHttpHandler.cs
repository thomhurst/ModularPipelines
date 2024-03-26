using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class RequestLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;

    public RequestLoggingHttpHandler(IModuleLogger logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await HttpLogger.PrintRequest(request, _logger);

        var response = await base.SendAsync(request, cancellationToken);

        return response;
    }
}