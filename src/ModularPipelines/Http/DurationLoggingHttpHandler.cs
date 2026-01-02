using System.Diagnostics;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class DurationLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IHttpLogger _httpLogger;

    public DurationLoggingHttpHandler(IModuleLoggerProvider loggerProvider, IHttpLogger httpLogger)
    {
        _loggerProvider = loggerProvider;
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
            var logger = _loggerProvider.GetLogger();
            _httpLogger.PrintDuration(stopwatch.Elapsed, logger);
        }
    }
}
