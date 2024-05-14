using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class DurationLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;

    public DurationLoggingHttpHandler(IModuleLogger logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        finally
        {
            HttpLogger.PrintDuration(stopwatch.Elapsed, _logger);
        }
    }
}