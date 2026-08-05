using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Options;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

internal class Http : IHttpContext
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public Http(
        IHttpClientFactory httpClientFactory,
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpLogger httpLogger,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _httpClientFactory = httpClientFactory;
        _moduleLoggerProvider = moduleLoggerProvider;
        _httpLogger = httpLogger;
        _pipelineOptions = pipelineOptions;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default)
    {
        var httpClient = httpOptions.HttpClient ?? GetHttpClient(httpOptions.LoggingType);

        // Priority: options property > pipeline default > HttpClient timeout
        var effectiveTimeout = httpOptions.Timeout
                               ?? _pipelineOptions.Value.Http.Timeout
                               ?? GetFiniteTimeout(httpClient);

        // Create timeout token if timeout is specified
        var timeoutCts = effectiveTimeout.HasValue
            ? new CancellationTokenSource(effectiveTimeout.Value)
            : null;

        // Keep caller cancellation alive through streamed body consumption even when no
        // finite timeout applies.
        var linkedCts = timeoutCts != null
            ? CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken)
            : cancellationToken.CanBeCanceled
                ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                : null;

        var effectiveCancellationToken = linkedCts?.Token ?? cancellationToken;

        try
        {
            HttpResponseMessage WrapResponse(HttpResponseMessage response)
            {
                if (linkedCts is null)
                {
                    return response;
                }

                response.Content = new TimeoutHttpContent(response.Content, timeoutCts, linkedCts);
                timeoutCts = null;
                linkedCts = null;
                return response;
            }

            if (httpOptions.HttpClient != null)
            {
                return await SendAndWrapLogging(
                        httpClient,
                        httpOptions,
                        WrapResponse,
                        effectiveCancellationToken)
                    .ConfigureAwait(false);
            }

            var response = await httpClient.SendAsync(
                    httpOptions.HttpRequestMessage,
                    HttpCompletionOption.ResponseHeadersRead,
                    effectiveCancellationToken)
                .ConfigureAwait(false);

            response = WrapResponse(response);
            try
            {
                if (!httpOptions.ThrowOnNonSuccessStatusCode)
                {
                    return response;
                }

                return await response
                    .EnsureSuccessStatusCodeWithContentAsync(effectiveCancellationToken)
                    .ConfigureAwait(false);
            }
            catch
            {
                response.Dispose();
                throw;
            }
        }
        finally
        {
            linkedCts?.Dispose();
            timeoutCts?.Dispose();
        }
    }

    private HttpClient GetHttpClient(HttpLoggingType loggingType)
    {
        var clientName = HttpClientNames.GetClientName(loggingType);
        return _httpClientFactory.CreateClient(clientName);
    }

    private async Task<HttpResponseMessage> SendAndWrapLogging(
        HttpClient httpClient,
        HttpOptions httpOptions,
        Func<HttpResponseMessage, HttpResponseMessage> wrapResponse,
        CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();
        var loggingOptions = GetEffectiveLoggingOptions(httpOptions);

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Request))
        {
            await _httpLogger
                .PrintRequest(
                    httpOptions.HttpRequestMessage,
                    logger,
                    loggingOptions,
                    cancellationToken)
                .ConfigureAwait(false);
        }

        var stopWatch = Stopwatch.StartNew();

        var response = await httpClient.SendAsync(
                httpOptions.HttpRequestMessage,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken)
            .ConfigureAwait(false);

        try
        {
            LogStatusCode(response.StatusCode, httpOptions, loggingOptions);
            LogDuration(stopWatch.Elapsed, httpOptions, loggingOptions);

            if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Response))
            {
                await _httpLogger
                    .PrintResponse(response, logger, loggingOptions, cancellationToken)
                    .ConfigureAwait(false);
            }

            response = wrapResponse(response);

            if (!httpOptions.ThrowOnNonSuccessStatusCode)
            {
                return response;
            }

            return await response.EnsureSuccessStatusCodeWithContentAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            response.Dispose();
            throw;
        }
    }

    private HttpLoggingOptions GetEffectiveLoggingOptions(HttpOptions httpOptions)
    {
        // Priority: options property > pipeline default > default
        if (httpOptions.LogSettings is not null)
        {
            return httpOptions.LogSettings;
        }

        if (_pipelineOptions.Value.Http.Logging is not null)
        {
            return _pipelineOptions.Value.Http.Logging;
        }

        return HttpLoggingOptions.Default;
    }

    private static TimeSpan? GetFiniteTimeout(HttpClient httpClient)
    {
        return httpClient.Timeout == System.Threading.Timeout.InfiniteTimeSpan
            ? null
            : httpClient.Timeout;
    }

    private void LogDuration(TimeSpan duration, HttpOptions httpOptions, HttpLoggingOptions loggingOptions)
    {
        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Duration) && loggingOptions.LogDuration)
        {
            _httpLogger.PrintDuration(duration, _moduleLoggerProvider.GetLogger());
        }
    }

    private void LogStatusCode(HttpStatusCode? httpStatusCode, HttpOptions httpOptions, HttpLoggingOptions loggingOptions)
    {
        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.StatusCode) && loggingOptions.LogStatusCode)
        {
            _httpLogger.PrintStatusCode(httpStatusCode, _moduleLoggerProvider.GetLogger());
        }
    }
}
