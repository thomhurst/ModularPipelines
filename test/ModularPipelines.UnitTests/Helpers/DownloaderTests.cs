using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Constants;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class DownloaderTests : TestBase
{
    [Test]
    public async Task DownloadStringAsync_EmptyContentReturnsEmptyString()
    {
        IDownloaderContext downloader = CreateDownloader(new StringContent(string.Empty));
        var result = await downloader.DownloadStringAsync(new DownloadOptions(new Uri("https://example.test/download")));
        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task DownloadStringAsync_HttpFailureThrows()
    {
        IDownloaderContext downloader = CreateDownloader(new StringContent("failure"), statusCode: HttpStatusCode.BadRequest);
        await Assert.ThrowsAsync<PipelineHttpResponseException>(() =>
            downloader.DownloadStringAsync(new DownloadOptions(new Uri("https://example.test/download"))));
    }

    [Test]
    public async Task DownloadOptions_UseMinimalFallbackLoggingByDefault()
    {
        HttpOptions? observedOptions = null;
        var downloader = CreateDownloader(
            new StringContent("download"),
            observeOptions: options => observedOptions = options);

        using var response = await downloader.DownloadResponseAsync(
            new DownloadOptions(new Uri("https://example.test/file")));

        using (Assert.Multiple())
        {
            await Assert.That(observedOptions).IsNotNull();
            await Assert.That(observedOptions!.Logging).IsNull();
            await Assert.That(observedOptions.FallbackLogging).IsSameReferenceAs(HttpLoggingOptions.Minimal);
            await Assert.That(observedOptions.FallbackLogging!.ShowResponse).IsFalse();
        }
    }

    [Test]
    public async Task Can_Download()
    {
        var downloader = await GetService<IDownloaderContext>();
        var hash = await GetService<IHashContext>();
        await using var server = LocalHttpServer.Start("local download fixture"u8.ToArray());

        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(server.Uri));

        await Assert.That(hash.Md5File(file)).IsEqualTo("aedf5d7c23744269f358814e602afe89");
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesRequestAndPreservesResponseAndClient()
    {
        using var requestContent = new TrackingStringContent("request");
        using var client = new HttpClient(new DownloadResponseHandler());
        var downloader = await GetService<IDownloaderContext>();
        var options = new DownloadOptions(new Uri("https://example.test/download"))
        {
            HttpClient = client,
            RequestConfigurator = request => request.Content = requestContent,
        };

        Stream responseStream;
        using (var response = await downloader.DownloadResponseAsync(options))
        {
            await Assert.That(requestContent.IsDisposed).IsTrue();
            responseStream = await response.Content.ReadAsStreamAsync();
            await Assert.That(responseStream.CanRead).IsTrue();
            using var reader = new StreamReader(responseStream, leaveOpen: true);
            await Assert.That(await reader.ReadToEndAsync()).IsEqualTo("download");
        }

        await Assert.That(responseStream.CanRead).IsFalse();
        using var subsequentResponse = await client.GetAsync(options.DownloadUri);
        await Assert.That(await subsequentResponse.Content.ReadAsStringAsync()).IsEqualTo("download");
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesRequestWhenConfiguratorThrows()
    {
        using var requestContent = new TrackingStringContent("request");
        using var responseContent = new StringContent("download");
        var downloader = CreateDownloader(responseContent);
        var failure = new InvalidOperationException("Configuration failed");

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            downloader.DownloadResponseAsync(new DownloadOptions(new Uri("https://example.test/download"))
            {
                RequestConfigurator = request =>
                {
                    request.Content = requestContent;
                    throw failure;
                },
            }));

        await Assert.That(exception).IsSameReferenceAs(failure);
        await Assert.That(requestContent.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesRequestWhenSendFails()
    {
        using var requestContent = new TrackingStringContent("request");
        using var responseContent = new StringContent("download");
        var failure = new HttpRequestException("Send failed");
        var downloader = CreateDownloader(responseContent,
            sendAsync: (_, _) => Task.FromException<HttpResponseMessage>(failure));

        var exception = await Assert.ThrowsAsync<HttpRequestException>(() =>
            downloader.DownloadResponseAsync(new DownloadOptions(new Uri("https://example.test/download"))
            {
                RequestConfigurator = request => request.Content = requestContent,
            }));

        await Assert.That(exception).IsSameReferenceAs(failure);
        await Assert.That(requestContent.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesRequestWhenSendIsCanceled()
    {
        using var requestContent = new TrackingStringContent("request");
        using var responseContent = new StringContent("download");
        using var cancellation = new CancellationTokenSource();
        var downloader = CreateDownloader(responseContent, sendAsync: (_, token) =>
        {
            cancellation.Cancel();
            return Task.FromCanceled<HttpResponseMessage>(token);
        });

        var exception = await Assert.ThrowsAsync<TaskCanceledException>(() =>
            downloader.DownloadResponseAsync(new DownloadOptions(new Uri("https://example.test/download"))
            {
                RequestConfigurator = request => request.Content = requestContent,
            }, cancellation.Token));

        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
        await Assert.That(requestContent.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadStringAsync_DisposesResponse()
    {
        using var requestContent = new TrackingStringContent("request");
        var content = new TrackingStringContent("download");
        var downloader = CreateDownloader(content);

        var result = await downloader.DownloadStringAsync(
            new DownloadOptions(new Uri("https://example.test/download"))
            {
                RequestConfigurator = request => request.Content = requestContent,
            });

        using (Assert.Multiple())
        {
            await Assert.That(result).IsEqualTo("download");
            await Assert.That(content.IsDisposed).IsTrue();
            await Assert.That(requestContent.IsDisposed).IsTrue();
        }
    }

    [Test]
    public async Task DownloadFileAsync_DisposesResponse()
    {
        using var requestContent = new TrackingStringContent("request");
        var content = new TrackingStringContent("download");
        var fileSystemProvider = new Mock<IFileSystemProvider>();
        fileSystemProvider
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns(() => new MemoryStream());
        fileSystemProvider
            .Setup(x => x.GetRandomFileName())
            .Returns("random.tmp");
        var downloader = CreateDownloader(content, fileSystemProvider.Object);

        await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/download"))
        {
            SavePath = "download.bin",
            RequestConfigurator = request => request.Content = requestContent,
        });

        await Assert.That(content.IsDisposed).IsTrue();
        await Assert.That(requestContent.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadFileAsync_ResolvesRelativeSavePathAgainstPipelineDirectory()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), "pipeline-working-directory");
        var savePath = Path.Combine("downloads", "download.bin");
        var expectedSavePath = Path.Combine(workingDirectory, savePath);
        var expectedTemporaryPath = Path.Combine(workingDirectory, "downloads", "random.tmp");
        var fileSystemProvider = new Mock<IFileSystemProvider>();
        fileSystemProvider.Setup(x => x.GetRandomFileName()).Returns("random.tmp");
        fileSystemProvider
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns(() => new MemoryStream());
        var downloader = CreateDownloader(
            new StringContent("download"),
            fileSystemProvider.Object,
            workingDirectory: workingDirectory);

        await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/download"))
        {
            SavePath = savePath,
        });

        fileSystemProvider.Verify(x => x.Create(expectedTemporaryPath), Times.Once());
        fileSystemProvider.Verify(x => x.MoveFile(expectedTemporaryPath, expectedSavePath, true), Times.Once());
    }

    [Test]
    public async Task DownloadFileAsync_Derives_Extension_From_Uri_Path()
    {
        var downloader = CreateDownloader(new StringContent("download"));
        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/archive.zip?token=abc.def#fragment.txt")));

        try
        {
            await Assert.That(Path.GetExtension(file.Path)).IsEqualTo(".zip");
        }
        finally
        {
            file.Delete();
        }
    }

    [Test]
    public async Task DownloadFileAsync_Removes_Invalid_Characters_From_Derived_Extension()
    {
        var downloader = CreateDownloader(new StringContent("download"));
        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/archive.zip%00suffix")));

        try
        {
            await Assert.That(Path.GetExtension(file.Path)).IsEqualTo(".zipsuffix");
        }
        finally
        {
            file.Delete();
        }
    }

    [Test]
    public async Task DownloadFileAsync_Removes_Control_Characters_From_Derived_Extension()
    {
        var downloader = CreateDownloader(new StringContent("download"));
        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/archive.zip%0Aforged")));

        try
        {
            await Assert.That(Path.GetExtension(file.Path)).IsEqualTo(".zipforged");
            await Assert.That(file.Path.Any(char.IsControl)).IsFalse();
        }
        finally
        {
            file.Delete();
        }
    }

    [Test]
    public async Task DownloadFileAsync_Preserves_Extension_Before_Decoding_Encoded_Separators()
    {
        var downloader = CreateDownloader(new StringContent("download"));
        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/archive.zip%2Fsuffix")));

        try
        {
            await Assert.That(Path.GetExtension(file.Path)).IsEqualTo(".zipsuffix");
        }
        finally
        {
            file.Delete();
        }
    }

    [Test]
    [Arguments("archive.zip%20")]
    public async Task DownloadFileAsync_Removes_Windows_Forbidden_Extension_Endings(string uriPath)
    {
        var downloader = CreateDownloader(new StringContent("download"));
        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri($"https://example.test/{uriPath}")));

        try
        {
            await Assert.That(Path.GetExtension(file.Path)).IsEqualTo(".zip");
        }
        finally
        {
            file.Delete();
        }
    }

    [Test]
    public async Task DownloadFileAsync_UsesBoundedSiblingTemporaryName()
    {
        var workingDirectory = Path.GetTempPath();
        var destination = Path.Combine(
            "downloads",
            new string('x', 240) + ".bin");
        var expectedDestination = Path.Combine(workingDirectory, destination);
        var expectedTemporaryPath = Path.Combine(workingDirectory, "downloads", "random.tmp");
        var fileSystemProvider = new Mock<IFileSystemProvider>();
        fileSystemProvider
            .Setup(x => x.GetRandomFileName())
            .Returns("random.tmp");
        fileSystemProvider
            .Setup(x => x.Create(expectedTemporaryPath))
            .Returns(() => new MemoryStream());
        var downloader = CreateDownloader(
            new StringContent("download"),
            fileSystemProvider.Object,
            workingDirectory: workingDirectory);

        await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/download"))
        {
            SavePath = destination,
        });

        fileSystemProvider.Verify(x => x.Create(expectedTemporaryPath), Times.Once());
        fileSystemProvider.Verify(
            x => x.MoveFile(expectedTemporaryPath, expectedDestination, true),
            Times.Once());
    }

    [Test]
    public async Task LegacyProviderFallbackDoesNotDeleteExistingDestination()
    {
        const string destination = "download.bin";
        var fileSystemProvider = new Mock<IFileSystemProvider>
        {
            CallBase = true,
        };
        fileSystemProvider
            .Setup(x => x.FileExists(destination))
            .Returns(true);

        var exception = Assert.Throws<NotSupportedException>(() =>
            fileSystemProvider.Object.MoveFile("temporary.download", destination, overwrite: true));

        await Assert.That(exception.Message).Contains("does not support atomic overwrite moves");
        fileSystemProvider.Verify(x => x.DeleteFile(destination), Times.Never());
        fileSystemProvider.Verify(
            x => x.MoveFile(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never());
    }

    [Test]
    public async Task DownloadFileAsync_PreservesExistingFileWhenStreamingFails()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"modular-pipelines-download-{Guid.NewGuid():N}");
        var destination = Path.Combine(directory, "download.bin");
        Directory.CreateDirectory(directory);
        await System.IO.File.WriteAllTextAsync(destination, "original");

        try
        {
            var content = new StreamContent(new FailingReadStream("partial download"u8.ToArray()));
            var downloader = CreateDownloader(content);

            await Assert.ThrowsAsync<IOException>(() => downloader.DownloadFileAsync(new DownloadFileOptions(
                new Uri("https://example.test/download"))
            {
                SavePath = destination,
            }));

            using (Assert.Multiple())
            {
                await Assert.That(await System.IO.File.ReadAllTextAsync(destination)).IsEqualTo("original");
                await Assert.That(Directory.GetFiles(directory).Length).IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task DownloadFileAsync_ReplacesExistingFileAfterStreamingSucceeds()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"modular-pipelines-download-{Guid.NewGuid():N}");
        var destination = Path.Combine(directory, "download.bin");
        Directory.CreateDirectory(directory);
        await System.IO.File.WriteAllTextAsync(destination, "original");

        try
        {
            var downloader = CreateDownloader(new StringContent("replacement"));

            await downloader.DownloadFileAsync(new DownloadFileOptions(
                new Uri("https://example.test/download"))
            {
                SavePath = destination,
            });

            using (Assert.Multiple())
            {
                await Assert.That(await System.IO.File.ReadAllTextAsync(destination)).IsEqualTo("replacement");
                await Assert.That(Directory.GetFiles(directory).Length).IsEqualTo(1);
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesResponseWhenStatusValidationFails()
    {
        using var requestContent = new TrackingStringContent("request");
        var content = new TrackingStringContent("failure");
        var downloader = CreateDownloader(content, statusCode: HttpStatusCode.BadRequest);

        await Assert.ThrowsAsync<PipelineHttpResponseException>(() =>
            downloader.DownloadResponseAsync(
                new DownloadOptions(new Uri("https://example.test/download"))
                {
                    RequestConfigurator = request => request.Content = requestContent,
                }));

        await Assert.That(content.IsDisposed).IsTrue();
        await Assert.That(requestContent.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadFileAsync_Redacts_Uri_In_Structured_Log()
    {
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(It.IsAny<Microsoft.Extensions.Logging.LogLevel>())).Returns(true);
        var fileSystem = new Mock<IFileSystemProvider>();
        fileSystem.Setup(x => x.GetRandomFileName()).Returns("random.tmp");
        fileSystem.Setup(x => x.Create(It.IsAny<string>())).Returns(() => new MemoryStream());
        var downloader = CreateDownloader(new StringContent("download"), fileSystem.Object, logger: logger.Object);
        var uri = new Uri("https://user:password@example.test/file?sig=signed-secret&custom_credential=custom-secret&keep=yes");

        await downloader.DownloadFileAsync(new DownloadFileOptions(uri)
        {
            SavePath = "download.bin",
            Logging = new HttpLoggingOptions
            {
                SensitiveQueryParameterNames = [.. HttpLoggingOptions.Default.SensitiveQueryParameterNames, "custom_credential"],
            },
        });

        var log = logger.Invocations.Single(x => x.Method.Name == "Log");
        var state = (IEnumerable<KeyValuePair<string, object?>>) log.Arguments[2]!;
        var displayedUri = state.Single(x => x.Key == "Uri").Value!.ToString()!;
        await Assert.That(displayedUri).IsEqualTo($"https://{LoggingConstants.SecretMask}@example.test/file?sig={LoggingConstants.SecretMask}&custom_credential={LoggingConstants.SecretMask}&keep=yes");
        await Assert.That(uri.UserInfo).IsEqualTo("user:password");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DownloadResponseAsync_Redacts_Custom_Query_Names_In_Failure(bool perDownload)
    {
        var logging = HttpLoggingOptions.Silent with
        {
            SensitiveQueryParameterNames = [.. HttpLoggingOptions.Default.SensitiveQueryParameterNames, "custom_credential"],
        };
        var downloader = CreateDownloader(new StringContent("failure"), statusCode: HttpStatusCode.BadRequest,
            pipelineLogging: perDownload ? null : logging);
        var uri = new Uri("https://example.test/file?custom_credential=custom-secret&sig=signed-secret");

        var exception = await Assert.ThrowsAsync<PipelineHttpResponseException>(() => downloader.DownloadResponseAsync(new DownloadOptions(uri)
        {
            Logging = perDownload ? logging : null,
        }));

        await Assert.That(exception!.Message).DoesNotContain("custom-secret");
        await Assert.That(exception.Message).DoesNotContain("signed-secret");
        await Assert.That(exception.RequestUri).IsSameReferenceAs(uri);
    }

    [Test]
    [Arguments("https://example.test/file?sig=signed-secret")]
    [Arguments("https://example.test/file?key=signed-secret")]
    [Arguments("https://example.test/file?api_key=signed-secret")]
    [Arguments("https://example.test/file?apikey=signed-secret")]
    [Arguments("https://example.test/file?client_secret=signed-secret")]
    [Arguments("https://example.test/file?password=signed-secret")]
    [Arguments("https://example.test/file?refresh_token=signed-secret")]
    [Arguments("https://example.test/file?id_token=signed-secret")]
    [Arguments("https://example.test/file?private_token=signed-secret")]
    [Arguments("https://example.test/file?signature=signed-secret")]
    [Arguments("https://example.test/file?auth=signed-secret")]
    [Arguments("https://example.test/file?KEY=signed-secret")]
    [Arguments("https://example.test/file?%61pi_key=signed-secret")]
    [Arguments("https://hooks.slack.com/services/team/channel/webhook-secret")]
    public async Task DownloadFileAsync_Redacts_Uri_With_Default_Options(string url)
    {
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(It.IsAny<Microsoft.Extensions.Logging.LogLevel>())).Returns(true);
        var fileSystem = new Mock<IFileSystemProvider>();
        fileSystem.Setup(x => x.GetRandomFileName()).Returns("random.tmp");
        fileSystem.Setup(x => x.Create(It.IsAny<string>())).Returns(() => new MemoryStream());
        var downloader = CreateDownloader(new StringContent("download"), fileSystem.Object, logger: logger.Object);

        await downloader.DownloadFileAsync(new DownloadFileOptions(new Uri(url)) { SavePath = "download.bin" });

        var display = logger.Invocations.Single(x => x.Method.Name == "Log").Arguments[2]!.ToString()!;
        await Assert.That(display).DoesNotContain("signed-secret");
        await Assert.That(display).DoesNotContain("webhook-secret");
        await Assert.That(display).Contains(LoggingConstants.SecretMask);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DownloadFileAsync_UsesEffectiveLoggingForRequestAndCompletion(bool perDownload)
    {
        var logging = HttpLoggingOptions.Minimal with
        {
            SensitiveQueryParameterNames = [.. HttpLoggingOptions.Default.SensitiveQueryParameterNames, "custom_credential"],
        };
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var downloader = await CreateLoggingDownloader(perDownload ? HttpLoggingOptions.Silent : logging, logger);
        var uri = new Uri("https://example.test/file?custom_credential=custom-secret&token=default-secret&keep=yes");
        using var client = new HttpClient(new DownloadResponseHandler());

        await downloader.DownloadFileAsync(new DownloadFileOptions(uri)
        {
            HttpClient = client,
            SavePath = "download.bin",
            Logging = perDownload ? logging : null,
        });

        var logs = logger.Invocations.Where(x => x.Method.Name == "Log").ToArray();
        var requestLog = logs.Single(x => x.Arguments[2]!.ToString()!.Contains("HTTP Request:"));
        var completionLog = logs.Single(x => x.Arguments[2]!.ToString()!.StartsWith("Downloaded "));
        foreach (var log in new[] { requestLog, completionLog })
        {
            var state = (IEnumerable<KeyValuePair<string, object?>>) log.Arguments[2]!;
            var displayedUri = state.Single(x => x.Key == (log == requestLog ? "Request" : "Uri")).Value!.ToString()!;
            await Assert.That(displayedUri).Contains($"custom_credential={LoggingConstants.SecretMask}");
            await Assert.That(displayedUri).Contains($"token={LoggingConstants.SecretMask}");
            await Assert.That(displayedUri).Contains("keep=yes");
            await Assert.That(displayedUri).DoesNotContain("custom-secret");
            await Assert.That(displayedUri).DoesNotContain("default-secret");
        }

        await Assert.That(uri.Query).Contains("custom-secret");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task DownloadFileAsync_SilentLoggingSuppressesRequestAndCompletion(bool perDownload)
    {
        var logger = new Mock<IModuleLogger>();
        logger.Setup(x => x.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var downloader = await CreateLoggingDownloader(perDownload ? HttpLoggingOptions.Minimal : HttpLoggingOptions.Silent, logger);
        using var client = new HttpClient(new DownloadResponseHandler());

        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(new Uri("https://example.test/file"))
        {
            HttpClient = client,
            SavePath = "download.bin",
            Logging = perDownload ? HttpLoggingOptions.Silent : null,
        });

        await Assert.That(file.Name).IsEqualTo("download.bin");
        await Assert.That(logger.Invocations.Any(x => x.Method.Name == "Log")).IsFalse();
    }

    private async Task<IDownloaderContext> CreateLoggingDownloader(HttpLoggingOptions pipelineLogging, Mock<IModuleLogger> logger)
    {
        var accessor = new Mock<IModuleLoggerAccessor>();
        accessor.SetupGet(x => x.Logger).Returns(logger.Object);
        var fileSystem = new Mock<IFileSystemProvider>();
        fileSystem.Setup(x => x.GetRandomFileName()).Returns("random.tmp");
        fileSystem.Setup(x => x.Create(It.IsAny<string>())).Returns(() => new MemoryStream());
        var (downloader, _) = await GetServiceWithPipelineConfiguration<IDownloaderContext>(builder =>
        {
            builder.Services.AddSingleton(accessor.Object);
            builder.Services.AddSingleton(fileSystem.Object);
            builder.ConfigureOptions(options => options with
            {
                Http = options.Http with { Logging = pipelineLogging },
            });
        });
        logger.Invocations.Clear();
        return downloader;
    }

    private static Downloader CreateDownloader(
        HttpContent content,
        IFileSystemProvider? fileSystemProvider = null,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? workingDirectory = null,
        Action<HttpOptions>? observeOptions = null,
        IModuleLogger? logger = null,
        Func<HttpOptions, CancellationToken, Task<HttpResponseMessage>>? sendAsync = null,
        HttpLoggingOptions? pipelineLogging = null)
    {
        var http = new Mock<IHttpContext>();
        http.Setup(x => x.SendAsync(
                It.IsAny<HttpOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<HttpOptions, CancellationToken>((options, _) => observeOptions?.Invoke(options))
            .Returns((HttpOptions options, CancellationToken token) => sendAsync?.Invoke(options, token)
                ?? Task.FromResult(new HttpResponseMessage(statusCode)
                {
                    Content = content,
                    RequestMessage = options.HttpRequestMessage,
                }));
        var moduleLoggerAccessor = new Mock<IModuleLoggerAccessor>();
        moduleLoggerAccessor
            .Setup(x => x.Logger)
            .Returns(logger ?? Mock.Of<IModuleLogger>());

        return new Downloader(
            moduleLoggerAccessor.Object,
            http.Object,
            fileSystemProvider ?? SystemFileSystemProvider.Instance,
            new PipelineWorkingDirectory(workingDirectory ?? Environment.CurrentDirectory),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Http = new PipelineHttpOptions { Logging = pipelineLogging },
            }));
    }

    private sealed class DownloadResponseHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new MemoryStream("download"u8.ToArray())),
                RequestMessage = request,
            });
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

    private sealed class FailingReadStream(byte[] content) : MemoryStream(content)
    {
        private bool _hasRead;

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_hasRead)
            {
                throw new IOException("Simulated streaming failure");
            }

            _hasRead = true;
            return base.ReadAsync(buffer, cancellationToken);
        }
    }
}
