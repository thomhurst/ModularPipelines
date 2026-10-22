using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RestRedisDiscoveryStoreTests
{
    [Test]
    [Arguments("http://localhost:8079/base%20path")]
    [Arguments("https://redis.example/base%20path/")]
    [Arguments("https://redis.example/base%3Fkey%23fragment")]
    public async Task Endpoint_Preserves_Port_And_Escaped_Path_Prefix(string endpoint)
    {
        Uri? requestedUri = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            requestedUri = request.RequestUri;
            return JsonResponse("{\"result\":null}");
        });
        using var store = new RestRedisDiscoveryStore(new Uri(endpoint), "test-token", handler);

        await store.GetAsync("test:key", CancellationToken.None);

        await Assert.That(requestedUri!.AbsoluteUri).IsEqualTo(endpoint.TrimEnd('/') + "/get/test%3Akey");
    }

    [Test]
    [Arguments("http://redis.example")]
    [Arguments("relative/path")]
    [Arguments("https://redis.example/base?database=1")]
    [Arguments("https://redis.example/base#section")]
    [Arguments("https://redis.example/base?")]
    [Arguments("https://redis.example/base#")]
    [Arguments("https://redis.example/base?#")]
    [Arguments("http://localhost:8079/base?database=1#section")]
    public async Task Constructor_Rejects_Unsupported_Endpoint_Before_Sending_Token(string endpoint)
    {
        var sent = false;
        using var handler = new StubHttpMessageHandler(_ =>
        {
            sent = true;
            return JsonResponse("{\"result\":null}");
        });

        await Assert.That(() => new RestRedisDiscoveryStore(new Uri(endpoint, UriKind.RelativeOrAbsolute), "secret-token", handler))
            .Throws<ArgumentException>();
        await Assert.That(sent).IsFalse();
    }

    [Test]
    [Arguments("https://master.example")]
    [Arguments("""{"AccessToken":"master-secret/+?&#% 雪","Endpoint":"https://master.example"}""")]
    [Arguments("")]
    public async Task SetAsync_Sends_Value_In_Body_With_Authentication_And_Ttl(string value)
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return JsonResponse("{\"result\":\"OK\"}");
        });
        using var store = new RestRedisDiscoveryStore(
            new Uri("https://redis.example/base%20path/"),
            "secret-token",
            handler);

        await store.SetAsync("test:key", value, TimeSpan.FromSeconds(60), CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(capturedRequest.RequestUri!.AbsoluteUri)
            .IsEqualTo("https://redis.example/base%20path/set/test%3Akey?EX=60");
        await Assert.That(handler.RequestBody).IsEqualTo(value);
        await Assert.That(capturedRequest.Headers.Authorization)
            .IsEqualTo(new AuthenticationHeaderValue("Bearer", "secret-token"));
    }

    [Test]
    public async Task DeleteAsync_Sends_Authenticated_Conditional_Delete_Command()
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return JsonResponse("{\"result\":1}");
        });
        using var store = new RestRedisDiscoveryStore(new Uri("https://redis.example/"), "secret-token", handler);

        await store.DeleteAsync("test:key", "secret-value", CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(capturedRequest.RequestUri!.AbsoluteUri).IsEqualTo("https://redis.example/");
        await Assert.That(JsonSerializer.Deserialize<string[]>(handler.RequestBody!))
            .IsEquivalentTo(new[] { "EVAL", RedisDiscoveryScripts.Delete, "1", "test:key", "secret-value" });
        await Assert.That(capturedRequest.Headers.Authorization)
            .IsEqualTo(new AuthenticationHeaderValue("Bearer", "secret-token"));
    }

    [Test]
    [Arguments(0, false)]
    [Arguments(1, true)]
    public async Task Refresh_Sends_Atomic_Script_And_Secret_In_The_Body(int result, bool expected)
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return JsonResponse($"{{\"result\":{result}}}");
        });
        using var store = new RestRedisDiscoveryStore(new Uri("https://redis.example/base/"), "rest-token", handler);

        var refreshed = await store.RefreshAsync("test:key", "secret-token", TimeSpan.FromMilliseconds(1500), CancellationToken.None);

        await Assert.That(refreshed).IsEqualTo(expected);
        await Assert.That(capturedRequest!.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(capturedRequest.RequestUri!.AbsoluteUri).IsEqualTo("https://redis.example/base/");
        await Assert.That(capturedRequest.Headers.Authorization)
            .IsEqualTo(new AuthenticationHeaderValue("Bearer", "rest-token"));
        await Assert.That(JsonSerializer.Deserialize<string[]>(handler.RequestBody!))
            .IsEquivalentTo(new[] { "EVAL", RedisDiscoveryScripts.Refresh, "1", "test:key", "secret-token", "1500" });
    }

    [Test]
    public async Task GetAsync_Returns_Stored_Value()
    {
        var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"result\":\"https://master.example\"}"));
        using var store = new RestRedisDiscoveryStore(new Uri("https://redis.example"), "secret-token", handler);

        var result = await store.GetAsync("test:key", CancellationToken.None);

        await Assert.That(result).IsEqualTo("https://master.example");
    }

    [Test]
    public async Task GetAsync_Returns_Null_When_Key_Does_Not_Exist()
    {
        var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"result\":null}"));
        using var store = new RestRedisDiscoveryStore(new Uri("https://redis.example"), "secret-token", handler);

        var result = await store.GetAsync("test:key", CancellationToken.None);

        await Assert.That(result).IsNull();
    }

    private static HttpResponseMessage JsonResponse(string json)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
    }

    private sealed class StubHttpMessageHandler(
        Func<HttpRequestMessage, HttpResponseMessage> responseFactory) : HttpMessageHandler
    {
        public string? RequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
            return responseFactory(request);
        }
    }
}
