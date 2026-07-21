using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

public class RestRedisDiscoveryStoreTests
{
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
            "https://redis.example/",
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
        using var store = new RestRedisDiscoveryStore("https://redis.example", "secret-token", handler);

        var result = await store.GetAsync("test:key", CancellationToken.None);

        await Assert.That(result).IsEqualTo("https://master.example");
    }

    [Test]
    public async Task GetAsync_Returns_Null_When_Key_Does_Not_Exist()
    {
        var handler = new StubHttpMessageHandler(_ => JsonResponse("{\"result\":null}"));
        using var store = new RestRedisDiscoveryStore("https://redis.example", "secret-token", handler);

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
