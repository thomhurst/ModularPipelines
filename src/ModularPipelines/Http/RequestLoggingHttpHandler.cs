using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class RequestLoggingHttpHandler : DelegatingHandler
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

        return response;
    }
}