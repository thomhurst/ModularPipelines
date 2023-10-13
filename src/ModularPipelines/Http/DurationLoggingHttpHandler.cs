using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class DurationLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public DurationLoggingHttpHandler(IModuleLoggerProvider moduleLoggerProvider)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
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
            HttpLogger.PrintDuration(stopwatch.Elapsed, _moduleLoggerProvider.GetLogger());
        }
    }
}