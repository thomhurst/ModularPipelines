using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class DurationLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLogger _logger;
    private readonly IHttpLogger _httpLogger;

    public DurationLoggingHttpHandler(IModuleLogger logger, IHttpLogger httpLogger)
    {
        _logger = logger;
        _httpLogger = httpLogger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            _httpLogger.PrintDuration(stopwatch.Elapsed, _logger);
        }
    }
}