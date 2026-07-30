using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerProvider _loggerProvider;
    private readonly IHttpLogger _httpLogger;

    public ResponseLoggingHttpHandler(IModuleLoggerProvider loggerProvider, IHttpLogger httpLogger)
    {
        _loggerProvider = loggerProvider;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        try
        {
            var logger = _loggerProvider.GetLogger();
            await _httpLogger
                .PrintResponse(response, logger, cancellationToken)
                .ConfigureAwait(false);
            return response;
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }
}
