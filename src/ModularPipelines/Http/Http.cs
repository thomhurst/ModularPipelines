using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

internal class Http : IHttp, IDisposable
{
    public HttpClient HttpClient { get; }

    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    private readonly ConcurrentDictionary<HttpLoggingType, HttpClient> _loggingHttpClients = new();

    public Http(HttpClient defaultHttpClient,
        IModuleLoggerProvider moduleLoggerProvider,
        IHttpClientFactory httpClientFactory)
    {
        HttpClient = defaultHttpClient;
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpOptions httpOptions, CancellationToken cancellationToken = default)
    {
        if (httpOptions.HttpClient != null)
        {
            return await SendAndWrapLogging(httpOptions, cancellationToken);
        }

        var httpClient = GetLoggingHttpClient(httpOptions.LoggingType);

        var response = await httpClient.SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        if (!httpOptions.ThrowOnNonSuccessStatusCode)
        {
            return response;
        }
        
        return response.EnsureSuccessStatusCode();
    }

    public HttpClient GetLoggingHttpClient(HttpLoggingType loggingType)
    {
        return _loggingHttpClients.GetOrAdd(loggingType, _ =>
        {
            var moduleLogger = _moduleLoggerProvider.GetLogger();
            var serviceCollection = new ServiceCollection()
                .AddSingleton(moduleLogger)
                .AddTransient<SuccessHttpHandler>();

            var httpClientBuilder = serviceCollection
                .AddHttpClient<ModularPipelinesHttpClientProvider>()
                .AddHttpMessageHandler<SuccessHttpHandler>();

            if (loggingType.HasFlag(HttpLoggingType.Request))
            {
                serviceCollection
                    .AddTransient<RequestLoggingHttpHandler>();

                httpClientBuilder
                    .AddHttpMessageHandler<RequestLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.Response))
            {
                serviceCollection
                    .AddTransient<ResponseLoggingHttpHandler>();

                httpClientBuilder.AddHttpMessageHandler<ResponseLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.Duration))
            {
                serviceCollection
                    .AddTransient<DurationLoggingHttpHandler>();

                httpClientBuilder.AddHttpMessageHandler<DurationLoggingHttpHandler>();
            }

            if (loggingType.HasFlag(HttpLoggingType.StatusCode))
            {
                serviceCollection
                    .AddTransient<StatusCodeLoggingHttpHandler>();

                httpClientBuilder.AddHttpMessageHandler<StatusCodeLoggingHttpHandler>();
            }

            return serviceCollection.BuildServiceProvider()
                .GetRequiredService<ModularPipelinesHttpClientProvider>()
                .HttpClient;
        });
    }

    public void Dispose()
    {
        HttpClient.Dispose();

        foreach (var httpClient in _loggingHttpClients.Values)
        {
            httpClient.Dispose();
        }
    }

    private async Task<HttpResponseMessage> SendAndWrapLogging(HttpOptions httpOptions, CancellationToken cancellationToken)
    {
        var logger = _moduleLoggerProvider.GetLogger();

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Request))
        {
            await HttpLogger.PrintRequest(httpOptions.HttpRequestMessage, logger);
        }

        var stopWatch = Stopwatch.StartNew();

        var response = await httpOptions.HttpClient!.SendAsync(httpOptions.HttpRequestMessage, cancellationToken);

        LogStatusCode(response.StatusCode, httpOptions);
        LogDuration(stopWatch.Elapsed, httpOptions);

        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Response))
        {
            await HttpLogger.PrintResponse(response, logger);
        }
        
        if (!httpOptions.ThrowOnNonSuccessStatusCode)
        {
            return response;
        }

        return response.EnsureSuccessStatusCode();
    }

    private void LogDuration(TimeSpan duration, HttpOptions httpOptions)
    {
        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.Duration))
        {
            HttpLogger.PrintDuration(duration, _moduleLoggerProvider.GetLogger());
        }
    }

    private void LogStatusCode(HttpStatusCode? httpStatusCode, HttpOptions httpOptions)
    {
        if (httpOptions.LoggingType.HasFlag(HttpLoggingType.StatusCode))
        {
            HttpLogger.PrintStatusCode(httpStatusCode, _moduleLoggerProvider.GetLogger());
        }
    }
}