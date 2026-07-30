using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Helpers;
using Moq;
using NReco.Logging.File;
using File = System.IO.File;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Context;

public class HttpTests : TestBase
{
    [Test]
    public async Task SendAsync_ReturnsAfterHeadersWithoutBufferingResponseBody()
    {
        var content = new BlockingHttpContent();
        var handler = new ImmediateResponseHandler(content);
        using var httpClient = new HttpClient(handler);
        var result = await GetService<IHttpContext>((_, _) => { });
        var sendTask = result.T.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/large-file"))
        {
            HttpClient = httpClient,
            LoggingType = HttpLoggingType.None,
        });

        await handler.RequestReceived.Task.WaitAsync(TimeSpan.FromSeconds(5));

        HttpResponseMessage? response = null;
        try
        {
            response = await sendTask.WaitAsync(TimeSpan.FromSeconds(1));
        }
        finally
        {
            content.Release();
            response ??= await sendTask;
            response.Dispose();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task SendAsync_ConfiguredTimeoutCancelsStreamedBody(bool useRequestTimeout)
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream();
        using var httpClient = new HttpClient(new ImmediateResponseHandler(new StreamContent(contentStream)));
        var result = await GetService<IHttpContext>((builder, _) =>
            builder.ConfigurePipelineOptions(options => options with
            {
                DefaultHttpTimeout = useRequestTimeout ? null : timeout,
            }));
        using var response = await result.T.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-body"))
        {
            HttpClient = httpClient,
            LoggingType = HttpLoggingType.None,
            Timeout = useRequestTimeout ? timeout : null,
        });
        var stream = await response.Content.ReadAsStreamAsync();
        var readTask = stream.ReadAsync(new byte[1]).AsTask();

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TimeSpan.FromSeconds(1)));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    public async Task SendAsync_ConfiguredTimeoutCancelsFactoryResponseLogging()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream();
        var content = new StreamContent(contentStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        var moduleLoggerProvider = new Mock<IModuleLoggerProvider>();
        moduleLoggerProvider
            .Setup(x => x.GetLogger())
            .Returns(Mock.Of<IModuleLogger>());
        var httpLogger = new HttpLogger(
            Mock.Of<IHttpRequestFormatter>(),
            new HttpResponseFormatter(
                Mock.Of<ISecretObfuscator>(),
                Mock.Of<ISecretProvider>(),
                Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions())));
        var responseLoggingHandler = new ResponseLoggingHttpHandler(
            moduleLoggerProvider.Object,
            httpLogger)
        {
            InnerHandler = new ImmediateResponseHandler(content),
        };
        using var httpClient = new HttpClient(responseLoggingHandler);
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var http = new ModularPipelines.Http.Http(
            httpClientFactory.Object,
            moduleLoggerProvider.Object,
            httpLogger,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await http.SendAsync(new HttpOptions(
                        new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-log-body"))
                    {
                        LoggingType = HttpLoggingType.Response,
                        Timeout = timeout,
                    })
                    .WaitAsync(TimeSpan.FromSeconds(1)));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    public async Task ResilienceHandler_DisposesRetryableResponseBeforeNextAttempt()
    {
        var retryContent = new TrackingStringContent("retry");
        var innerHandler = new SequenceResponseHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
            {
                Content = retryContent,
            },
            new HttpResponseMessage(HttpStatusCode.OK));
        var moduleLoggerProvider = new Mock<IModuleLoggerProvider>();
        moduleLoggerProvider
            .Setup(x => x.GetLogger())
            .Returns(Mock.Of<IModuleLogger>());
        using var handler = new ResilienceHttpHandler(
            moduleLoggerProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                DefaultHttpResilienceOptions = new HttpResilienceOptions
                {
                    MaxRetryAttempts = 1,
                    InitialDelay = TimeSpan.Zero,
                    JitterFactor = 0,
                },
            }))
        {
            InnerHandler = innerHandler,
        };
        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test/retry");
        using var response = await invoker.SendAsync(request, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            await Assert.That(innerHandler.CallCount).IsEqualTo(2);
            await Assert.That(retryContent.IsDisposed).IsTrue();
        }
    }

    [Test]
    public async Task PublicApi_DoesNotExposeRawHttpClients()
    {
        var rawClientMembers = typeof(IHttpContext)
            .GetMembers()
            .Where(member => member switch
            {
                PropertyInfo property => property.PropertyType == typeof(HttpClient),
                MethodInfo method => method.ReturnType == typeof(HttpClient),
                _ => false,
            })
            .Select(member => member.Name);

        await Assert.That(rawClientMembers).IsEmpty();
    }

    [Test]
    public async Task Can_Send_Request_With_String_To_Request_Implicit_Conversion()
    {
        await using var server = LocalHttpServer.Start();
        var (http, _) = await GetService<IHttpContext>((_, _) => { });

        await http.SendAsync(server.Uri);
    }

    [Test]
    public async Task When_Log_Request_False_Then_Do_Not_Log_Request()
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
        {
            ThrowOnNonSuccessStatusCode = false,
            LoggingType = HttpLoggingType.Response,
        });

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).DoesNotContain("HTTP Request:");
        await Assert.That(logFile).DoesNotContain($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).Contains("HTTP Response:");
        await Assert.That(logFile).Contains("Server: LocalHttpServer");
    }

    [Test]
    public async Task When_Log_Response_False_Then_Do_Not_Log_Response()
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
        {
            ThrowOnNonSuccessStatusCode = false,
            LoggingType = HttpLoggingType.Request,
        });

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains("HTTP Request:");
        await Assert.That(logFile).Contains($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).DoesNotContain("HTTP Response:");
        await Assert.That(logFile).DoesNotContain("Server: LocalHttpServer");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Assert_SendAsync_Logs_As_Expected(bool customHttpClient)
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>((_, collection) =>
        {
            collection.AddLogging(builder =>
            {
                collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                builder.AddFile(file);
            });
        });

        if (!customHttpClient)
        {
            await http.SendAsync(server.Uri);
        }
        else
        {
            await http.SendAsync(new HttpOptions(new HttpRequestMessage(HttpMethod.Get, server.Uri))
            {
                ThrowOnNonSuccessStatusCode = false,
                HttpClient = new HttpClient()
            });
        }

        await host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains("HTTP Request:");
        await Assert.That(logFile).Contains($"GET {server.Uri} HTTP/1.1");
        await Assert.That(logFile).Contains("HTTP Response:");
        await Assert.That(logFile).Contains("Headers");
        await Assert.That(logFile).Contains("Server: LocalHttpServer");
        await Assert.That(logFile).Contains("Body");
        await Assert.That(logFile).Contains("Duration:");
        await Assert.That(logFile).Contains("HTTP Status:");

        var logFileLines = (await File.ReadAllLinesAsync(file)).ToList();

        var indexOfRequest = logFileLines.FindIndex(x => x.Contains("HTTP Request:"));
        var indexOfStatusCode = logFileLines.FindIndex(x => x.Contains("HTTP Status:"));
        var indexOfDuration = logFileLines.FindIndex(x => x.Contains("Duration:"));
        var indexOfResponse = logFileLines.FindIndex(x => x.Contains("HTTP Response:"));

        using (Assert.Multiple())
        {
            await Assert.That(indexOfRequest).IsLessThan(indexOfStatusCode);
            await Assert.That(indexOfStatusCode).IsLessThan(indexOfDuration);
            await Assert.That(indexOfDuration).IsLessThan(indexOfResponse);
        }
    }

    private sealed class ImmediateResponseHandler(HttpContent content) : HttpMessageHandler
    {
        public TaskCompletionSource RequestReceived { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestReceived.TrySetResult();
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content,
                RequestMessage = request,
            });
        }
    }

    private sealed class BlockingHttpContent : HttpContent
    {
        private readonly TaskCompletionSource _release =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void Release() => _release.TrySetResult();

        protected override async Task SerializeToStreamAsync(
            Stream stream,
            TransportContext? context)
        {
            await _release.Task;
            await stream.WriteAsync("response body"u8.ToArray());
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }

    private sealed class BlockingReadStream : MemoryStream
    {
        private readonly TaskCompletionSource _release =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void Release() => _release.TrySetResult();

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            await _release.Task.WaitAsync(cancellationToken);
            return 0;
        }
    }

    private sealed class SequenceResponseHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(responses[CallCount++]);
        }
    }

    private sealed class TrackingStringContent(string content) : StringContent(content)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
