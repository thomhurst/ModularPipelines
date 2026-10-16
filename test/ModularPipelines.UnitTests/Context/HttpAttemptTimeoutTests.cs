using System.Net;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class HttpAttemptTimeoutTests
{
    private static readonly TimeSpan AttemptTimeout = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(10);

    [Test]
    public async Task AttemptTimeout_RetriesWithFreshToken_AndDisposesSuccessfulAttemptTimer()
    {
        var time = new FakeTimeProvider();
        using var cancellation = new CancellationTokenSource();
        var attempts = Channel.CreateUnbounded<CancellationToken>();
        var callCount = 0;
        using var handler = CreateHandler(new HttpResilienceOptions
        {
            AttemptTimeout = AttemptTimeout,
            InitialDelay = TimeSpan.Zero,
        }, time, async (_, token) =>
        {
            attempts.Writer.TryWrite(token);
            if (Interlocked.Increment(ref callCount) == 1)
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        using var invoker = new HttpMessageInvoker(handler);
        using var request = CreateRequest();
        var send = invoker.SendAsync(request, cancellation.Token);
        var firstToken = await attempts.Reader.ReadAsync().AsTask().WaitAsync(TestTimeout);

        time.Advance(AttemptTimeout);
        using var response = await send.WaitAsync(TestTimeout);
        var secondToken = await attempts.Reader.ReadAsync().AsTask().WaitAsync(TestTimeout);
        cancellation.Cancel();
        time.Advance(AttemptTimeout);

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(callCount).IsEqualTo(2);
        await Assert.That(firstToken.IsCancellationRequested).IsTrue();
        await Assert.That(secondToken == firstToken).IsFalse();
        await Assert.That(secondToken.IsCancellationRequested).IsFalse();
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async Task AttemptTimeout_StopsAtRetryLimit(int maxRetries)
    {
        var time = new FakeTimeProvider();
        var attempts = Channel.CreateUnbounded<CancellationToken>();
        using var handler = CreateHandler(new HttpResilienceOptions
        {
            AttemptTimeout = AttemptTimeout,
            MaxRetryAttempts = maxRetries,
            InitialDelay = TimeSpan.Zero,
        }, time, async (_, token) =>
        {
            attempts.Writer.TryWrite(token);
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        using var invoker = new HttpMessageInvoker(handler);
        using var request = CreateRequest();
        var send = invoker.SendAsync(request, CancellationToken.None);

        for (var i = 0; i <= maxRetries; i++)
        {
            await attempts.Reader.ReadAsync().AsTask().WaitAsync(TestTimeout);
            time.Advance(AttemptTimeout);
        }

        var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => send.WaitAsync(TestTimeout));
        await Assert.That(exception!.InnerException).IsTypeOf<TimeoutException>();
        await Assert.That(attempts.Reader.TryRead(out _)).IsFalse();
    }

    [Test]
    public async Task AttemptTimeout_DoesNotRetryWhenDisabled()
    {
        var time = new FakeTimeProvider();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;
        using var handler = CreateHandler(new HttpResilienceOptions
        {
            AttemptTimeout = AttemptTimeout,
            RetryOnTimeout = false,
            InitialDelay = TimeSpan.Zero,
        }, time, async (_, token) =>
        {
            Interlocked.Increment(ref calls);
            started.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        using var invoker = new HttpMessageInvoker(handler);
        using var request = CreateRequest();
        var send = invoker.SendAsync(request, CancellationToken.None);
        await started.Task.WaitAsync(TestTimeout);

        time.Advance(AttemptTimeout);

        await Assert.ThrowsAsync<TaskCanceledException>(() => send.WaitAsync(TestTimeout));
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    [Arguments("POST")]
    [Arguments("opt-out")]
    [Arguments("stream")]
    [Arguments("large")]
    [Arguments("understated")]
    public async Task AttemptTimeout_AppliesWhenRequestCannotBeRetried(string policy)
    {
        var time = new FakeTimeProvider();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;
        using var request = CreateRequest();
        switch (policy)
        {
            case "POST":
                request.Method = HttpMethod.Post;
                break;
            case "opt-out":
                request.Options.Set(HttpResilienceOptions.RetryEnabled, false);
                break;
            case "stream":
                request.Content = new StreamContent(new MemoryStream([1, 2, 3]));
                break;
            default:
                request.Content = new ByteArrayContent(new byte[(1024 * 1024) + 1]);
                if (policy == "understated")
                {
                    request.Content.Headers.ContentLength = 1;
                }

                break;
        }

        using var handler = CreateHandler(new HttpResilienceOptions
        {
            AttemptTimeout = AttemptTimeout,
            InitialDelay = TimeSpan.Zero,
        }, time, async (sentRequest, token) =>
        {
            Interlocked.Increment(ref calls);
            started.TrySetResult();
            await Assert.That(sentRequest).IsSameReferenceAs(request);
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        using var invoker = new HttpMessageInvoker(handler);
        var send = invoker.SendAsync(request, CancellationToken.None);
        await started.Task.WaitAsync(TestTimeout);

        time.Advance(AttemptTimeout);

        var exception = await Assert.ThrowsAsync<TaskCanceledException>(() => send.WaitAsync(TestTimeout));
        await Assert.That(exception!.InnerException).IsTypeOf<TimeoutException>();
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    [Arguments(true, false)]
    [Arguments(false, false)]
    [Arguments(true, true)]
    [Arguments(false, true)]
    public async Task OperationCancellation_StopsSendAndBackoff(bool callerCancellation, bool duringBackoff)
    {
        var time = new FakeTimeProvider();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var retrying = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var calls = 0;
        var retries = 0;
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Callback(() =>
            {
                Interlocked.Increment(ref retries);
                retrying.TrySetResult();
            });
        var options = new PipelineOptions
        {
            Http = new PipelineHttpOptions
            {
                Timeout = TimeSpan.FromMinutes(1),
                Resilience = new HttpResilienceOptions
                {
                    AttemptTimeout = duringBackoff ? AttemptTimeout : TimeSpan.FromMinutes(2),
                    InitialDelay = TimeSpan.FromDays(1),
                    JitterFactor = 0,
                },
            },
        };
        using var handler = CreateHandler(options.Http.Resilience, time, async (_, token) =>
        {
            Interlocked.Increment(ref calls);
            started.TrySetResult();
            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
            }
            catch (OperationCanceledException exception)
            {
                throw new TaskCanceledException("Transport timeout", new TimeoutException("Transport timeout", exception), token);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }, logger.Object);
        using var client = new HttpClient(handler) { Timeout = Timeout.InfiniteTimeSpan };
        var http = new ModularPipelines.Http.Http(Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(), Mock.Of<IHttpLogger>(), Microsoft.Extensions.Options.Options.Create(options), time);
        using var cancellation = new CancellationTokenSource();
        using var request = CreateRequest();
        var send = http.SendAsync(new HttpOptions(request) { HttpClient = client }, cancellation.Token);
        await started.Task.WaitAsync(TestTimeout);
        if (duringBackoff)
        {
            time.Advance(AttemptTimeout);
            await retrying.Task.WaitAsync(TestTimeout);
        }

        if (callerCancellation)
        {
            cancellation.Cancel();
        }
        else
        {
            time.Advance(TimeSpan.FromMinutes(1));
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => send.WaitAsync(TestTimeout));
        await Assert.That(calls).IsEqualTo(1);
        await Assert.That(retries).IsEqualTo(duringBackoff ? 1 : 0);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task UnsetAttemptTimeout_PreservesTransportCancellationBehavior(bool timeout)
    {
        var calls = 0;
        using var handler = CreateHandler(new HttpResilienceOptions { InitialDelay = TimeSpan.Zero },
            new FakeTimeProvider(), (_, _) =>
            {
                if (++calls == 1)
                {
                    throw new TaskCanceledException("Transport cancelled", timeout ? new TimeoutException() : null);
                }

                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });
        using var invoker = new HttpMessageInvoker(handler);
        using var request = CreateRequest();
        if (timeout)
        {
            using var response = await invoker.SendAsync(request, CancellationToken.None);
            await Assert.That(calls).IsEqualTo(2);
        }
        else
        {
            await Assert.ThrowsAsync<TaskCanceledException>(() => invoker.SendAsync(request, CancellationToken.None));
            await Assert.That(calls).IsEqualTo(1);
        }
    }

    [Test]
    [Arguments(0d)]
    [Arguments(-1d)]
    [Arguments(-2d)]
    [Arguments(4294967295d)]
    public async Task AttemptTimeout_RejectsInvalidValues(double milliseconds)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Task.FromResult(new HttpResilienceOptions
        {
            AttemptTimeout = TimeSpan.FromMilliseconds(milliseconds),
        }));
    }

    private static HttpRequestMessage CreateRequest() => new(HttpMethod.Get, "https://example.test/timeout");

    private static ResilienceHttpHandler CreateHandler(HttpResilienceOptions options, TimeProvider time,
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send, ILogger? logger = null)
    {
        var accessor = new Mock<IModuleLoggerAccessor>();
        accessor.Setup(x => x.Logger).Returns(logger ?? Mock.Of<ILogger>());
        return new ResilienceHttpHandler(accessor.Object, Microsoft.Extensions.Options.Options.Create(new PipelineOptions
        {
            Http = new PipelineHttpOptions { Resilience = options },
        }), time)
        {
            InnerHandler = new CallbackHandler(send),
        };
    }

    private sealed class CallbackHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => send(request, cancellationToken);
    }
}
