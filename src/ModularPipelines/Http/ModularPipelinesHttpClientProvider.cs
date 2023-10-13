using System.Net.Http;

namespace ModularPipelines.Http;

internal class ModularPipelinesHttpClientProvider(HttpClient httpClient)
{
    public HttpClient HttpClient { get; } = httpClient;
}