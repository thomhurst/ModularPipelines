using System.Net;
using System.Net.Http.Headers;
using System.Text;
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
    public async Task SetAsync_Sends_Authenticated_Command_With_Ttl()
    {
        HttpRequestMessage? capturedRequest = null;
        var handler = new StubHttpMessageHandler(request =>
        {
            capturedRequest = request;
            return JsonResponse("{\"result\":\"OK\"}");
        });
        using var store = new RestRedisDiscoveryStore(
            new Uri("https://redis.example/"),
            "secret-token",
            handler);

        await store.SetAsync("test:key", "https://master.example", TimeSpan.FromSeconds(60), CancellationToken.None);

        await Assert.That(capturedRequest).IsNotNull();
        await Assert.That(capturedRequest!.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(capturedRequest.RequestUri!.AbsoluteUri)
            .IsEqualTo("https://redis.example/set/test%3Akey/https%3A%2F%2Fmaster.example/ex/60");
        await Assert.That(capturedRequest.Headers.Authorization)
            .IsEqualTo(new AuthenticationHeaderValue("Bearer", "secret-token"));
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
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(responseFactory(request));
        }
    }
}
