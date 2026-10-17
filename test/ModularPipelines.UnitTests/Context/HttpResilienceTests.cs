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
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task UnderstatedContentLengthDoesNotBypassRetryLimit(bool prebuffered, bool optIn)
    {
        var payload = new byte[(1024 * 1024) + 1];
        using var content = new ByteArrayContent(payload);
        if (prebuffered)
        {
            await content.LoadIntoBufferAsync();
        }

        content.Headers.ContentLength = 1;
        var inner = new RetryHandler(readContent: true);
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
        await Assert.That(inner.Payloads[0].SequenceEqual(payload)).IsTrue();
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadGateway);
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

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task ClonedRequestsAreDisposedAfterSuccessOrRetryExhaustion(int rejectedAttempts)
    {
        var inner = new DisposalTrackingHandler(rejectedAttempts);
        using var invoker = CreateInvoker(inner);
        using var content = new DisposalTrackingContent();
        using var request = new HttpRequestMessage(HttpMethod.Put, "https://example.test/retry")
        {
            Content = content,
        };

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.Requests.Count).IsEqualTo(Math.Min(rejectedAttempts + 1, 2));
        await Assert.That(response.StatusCode).IsEqualTo(rejectedAttempts < 2 ? HttpStatusCode.OK : HttpStatusCode.BadGateway);
        await AssertClonesDisposed(inner, request);
        await Assert.That(content.IsDisposed).IsFalse();
        request.Content = content;
        await Assert.That(await content.ReadAsStringAsync()).IsEqualTo("payload");
        await Assert.That(await response.Content.ReadAsStringAsync()).IsEqualTo("response body");
        await Assert.That(response.RequestMessage).IsSameReferenceAs(request);
        await Assert.That(await response.RequestMessage!.Content!.ReadAsStringAsync()).IsEqualTo("payload");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ClonedRequestsAreDisposedAfterFailureOrCancellation(bool cancel)
    {
        using var cancellation = new CancellationTokenSource();
        var inner = new DisposalTrackingHandler(fail: true, cancellation: cancel ? cancellation : null);
        using var invoker = CreateInvoker(inner);
        using var content = new DisposalTrackingContent();
        using var request = new HttpRequestMessage(HttpMethod.Put, "https://example.test/retry")
        {
            Content = content,
        };

        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() => invoker.SendAsync(request, cancellation.Token));
        }
        else
        {
            await Assert.ThrowsAsync<HttpRequestException>(() => invoker.SendAsync(request, cancellation.Token));
        }

        await Assert.That(inner.Requests.Count).IsEqualTo(cancel ? 1 : 2);
        await AssertClonesDisposed(inner, request);
        await Assert.That(content.IsDisposed).IsFalse();
        request.Content = content;
        await Assert.That(await content.ReadAsStringAsync()).IsEqualTo("payload");
    }

    [Test]
    [Arguments("disabled")]
    [Arguments("method")]
    [Arguments("opt-out")]
    [Arguments("large")]
    [Arguments("understated")]
    public async Task UnclonedRequestsRemainCallerOwned(string bypass)
    {
        var inner = new RetryHandler(readContent: true);
        using var invoker = CreateInvoker(inner, maxRetries: bypass == "disabled" ? 0 : 1);
        using var content = new DisposalTrackingContent(bypass == "understated" ? new byte[(1024 * 1024) + 1] : null);
        if (bypass == "large")
        {
            content.Headers.ContentLength = (1024 * 1024) + 1;
        }
        else if (bypass == "understated")
        {
            content.Headers.ContentLength = 1;
        }

        using var request = new HttpRequestMessage(bypass == "method" ? HttpMethod.Post : HttpMethod.Put, "https://example.test/retry")
        {
            Content = content,
        };
        if (bypass == "opt-out")
        {
            request.Options.Set(HttpResilienceOptions.RetryEnabled, false);
        }

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(inner.Requests.Count).IsEqualTo(1);
        await Assert.That(inner.Requests[0]).IsSameReferenceAs(request);
        await Assert.That(content.IsDisposed).IsFalse();
        request.Content = content;
        await Assert.That((await content.ReadAsByteArrayAsync()).Length).IsEqualTo(bypass == "understated" ? (1024 * 1024) + 1 : 7);
    }

    [Test]
    public async Task IndependentlySuppliedResponseRequestIsPreserved()
    {
        using var responseRequest = new HttpRequestMessage(HttpMethod.Get, "https://example.test/redirected");
        var inner = new DisposalTrackingHandler(responseRequest: responseRequest);
        using var invoker = CreateInvoker(inner);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/retry");

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        await Assert.That(response.RequestMessage).IsSameReferenceAs(responseRequest);
        await AssertClonesDisposed(inner, request);
    }

    private static async Task AssertClonesDisposed(DisposalTrackingHandler inner, HttpRequestMessage original)
    {
        foreach (var request in inner.Requests)
        {
            await Assert.That(ReferenceEquals(request, original)).IsFalse();
            await Assert.That(((DisposalTrackingContent) request.Content!).IsDisposed).IsTrue();
            Assert.Throws<ObjectDisposedException>(() => request.Content = null);
        }
    }

    private sealed class DisposalTrackingHandler(
        int rejectedAttempts = 0,
        bool fail = false,
        CancellationTokenSource? cancellation = null,
        HttpRequestMessage? responseRequest = null) : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Track content installed by an inner handler as well as the cloned request itself.
            request.Content?.Dispose();
            request.Content = new DisposalTrackingContent();
            Requests.Add(request);

            await Task.Yield();
            await Assert.That(((DisposalTrackingContent) request.Content).IsDisposed).IsFalse();
            cancellation?.Cancel();
            cancellationToken.ThrowIfCancellationRequested();
            if (fail)
            {
                throw new HttpRequestException("Terminal send failure.");
            }

            return new HttpResponseMessage(Requests.Count <= rejectedAttempts ? HttpStatusCode.BadGateway : HttpStatusCode.OK)
            {
                RequestMessage = responseRequest ?? request,
                Content = new StringContent("response body"),
            };
        }
    }

    private sealed class DisposalTrackingContent(byte[]? bytes = null) : ByteArrayContent(bytes ?? "payload"u8.ToArray())
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
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
