using ModularPipelines.Secrets;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
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
    private static HttpLoggingOptions RequestOnly { get; } = new()
    {
        LogResponse = false,
        LogStatusCode = false,
        LogDuration = false,
    };

    private static HttpLoggingOptions ResponseOnly { get; } = new()
    {
        LogRequest = false,
        LogStatusCode = false,
        LogDuration = false,
    };

    [Test]
    public async Task SendAsync_ReturnsAfterHeadersWithoutBufferingResponseBody()
    {
        var content = new BlockingHttpContent();
        var handler = new ImmediateResponseHandler(content);
        using var httpClient = new HttpClient(handler);
        var result = await GetService<IHttpContext>(_ => { });
        var sendTask = result.T.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/large-file"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
        });

        await handler.RequestReceived.Task.WaitAsync(TimeSpan.FromSeconds(5));

        HttpResponseMessage? response = null;
        try
        {
            response = await sendTask.WaitAsync(TestHostSettings.DefaultTestTimeout);
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
        var result = await GetServiceWithPipelineConfiguration<IHttpContext>(builder =>
            builder.ConfigurePipelineOptions(options => options with
            {
                Http = options.Http with
                {
                    Timeout = useRequestTimeout ? null : timeout,
                },
            }));
        using var response = await result.T.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-body"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = useRequestTimeout ? timeout : null,
        });
        var stream = await response.Content.ReadAsStreamAsync();
        var readTask = stream.ReadAsync(new byte[1]).AsTask();

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task SendAsync_HttpClientTimeoutCancelsStreamedBody(bool useCustomClient)
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream();
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)))
        {
            Timeout = timeout,
        };
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var http = new ModularPipelines.Http.Http(
            httpClientFactory.Object,
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/client-timeout"))
        {
            HttpClient = useCustomClient ? httpClient : null,
            Logging = HttpLoggingOptions.None,
        });
        var stream = response.Content.ReadAsStream();
        var readTask = stream.ReadAsync(new byte[1]).AsTask();

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    public async Task SendAsync_InfiniteTimeoutRetainsCallerCancellationForStreamedBody()
    {
        var contentStream = new BlockingReadStream();
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)))
        {
            Timeout = Timeout.InfiniteTimeSpan,
        };
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var cancellationTokenSource = new CancellationTokenSource();
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/caller-cancellation"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
        }, cancellationTokenSource.Token);
        var stream = response.Content.ReadAsStream();
        var readTask = stream.ReadAsync(new byte[1]).AsTask();

        cancellationTokenSource.Cancel();

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task SendAsync_NormalizesAsyncStreamFailureAfterTimeout(
        bool throwIOExceptionWhenDisposed)
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(
            throwIOExceptionWhenDisposed,
            ignoreAsyncCancellation: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/async-timeout"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = timeout,
        });
        var stream = response.Content.ReadAsStream();
        var readTask = stream.ReadAsync(new byte[1]).AsTask();

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task SendAsync_NormalizesAsyncStreamFailureAfterPerReadCancellation(
        bool throwIOExceptionWhenDisposed)
    {
        var contentStream = new BlockingReadStream(
            throwIOExceptionWhenDisposed,
            ignoreAsyncCancellation: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)))
        {
            Timeout = Timeout.InfiniteTimeSpan,
        };
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/read-cancellation"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = TimeSpan.FromMinutes(1),
        });
        var stream = response.Content.ReadAsStream();
        using var cancellationTokenSource = new CancellationTokenSource();
        var readTask = stream
            .ReadAsync(new byte[1], cancellationTokenSource.Token)
            .AsTask();

        await contentStream.ReadStarted.WaitAsync(TestHostSettings.DefaultTestTimeout);
        cancellationTokenSource.Cancel();

        try
        {
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
            await Assert.That(exception!.CancellationToken.IsCancellationRequested).IsTrue();
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SendAsync_DoesNotTreatCancellationAsSuccessfulEndOfStream(
        bool useBufferedCopy)
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var contentStream = new BlockingReadStream(
            ignoreAsyncCancellation: true,
            returnEofWhenDisposed: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/cancelled-eof"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = TimeSpan.FromMinutes(1),
        }, cancellationTokenSource.Token);

        Task readTask;
        if (useBufferedCopy)
        {
            readTask = response.Content.ReadAsStringAsync();
        }
        else
        {
            var stream = await response.Content.ReadAsStreamAsync();
            readTask = stream.ReadAsync(new byte[1]).AsTask();
        }

        await contentStream.ReadStarted.WaitAsync(TestHostSettings.DefaultTestTimeout);
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(
            async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
    }

    [Test]
    public async Task SendAsync_CancelsLegacyResponseLogger()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(ignoreAsyncCancellation: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            new LegacyBodyLogger(logRequest: false),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await http.SendAsync(new HttpOptions(
                    new HttpRequestMessage(HttpMethod.Get, "https://example.test/legacy-logger"))
            {
                HttpClient = httpClient,
                Logging = ResponseOnly,
                Timeout = timeout,
            })
                .WaitAsync(TestHostSettings.DefaultTestTimeout));
    }

    [Test]
    public async Task SendAsync_CancelsLegacyRequestLogger()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(ignoreAsyncCancellation: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StringContent("response")));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            new LegacyBodyLogger(logRequest: true),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://example.test/legacy-request-logger")
        {
            Content = new StreamContent(contentStream),
        };

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await http.SendAsync(new HttpOptions(request)
            {
                HttpClient = httpClient,
                Logging = RequestOnly,
                Timeout = timeout,
            })
                .WaitAsync(TestHostSettings.DefaultTestTimeout));
    }

    [Test]
    [Arguments(true, true, false)]
    [Arguments(true, false, false)]
    [Arguments(false, true, false)]
    [Arguments(false, false, false)]
    [Arguments(true, false, true)]
    [Arguments(false, false, true)]
    public async Task SendAsync_ConfiguredTimeoutInterruptsSynchronousBodyRead(
        bool useCopyTo,
        bool throwIOExceptionWhenDisposed,
        bool returnEofWhenDisposed)
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(
            throwIOExceptionWhenDisposed,
            returnEofWhenDisposed: returnEofWhenDisposed);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)));
        var result = await GetService<IHttpContext>(_ => { });
        using var response = await result.T.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/synchronous-read"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = timeout,
        });
        var stream = await response.Content.ReadAsStreamAsync();
        var readTask = Task.Run(() =>
        {
            if (useCopyTo)
            {
                stream.CopyTo(Stream.Null);
            }
            else
            {
                stream.ReadExactly(new byte[1]);
            }
        });

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await readTask.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    public async Task SendAsync_CustomClientLogsRawContentBeforeApplyingBodyTimeout()
    {
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(new MemoryStream([1, 2, 3]))))
        {
            Timeout = TimeSpan.FromSeconds(5),
        };
        Type? loggedContentType = null;
        var httpLogger = new Mock<IHttpLogger>();
        httpLogger
            .Setup(x => x.PrintResponse(
                It.IsAny<HttpResponseMessage>(),
                It.IsAny<ILogger>(),
                It.IsAny<HttpLoggingOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<HttpResponseMessage, ILogger, HttpLoggingOptions, CancellationToken>(
                (response, _, _, _) => loggedContentType = response.Content.GetType())
            .Returns(Task.CompletedTask);
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            httpLogger.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/binary"))
        {
            HttpClient = httpClient,
            Logging = ResponseOnly,
        });

        using (Assert.Multiple())
        {
            await Assert.That(loggedContentType).IsEqualTo(typeof(StreamContent));
            await Assert.That(response.Content).IsTypeOf<TimeoutHttpContent>();
        }
    }

    [Test]
    public async Task SendAsync_UsesPerRequestThenFallbackThenPipelineLogging()
    {
        var observedOptions = new List<HttpLoggingOptions>();
        var httpLogger = new Mock<IHttpLogger>();
        httpLogger
            .Setup(x => x.PrintRequest(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<ILogger>(),
                It.IsAny<HttpLoggingOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<HttpRequestMessage, ILogger, HttpLoggingOptions, CancellationToken>(
                (_, _, options, _) => observedOptions.Add(options))
            .Returns(Task.CompletedTask);
        httpLogger
            .Setup(x => x.PrintResponse(
                It.IsAny<HttpResponseMessage>(),
                It.IsAny<ILogger>(),
                It.IsAny<HttpLoggingOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor.SetupGet(x => x.Logger).Returns(Mock.Of<ILogger>());
        using var httpClient = new HttpClient(new SequenceResponseHandler(
            new HttpResponseMessage(HttpStatusCode.OK),
            new HttpResponseMessage(HttpStatusCode.OK),
            new HttpResponseMessage(HttpStatusCode.OK)));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            moduleLoggerAccessor.Object,
            httpLogger.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Http = new PipelineHttpOptions
                {
                    Logging = HttpLoggingOptions.Headers,
                },
            }));

        using var perRequestResponse = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/per-request"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.Minimal,
        });
        using var fallbackResponse = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/fallback"))
        {
            HttpClient = httpClient,
            FallbackLogging = HttpLoggingOptions.None,
        });
        using var pipelineDefaultResponse = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/pipeline-default"))
        {
            HttpClient = httpClient,
        });

        using (Assert.Multiple())
        {
            await Assert.That(observedOptions[0]).IsSameReferenceAs(HttpLoggingOptions.Minimal);
            await Assert.That(observedOptions[1]).IsSameReferenceAs(HttpLoggingOptions.None);
            await Assert.That(observedOptions[2]).IsSameReferenceAs(HttpLoggingOptions.Headers);
        }
    }

    [Test]
    [Arguments(4096, false)]
    [Arguments(0, false)]
    [Arguments(0, true)]
    public async Task SendAsync_CustomClientTimeoutInterruptsNonCooperativeResponseLogging(
        int maxBodySizeToLog,
        bool returnEofWhenDisposed)
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(
            ignoreAsyncCancellation: true,
            returnEofWhenDisposed: returnEofWhenDisposed);
        var content = new StreamContent(contentStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        using var httpClient = new HttpClient(new ImmediateResponseHandler(content));
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        var httpLogger = new HttpLogger(
            Mock.Of<IHttpRequestFormatter>(),
            new HttpResponseFormatter(
                Mock.Of<ISecretObfuscator>(),
                Mock.Of<ISecretProvider>(),
                Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions())));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            moduleLoggerAccessor.Object,
            httpLogger,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await http.SendAsync(new HttpOptions(
                    new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-custom-log-body"))
            {
                HttpClient = httpClient,
                Logging = ResponseOnly with
                {
                    MaxBodySizeToLog = maxBodySizeToLog,
                },
                Timeout = timeout,
            })
                .WaitAsync(TestHostSettings.DefaultTestTimeout));
    }

    [Test]
    public async Task SendAsync_ConfiguredTimeoutInterruptsBufferedBodyCopy()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream(ignoreAsyncCancellation: true);
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(new StreamContent(contentStream)));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/buffered-timeout"))
        {
            HttpClient = httpClient,
            Logging = HttpLoggingOptions.None,
            Timeout = timeout,
        });

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await response.Content.ReadAsStringAsync().WaitAsync(TestHostSettings.DefaultTestTimeout));
    }

    [Test]
    [Arguments(true, true)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(false, false)]
    public async Task SendAsync_PreservesCancellationWhileReadingErrorContent(
        bool useCustomClient,
        bool useConfiguredTimeout)
    {
        var contentStream = new BlockingReadStream();
        using var httpClient = new HttpClient(
            new ImmediateResponseHandler(
                new StreamContent(contentStream),
                HttpStatusCode.InternalServerError))
        {
            Timeout = Timeout.InfiniteTimeSpan,
        };
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var http = new ModularPipelines.Http.Http(
            httpClientFactory.Object,
            Mock.Of<IModuleLoggerAccessor>(),
            Mock.Of<IHttpLogger>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(100));

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await http.SendAsync(new HttpOptions(
                        new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-error-body"))
                {
                    HttpClient = useCustomClient ? httpClient : null,
                    Logging = HttpLoggingOptions.None,
                    ThrowOnNonSuccessStatusCode = true,
                    Timeout = useConfiguredTimeout ? TimeSpan.FromMilliseconds(100) : null,
                },
                    useConfiguredTimeout ? CancellationToken.None : cancellationTokenSource.Token)
                    .WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            contentStream.Release();
        }
    }

    [Test]
    public async Task SendAsync_CustomClientKeepsTimeoutOutsideLoggedReplayContent()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var content = new StreamContent(
            new MemoryStream("response body"u8.ToArray()));
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        using var httpClient = new HttpClient(new ImmediateResponseHandler(content));
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        var httpLogger = new HttpLogger(
            Mock.Of<IHttpRequestFormatter>(),
            new HttpResponseFormatter(
                Mock.Of<ISecretObfuscator>(),
                Mock.Of<ISecretProvider>(),
                Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions())));
        var http = new ModularPipelines.Http.Http(
            Mock.Of<IHttpClientFactory>(),
            moduleLoggerAccessor.Object,
            httpLogger,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        using var response = await http.SendAsync(new HttpOptions(
            new HttpRequestMessage(HttpMethod.Get, "https://example.test/logged-timeout"))
        {
            HttpClient = httpClient,
            Logging = ResponseOnly,
            Timeout = timeout,
        });

        var stream = await response.Content.ReadAsStreamAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await ReadUntilTimeout(stream).WaitAsync(TestHostSettings.DefaultTestTimeout));

        static async Task ReadUntilTimeout(Stream responseStream)
        {
            while (true)
            {
#pragma warning disable CA2022 // Zero-byte read probes timeout cancellation without consuming replay content.
                responseStream.Read(Span<byte>.Empty);
#pragma warning restore CA2022
                await Task.Yield();
            }
        }
    }

    [Test]
    public async Task SendAsync_ConfiguredTimeoutCancelsFactoryResponseLogging()
    {
        var timeout = TimeSpan.FromMilliseconds(100);
        var contentStream = new BlockingReadStream();
        var content = new StreamContent(contentStream);
        content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        var httpLogger = new HttpLogger(
            Mock.Of<IHttpRequestFormatter>(),
            new HttpResponseFormatter(
                Mock.Of<ISecretObfuscator>(),
                Mock.Of<ISecretProvider>(),
                Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions())));
        using var httpClient = new HttpClient(new ImmediateResponseHandler(content));
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var http = new ModularPipelines.Http.Http(
            httpClientFactory.Object,
            moduleLoggerAccessor.Object,
            httpLogger,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await http.SendAsync(new HttpOptions(
                        new HttpRequestMessage(HttpMethod.Get, "https://example.test/stalled-log-body"))
                {
                    Logging = ResponseOnly,
                    Timeout = timeout,
                })
                    .WaitAsync(TestHostSettings.DefaultTestTimeout));
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
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        using var handler = new ResilienceHttpHandler(
            moduleLoggerAccessor.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Http = new PipelineHttpOptions
                {
                    Resilience = new HttpResilienceOptions
                    {
                        MaxRetryAttempts = 1,
                        InitialDelay = TimeSpan.Zero,
                        JitterFactor = 0,
                    },
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
    public async Task ResilienceHandler_PreservesContentHeadersAcrossAttempts()
    {
        var innerHandler = new SequenceResponseHandler(
            new HttpResponseMessage(HttpStatusCode.ServiceUnavailable),
            new HttpResponseMessage(HttpStatusCode.OK));
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(Mock.Of<ILogger>());
        using var handler = new ResilienceHttpHandler(
            moduleLoggerAccessor.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Http = new PipelineHttpOptions
                {
                    Resilience = new HttpResilienceOptions
                    {
                        MaxRetryAttempts = 1,
                        InitialDelay = TimeSpan.Zero,
                        JitterFactor = 0,
                    },
                },
            }))
        {
            InnerHandler = innerHandler,
        };
        using var invoker = new HttpMessageInvoker(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/upload")
        {
            Content = new ByteArrayContent([1, 2, 3]),
        };
        request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = "artifact.bin",
        };
        request.Content.Headers.ContentEncoding.Add("gzip");
        request.Content.Headers.ContentLanguage.Add("en-GB");
        request.Content.Headers.ContentMD5 = [1, 2, 3, 4];
        request.Content.Headers.TryAddWithoutValidation("X-Content-Metadata", ["first", "second"]);
        var expectedHeaders = SnapshotContentHeaders(request);

        using var response = await invoker.SendAsync(request, CancellationToken.None);

        using (Assert.Multiple())
        {
            await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
            await Assert.That(innerHandler.ContentHeaderSnapshots).Count().IsEqualTo(2);
            await Assert.That(innerHandler.ContentHeaderSnapshots[0]).IsEquivalentTo(expectedHeaders);
            await Assert.That(innerHandler.ContentHeaderSnapshots[1]).IsEquivalentTo(expectedHeaders);
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
        var (http, _) = await GetService<IHttpContext>(_ => { });

        await http.SendAsync(server.Uri);
    }

    [Test]
    public async Task When_Log_Request_False_Then_Do_Not_Log_Request()
    {
        await using var server = LocalHttpServer.Start();
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var (http, host) = await GetService<IHttpContext>(collection =>
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
            Logging = ResponseOnly,
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

        var (http, host) = await GetService<IHttpContext>(collection =>
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
            Logging = RequestOnly,
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

        var (http, host) = await GetService<IHttpContext>(collection =>
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

    private static string[] SnapshotContentHeaders(HttpRequestMessage request) =>
        request.Content?.Headers
            .Where(static header => !header.Key.Equals(
                "Content-Length",
                StringComparison.OrdinalIgnoreCase))
            .OrderBy(static header => header.Key, StringComparer.OrdinalIgnoreCase)
            .Select(static header => $"{header.Key}:{string.Join("|", header.Value)}")
            .ToArray()
        ?? [];

    private sealed class ImmediateResponseHandler(
        HttpContent content,
        HttpStatusCode statusCode = HttpStatusCode.OK) : HttpMessageHandler
    {
        public TaskCompletionSource RequestReceived { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestReceived.TrySetResult();
            return Task.FromResult(new HttpResponseMessage(statusCode)
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

    private sealed class BlockingReadStream(
        bool throwIOExceptionWhenDisposed = false,
        bool ignoreAsyncCancellation = false,
        bool returnEofWhenDisposed = false) : Stream
    {
        private readonly TaskCompletionSource _release =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _readStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly ManualResetEventSlim _synchronousRelease = new();
        private bool _isDisposed;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public Task ReadStarted => _readStarted.Task;

        public void Release()
        {
            _release.TrySetResult();
            _synchronousRelease.Set();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return WaitForSynchronousRead();
        }

        public override int Read(Span<byte> buffer)
        {
            return WaitForSynchronousRead();
        }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            _readStarted.TrySetResult();
            if (ignoreAsyncCancellation)
            {
                await _release.Task;
                if (!returnEofWhenDisposed)
                {
                    ThrowIfDisposed();
                }
            }
            else
            {
                await _release.Task.WaitAsync(cancellationToken);
            }

            return 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isDisposed = true;
                Release();
            }

            base.Dispose(disposing);
        }

        private int WaitForSynchronousRead()
        {
            _synchronousRelease.Wait();
            ThrowIfDisposed();
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw throwIOExceptionWhenDisposed
                    ? new IOException("The stream was interrupted.")
                    : new ObjectDisposedException(nameof(BlockingReadStream));
            }
        }
    }

    private sealed class LegacyBodyLogger(bool logRequest) : IHttpLogger
    {
        public Task PrintRequest(HttpRequestMessage request, ILogger logger)
        {
            return logRequest
                ? request.Content!.ReadAsStringAsync()
                : Task.CompletedTask;
        }

        public Task PrintRequest(
            HttpRequestMessage request,
            ILogger logger,
            HttpLoggingOptions options)
        {
            return logRequest
                ? request.Content!.ReadAsStringAsync()
                : Task.CompletedTask;
        }

        public Task PrintResponse(HttpResponseMessage response, ILogger logger)
        {
            return logRequest
                ? Task.CompletedTask
                : response.Content.ReadAsStringAsync();
        }

        public Task PrintResponse(
            HttpResponseMessage response,
            ILogger logger,
            HttpLoggingOptions options)
        {
            return logRequest
                ? Task.CompletedTask
                : response.Content.ReadAsStringAsync();
        }

        public void PrintStatusCode(HttpStatusCode? httpStatusCode, ILogger logger)
        {
        }

        public void PrintDuration(TimeSpan duration, ILogger logger)
        {
        }
    }

    private sealed class SequenceResponseHandler(params HttpResponseMessage[] responses) : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        public List<string[]> ContentHeaderSnapshots { get; } = [];

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            ContentHeaderSnapshots.Add(SnapshotContentHeaders(request));
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
