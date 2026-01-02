using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.Options;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

internal class Http : IHttp
{
    public HttpClient HttpClient { get; }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHttpLogger _httpLogger;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public Http(HttpClient defaultHttpClient,
        IHttpClientFactory httpClientFactory,
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpLogger httpLogger,
        IOptions<PipelineOptions> pipelineOptions)
    {
        HttpClient = defaultHttpClient;
        _httpClientFactory = httpClientFactory;
        _moduleLoggerProvider = moduleLoggerProvider;
        _httpLogger = httpLogger;
        _pipelineOptions = pipelineOptions;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default)
    {
        if (httpOptions.HttpClient != null)
        {
            return await SendAndWrapLogging(httpOptions, cancellationToken).ConfigureAwait(false);
        }

        var httpClient = GetLoggingHttpClient(httpOptions.LoggingType);

        var response = await httpClient.SendAsync(httpOptions.HttpRequestMessage, cancellationToken).ConfigureAwait(false);

        if (!httpOptions.ThrowOnNonSuccessStatusCode)
        {
            return response;
        }

        return await response.EnsureSuccessStatusCodeWithContentAsync(cancellationToken).ConfigureAwait(false);
    }

    public HttpClient GetLoggingHttpClient(HttpLoggingType loggingType)
    {
        var clientName = HttpClientNames.GetClientName(loggingType);
        return _httpClientFactory.CreateClient(clientName);
    }

    private async Task<HttpResponseMessage> SendAndWrapLogging(HttpOptions httpOptions, CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();
        var loggingOptions = GetEffectiveLoggingOptions(httpOptions);

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Request))
        {
            await _httpLogger.PrintRequest(httpOptions.HttpRequestMessage, logger, loggingOptions).ConfigureAwait(false);
        }

        var stopWatch = Stopwatch.StartNew();

        var response = await httpOptions.HttpClient!.SendAsync(httpOptions.HttpRequestMessage, cancellationToken).ConfigureAwait(false);

        LogStatusCode(response.StatusCode, httpOptions, loggingOptions);
        LogDuration(stopWatch.Elapsed, httpOptions, loggingOptions);

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Response))
        {
            await _httpLogger.PrintResponse(response, logger, loggingOptions).ConfigureAwait(false);
        }

        if (!httpOptions.ThrowOnNonSuccessStatusCode)
        {
            return response;
        }

        return await response.EnsureSuccessStatusCodeWithContentAsync(cancellationToken).ConfigureAwait(false);
    }

    private HttpLoggingOptions GetEffectiveLoggingOptions(HttpOptions httpOptions)
    {
        // Priority: options property > pipeline default > default
        if (httpOptions.LogSettings is not null)
        {
            return httpOptions.LogSettings;
        }

        if (_pipelineOptions.Value.DefaultHttpLoggingOptions is not null)
        {
            return _pipelineOptions.Value.DefaultHttpLoggingOptions;
        }

        return HttpLoggingOptions.Default;
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
