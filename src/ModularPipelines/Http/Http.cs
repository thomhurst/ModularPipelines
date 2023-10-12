using System.Collections.Concurrent;
using System.Diagnostics;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

internal class Http : IHttp
{
    public HttpClient HttpClient { get; }

    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    private readonly HttpClient _loggingHttpClient;
    private readonly HttpClient _requestLoggingHttpClient;
    private readonly HttpClient _responseLoggingHttpClient;

    private readonly ConcurrentDictionary<HttpLoggingType, HttpClient> _loggingHttpClients = new();

    public Http(HttpClient defaultHttpClient,
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpClientFactory httpClientFactory)
    {
        HttpClient = defaultHttpClient;
        _moduleLoggerProvider = moduleLoggerProvider;
        _loggingHttpClient = httpClientFactory.CreateClient("LoggingHttpClient");
        _requestLoggingHttpClient = httpClientFactory.CreateClient("RequestLoggingHttpClient");
        _responseLoggingHttpClient = httpClientFactory.CreateClient("ResponseLoggingHttpClient");
    }

    public async Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default)
    {
        httpOptions.HttpClient ??= HttpClient;
        
        return await SendAndWrapLogging(httpOptions, cancellationToken);
    }

    public HttpClient GetLoggingHttpClient(HttpLoggingType loggingType)
    {
        return _loggingHttpClients.GetOrAdd(loggingType, () =>
        {
            var innerHandler = new MessageHandler

            if (loggingType.HasFlag(HttpLoggingType.Request))
            {
                handler = new RequestLoggingHttpHandler(_moduleLoggerProvider);
            }
            
            
        })
        return loggingType switch
        {
            HttpLoggingType.RequestAndResponse => _loggingHttpClient,
            HttpLoggingType.RequestOnly => _requestLoggingHttpClient,
            HttpLoggingType.ResponseOnly => _responseLoggingHttpClient,
            HttpLoggingType.None => HttpClient,
            _ => throw new ArgumentOutOfRangeException(nameof(loggingType), loggingType, null),
        };
    }

    private async Task<HttpResponseMessage> SendAndWrapLogging(HttpOptions httpOptions, CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();
        
        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Request))
        {
            await HttpLogger.PrintRequest(httpOptions.HttpRequestMessage, logger);
        }
        
        var stopwatch = Stopwatch.StartNew();

        var response = await httpOptions.HttpClient!.SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        var duration = stopwatch.Elapsed;

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Response))
        {
            await HttpLogger.PrintResponse(response, logger);
        }

        return response.EnsureSuccessStatusCode();
    }
}
