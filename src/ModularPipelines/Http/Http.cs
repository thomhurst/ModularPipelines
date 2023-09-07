using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

internal class Http : IHttp
{
    public HttpClient HttpClient { get; }
    public HttpClient LoggingHttpClient { get; }
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    public Http(HttpClient defaultHttpClient,
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpClientFactory httpClientFactory)
    {
        HttpClient = defaultHttpClient;
        _moduleLoggerProvider = moduleLoggerProvider;
        LoggingHttpClient = httpClientFactory.CreateClient("LoggingHttpClient");
    }

    public async Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default)
    {
        var logger = _moduleLoggerProvider.GetLogger();
        
        if (httpOptions.LogRequest)
        {
            await HttpLogger.PrintRequest(httpOptions.HttpRequestMessage, logger);
        }

        var response = await (httpOptions.HttpClient ?? HttpClient).SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        if (httpOptions.LogResponse)
        {
            await HttpLogger.PrintResponse(response, logger);
        }

        return response.EnsureSuccessStatusCode();
    }
}
