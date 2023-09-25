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
        if (httpOptions.HttpClient != null)
        {
            return await SendAndWrapLogging(httpOptions, cancellationToken);
        }

        var httpClient = GetLoggingHttpClient(httpOptions.LoggingType);

        var response = await httpClient.SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        return response.EnsureSuccessStatusCode();
    }

    public HttpClient GetLoggingHttpClient(HttpLoggingType loggingType)
    {
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

        if (httpOptions.LoggingType is HttpLoggingType.RequestOnly or HttpLoggingType.RequestAndResponse)
        {
            await HttpLogger.PrintRequest(httpOptions.HttpRequestMessage, logger);
        }

        var response = await httpOptions.HttpClient!.SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        if (httpOptions.LoggingType is HttpLoggingType.ResponseOnly or HttpLoggingType.RequestAndResponse)
        {
            await HttpLogger.PrintResponse(response, logger);
        }

        return response.EnsureSuccessStatusCode();
    }
}
