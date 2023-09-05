﻿using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Logging;
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
        if (httpOptions.LogRequest)
        {
            await PrintRequest(httpOptions.HttpRequestMessage);
        }

        var response = await (httpOptions.HttpClient ?? HttpClient).SendAsync(httpOptions.HttpRequestMessage);

        if (httpOptions.LogResponse)
        {
            await PrintResponse(response);
        }

        return response.EnsureSuccessStatusCode();
    }

    public async Task PrintRequest(HttpRequestMessage request)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"{request.Method} {request.RequestUri} HTTP/{request.Version}");

        sb.AppendLine();

        PrintHeaders(sb, request.Headers, request.Content?.Headers);

        sb.AppendLine();

        await PrintBody(sb, request.Content);

        _moduleLoggerProvider.GetLogger().LogInformation("---Request---\r\n{Request}", sb.ToString());
    }

    public async Task PrintResponse(HttpResponseMessage response)
    {
        var sb = new StringBuilder();

        var statusCode = (int) response.StatusCode;

        sb.AppendLine($"HTTP/{response.Version} {statusCode} {response.ReasonPhrase}");

        sb.AppendLine();

        PrintHeaders(sb, response.Headers, response.Content.Headers);

        sb.AppendLine();

        await PrintBody(sb, response.Content);

        _moduleLoggerProvider.GetLogger().LogInformation("---Response---\r\n{Response}", sb.ToString());
    }

    private static void PrintHeaders(StringBuilder sb, HttpHeaders baseHeaders, HttpHeaders? contentHeaders)
    {
        sb.AppendLine("Headers");
        foreach (var (key, values) in baseHeaders)
        {
            foreach (var value in values)
            {
                sb.AppendLine($"\t{key}: {value}");
            }
        }

        var contentHeadersArray = contentHeaders as IEnumerable<KeyValuePair<string, IEnumerable<string>>> ?? Array.Empty<KeyValuePair<string, IEnumerable<string>>>();

        foreach (var (key, values) in contentHeadersArray)
        {
            foreach (var value in values)
            {
                sb.AppendLine($"\t{key}: {value}");
            }
        }

        if (!baseHeaders.Any() && (!contentHeaders?.Any() ?? true))
        {
            sb.AppendLine("\t(null)");
        }
    }

    private static async Task PrintBody(StringBuilder sb, HttpContent? content)
    {
        sb.AppendLine("Body");
        var body = await (content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty));
        if (!string.IsNullOrWhiteSpace(body))
        {
            sb.AppendLine($"\t{body}");
        }
        else
        {
            sb.AppendLine("\t(null)");
        }
    }
}
