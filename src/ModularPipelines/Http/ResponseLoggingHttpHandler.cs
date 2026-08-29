using ModularPipelines.Logging;

namespace ModularPipelines.Http;

internal class ResponseLoggingHttpHandler : DelegatingHandler
{
    private readonly IModuleLoggerAccessor _loggerAccessor;
    private readonly IHttpLogger _httpLogger;

    public ResponseLoggingHttpHandler(IModuleLoggerAccessor loggerAccessor, IHttpLogger httpLogger)
    {
        _loggerAccessor = loggerAccessor;
        _httpLogger = httpLogger;
    }

    /// <inheritdoc/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        try
        {
            var logger = _loggerAccessor.Logger;
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
