using System.Net;
using Microsoft.Extensions.Logging;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpResilienceTests
{
    [Test]
    [Arguments("GET", true)]
    [Arguments("HEAD", true)]
    [Arguments("PUT", true)]
    [Arguments("DELETE", true)]
    [Arguments("OPTIONS", true)]
    [Arguments("TRACE", true)]
    [Arguments("POST", false)]
    [Arguments("PATCH", false)]
    [Arguments("CUSTOM", false)]
    public async Task RetriesOnlyIdempotentMethodsByDefault(string method, bool retries)
    {
        var inner = new RetryHandler();
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(new HttpMethod(method), "https://example.test/retry");

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(retries ? 2 : 1);
        await Assert.That(response.StatusCode).IsEqualTo(retries ? HttpStatusCode.OK : HttpStatusCode.BadGateway);
    }

    [Test]
    [Arguments("POST", true)]
    [Arguments("PATCH", true)]
    [Arguments("CUSTOM", true)]
    [Arguments("GET", false)]
    [Arguments("PUT", false)]
    public async Task RequestOptionOverridesMethodDefault(string method, bool enabled)
    {
        var inner = new RetryHandler(readContent: true);
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(new HttpMethod(method), "https://example.test/retry")
        {
            Content = new StringContent("payload"),
        };
        request.Options.Set(HttpResilienceOptions.RetryEnabled, enabled);

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(enabled ? 2 : 1);
        foreach (var payload in inner.Payloads)
        {
            await Assert.That(payload.SequenceEqual("payload"u8.ToArray())).IsTrue();
        }

        foreach (var sentRequest in inner.Requests)
        {
            await Assert.That(sentRequest.Options.TryGetValue(HttpResilienceOptions.RetryEnabled, out var sentEnabled)).IsTrue();
            await Assert.That(sentEnabled).IsEqualTo(enabled);
        }
    }

    [Test]
    [Arguments("GET", false, "network", true)]
    [Arguments("POST", false, "network", false)]
    [Arguments("POST", true, "network", true)]
    [Arguments("PATCH", false, "timeout", false)]
    [Arguments("PATCH", true, "timeout", true)]
    [Arguments("GET", false, "timeout", true)]
    public async Task MethodPolicyAlsoAppliesToExceptions(string method, bool optIn, string failure, bool retries)
    {
        var inner = new RetryHandler(failure);
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(new HttpMethod(method), "https://example.test/retry");
        if (optIn)
        {
            request.Options.Set(HttpResilienceOptions.RetryEnabled, true);
        }

        if (retries)
        {
            using var response = await invoker.SendAsync(request, CancellationToken.None);
            await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        }
        else if (failure == "network")
        {
            await Assert.ThrowsAsync<HttpRequestException>(() => invoker.SendAsync(request, CancellationToken.None));
        }
        else
        {
            await Assert.ThrowsAsync<TaskCanceledException>(() => invoker.SendAsync(request, CancellationToken.None));
        }

        await Assert.That(inner.CallCount).IsEqualTo(retries ? 2 : 1);
    }

    [Test]
    [Arguments("stream", false)]
    [Arguments("stream", true)]
    [Arguments("unknown", false)]
    [Arguments("unknown", true)]
    [Arguments("large", false)]
    [Arguments("large", true)]
    [Arguments("unbuffered", false)]
    [Arguments("unbuffered", true)]
    public async Task UnsafeBodiesAreSentUnchangedWithoutReadingOrRetrying(string body, bool optIn)
    {
        using var content = body switch
        {
            "stream" => (HttpContent) new StreamContent(new MemoryStream([1, 2, 3])),
            "unknown" => new UnknownLengthContent(),
            "large" => new ByteArrayContent([1, 2, 3]),
            _ => new UnbufferedContent(),
        };
        if (body == "large")
        {
            content.Headers.ContentLength = (long) int.MaxValue + 1;
        }

        var inner = new RetryHandler();
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(optIn ? HttpMethod.Post : HttpMethod.Put, "https://example.test/upload")
        {
            Content = content,
        };
        if (optIn)
        {
            request.Options.Set(HttpResilienceOptions.RetryEnabled, true);
        }

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(1);
        await Assert.That(inner.Requests[0]).IsSameReferenceAs(request);
        await Assert.That(inner.Requests[0].Content).IsSameReferenceAs(content);
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadGateway);
    }

    [Test]
    [Arguments(0, true)]
    [Arguments(1024 * 1024, true)]
    [Arguments((1024 * 1024) + 1, false)]
    public async Task BufferedBodiesHaveBoundedRetrySize(int length, bool retries)
    {
        var inner = new RetryHandler(readContent: true);
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(HttpMethod.Put, "https://example.test/upload")
        {
            Content = new ByteArrayContent(new byte[length]),
        };

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(retries ? 2 : 1);
        foreach (var payload in inner.Payloads)
        {
            await Assert.That(payload.Length).IsEqualTo(length);
        }
    }

    [Test]
    public async Task OptInDoesNotOverrideDisabledRetries()
    {
        var inner = new RetryHandler();
        using var invoker = CreateInvoker(inner, maxRetries: 0);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/retry")
        {
            Content = new UnbufferedContent(),
        };
        request.Options.Set(HttpResilienceOptions.RetryEnabled, true);

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(1);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task MethodPolicySkipsContentPreparation(bool optOut)
    {
        var inner = new RetryHandler();
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(optOut ? HttpMethod.Get : HttpMethod.Post, "https://example.test/retry")
        {
            Content = new UnbufferedContent(),
        };
        if (optOut)
        {
            request.Options.Set(HttpResilienceOptions.RetryEnabled, false);
        }

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.CallCount).IsEqualTo(1);
        await Assert.That(inner.Requests[0]).IsSameReferenceAs(request);
    }

    private static HttpMessageInvoker CreateInvoker(HttpMessageHandler inner, int maxRetries = 1)
    {
        var logger = new Mock<IModuleLoggerAccessor>();
        logger.Setup(x => x.Logger).Returns(Mock.Of<ILogger>());
        return new HttpMessageInvoker(new ResilienceHttpHandler(logger.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Http = new PipelineHttpOptions
                {
                    Resilience = new HttpResilienceOptions
                    {
                        MaxRetryAttempts = maxRetries,
                        InitialDelay = TimeSpan.Zero,
                        JitterFactor = 0,
                    },
                },
            }))
        {
            InnerHandler = inner,
        });
    }

    private sealed class RetryHandler(string? failure = null, bool readContent = false) : HttpMessageHandler
    {
        public int CallCount { get; private set; }
        public List<HttpRequestMessage> Requests { get; } = [];
        public List<byte[]> Payloads { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            Requests.Add(request);
            if (readContent && request.Content != null)
            {
                Payloads.Add(await request.Content.ReadAsByteArrayAsync(cancellationToken));
            }

            if (CallCount == 1)
            {
                if (failure == "network")
                {
                    throw new HttpRequestException("Connection lost after delivery.");
                }

                if (failure == "timeout")
                {
                    throw new TaskCanceledException("Request timed out after delivery.", new TimeoutException());
                }

                return new HttpResponseMessage(HttpStatusCode.BadGateway);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

    private sealed class UnknownLengthContent() : ByteArrayContent([1, 2, 3])
    {
        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }

    private sealed class UnbufferedContent : HttpContent
    {
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
            => throw new InvalidOperationException("The resilience handler must not read unbuffered content.");

        protected override bool TryComputeLength(out long length)
        {
            length = 3;
            return true;
        }
    }
}
