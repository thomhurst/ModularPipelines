using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class DurationLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IHttpLogger _httpLogger;

    public DurationLoggingHttpHandler(IModuleLoggerAccessor loggerAccessor, IHttpLogger httpLogger)
    {
        _loggerAccessor = loggerAccessor;
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
            var logger = _loggerAccessor.Logger;
            _httpLogger.PrintDuration(stopwatch.Elapsed, logger);
        }
    }
}
