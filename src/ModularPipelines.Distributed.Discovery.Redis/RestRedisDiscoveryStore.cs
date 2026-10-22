using System.Net.Http.Headers;
using System.Text.Json;

namespace ModularPipelines.Distributed.Discovery.Redis;

internal sealed class RestRedisDiscoveryStore : IRedisDiscoveryStore, IDisposable
{
    private readonly HttpClient _httpClient;

    public RestRedisDiscoveryStore(Uri restUrl, string restToken)
        : this(restUrl, restToken, new HttpClientHandler())
    {
    }

    internal RestRedisDiscoveryStore(Uri restUrl, string restToken, HttpMessageHandler handler)
    {
        if (!RedisDiscoveryOptionsValidator.IsSupportedRestEndpoint(restUrl))
        {
            throw new ArgumentException("REST discovery requires HTTPS, except for HTTP on loopback, without a query string or fragment.", nameof(restUrl));
        }

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(restUrl.OriginalString.TrimEnd('/') + "/", UriKind.Absolute),
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", restToken);
    }

    public async Task SetAsync(
        string key,
        string value,
        TimeSpan ttl,
        CancellationToken cancellationToken)
    {
        var path = $"set/{Encode(key)}?EX={(long) ttl.TotalSeconds}";
        using var request = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(value),
        };
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

    public Task<bool> RefreshAsync(string key, string expectedValue, TimeSpan ttl, CancellationToken cancellationToken) =>
        ExecuteScriptAsync(RedisDiscoveryScripts.Refresh, key, [expectedValue, ((long) ttl.TotalMilliseconds).ToString(System.Globalization.CultureInfo.InvariantCulture)], cancellationToken);

    public async Task DeleteAsync(string key, string expectedValue, CancellationToken cancellationToken) =>
        await ExecuteScriptAsync(RedisDiscoveryScripts.Delete, key, [expectedValue], cancellationToken).ConfigureAwait(false);

    private async Task<bool> ExecuteScriptAsync(string script, string key, string[] arguments, CancellationToken cancellationToken)
    {
        // Keep the expected value (which includes the access token) out of URLs and HTTP error messages.
        using var request = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = new StringContent(JsonSerializer.Serialize(new[] { "EVAL", script, "1", key }.Concat(arguments)),
                System.Text.Encoding.UTF8, "application/json"),
        };
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        await using var streamScope = stream.ConfigureAwait(false);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
        return document.RootElement.GetProperty("result").GetInt64() == 1;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private static string Encode(string value) => Uri.EscapeDataString(value);
}
