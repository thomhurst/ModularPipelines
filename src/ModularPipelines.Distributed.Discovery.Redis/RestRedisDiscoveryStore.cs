using System.Net.Http.Headers;
using System.Text.Json;

namespace ModularPipelines.Distributed.Discovery.Redis;

internal sealed class RestRedisDiscoveryStore : IRedisDiscoveryStore, IDisposable
{
    private readonly HttpClient _httpClient;

    public RestRedisDiscoveryStore(string restUrl, string restToken)
        : this(restUrl, restToken, new HttpClientHandler())
    {
    }

    internal RestRedisDiscoveryStore(string restUrl, string restToken, HttpMessageHandler handler)
    {
        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(restUrl.TrimEnd('/') + "/", UriKind.Absolute),
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", restToken);
    }

    public async Task SetAsync(
        string key,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        var path = $"set/{Encode(key)}/{Encode(value)}/ex/{(long) ttl.TotalSeconds}";
        using var request = new HttpRequestMessage(HttpMethod.Post, path);
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken)
    {
        using var response = await _httpClient.GetAsync($"get/{Encode(key)}", cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        await using var streamScope = stream.ConfigureAwait(false);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        var result = document.RootElement.GetProperty("result");

        return result.ValueKind == JsonValueKind.Null ? null : result.GetString();
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
