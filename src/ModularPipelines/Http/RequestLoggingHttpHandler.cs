using ModularPipelines.Logging;

namespace ModularPipelines.Http;

public class RequestLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public RequestLoggingHttpHandler(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();

        await HttpLogger.PrintRequest(request, logger);

        var response = await base.SendAsync(request, cancellationToken);

        return response.EnsureSuccessStatusCode();
    }
}