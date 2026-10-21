using System.Net;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Implementations;
using ModularPipelines.FileSystem;
using ModularPipelines.Http;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class DownloadTimeoutTests
{
    [Test]
    [Arguments("string", false)]
    [Arguments("response", false)]
    [Arguments("file", false)]
    [Arguments("installer", false)]
    [Arguments("string", true)]
    [Arguments("response", true)]
    [Arguments("file", true)]
    [Arguments("installer", true)]
    public async Task DefaultTimeout_DoesNotLimitDownloadBody(string downloadKind, bool pipelineTimeout)
    {
        var time = new FakeTimeProvider();
        var startedAt = time.GetUtcNow();
        using var client = CreateClient(time);
        var downloader = CreateDownloader(client, time, pipelineTimeout ? TimeSpan.FromSeconds(30) : null);
        var uri = new Uri("https://example.test/large-download");
        string content;

        if (downloadKind == "installer")
        {
            string? downloadedContent = null;
            var installer = new Mock<InstallersContext>(Mock.Of<ICommandContext>(), downloader,
                new PipelineWorkingDirectory(Environment.CurrentDirectory))
            { CallBase = true };
            installer.Setup(x => x.InstallAsync(It.IsAny<InstallerOptions>(), It.IsAny<CancellationToken>()))
                .Returns(async (InstallerOptions options, CancellationToken token) =>
                {
                    downloadedContent = await File.ReadAllTextAsync(options.Path, token);
                    return CommandResult.Ok();
                });
            await installer.Object.InstallFromWebAsync(new WebInstallerOptions(uri));
            content = downloadedContent!;
        }
        else if (downloadKind == "file")
        {
            var path = Path.Combine(Path.GetTempPath(), $"download-{Guid.NewGuid()}.txt");
            try
            {
                var file = await downloader.DownloadFileAsync(new DownloadFileOptions(uri) { SavePath = path });
                content = await File.ReadAllTextAsync(file.Path);
            }
            finally
            {
                File.Delete(path);
            }
        }
        else if (downloadKind == "response")
        {
            using var response = await downloader.DownloadResponseAsync(new DownloadOptions(uri));
            content = await response.Content.ReadAsStringAsync();
        }
        else
        {
            content = await downloader.DownloadStringAsync(new DownloadOptions(uri));
        }

        await Assert.That(content).IsEqualTo("abc");
        await Assert.That(time.GetUtcNow() - startedAt).IsGreaterThan(TimeSpan.FromSeconds(100));
        await Assert.That(client.Timeout).IsEqualTo(TimeSpan.FromSeconds(100));
    }

    [Test]
    public async Task DefaultTimeout_StillCancelsWhileWaitingForHeaders()
    {
        var time = new FakeTimeProvider();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var client = new HttpClient(new CallbackHandler(async (_, token) =>
        {
            started.TrySetResult();
            await Task.Delay(Timeout.InfiniteTimeSpan, token);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }));
        var downloader = CreateDownloader(client, time);
        var download = downloader.DownloadResponseAsync(new DownloadOptions(new Uri("https://example.test/slow-headers")));
        await started.Task.WaitAsync(TimeSpan.FromSeconds(10));

        time.Advance(client.Timeout);

        await Assert.ThrowsAsync<OperationCanceledException>(() => download.WaitAsync(TimeSpan.FromSeconds(10)));
    }

    [Test]
    public async Task DefaultTimeout_RetainsCallerCancellationAfterHeaders()
    {
        var time = new FakeTimeProvider();
        using var client = CreateClient(time);
        using var cancellation = new CancellationTokenSource();
        var downloader = CreateDownloader(client, time);
        using var response = await downloader.DownloadResponseAsync(
            new DownloadOptions(new Uri("https://example.test/download")), cancellation.Token);
        time.Advance(TimeSpan.FromMinutes(5));
        var stream = await response.Content.ReadAsStreamAsync();
        await Assert.That(await stream.ReadAsync(new byte[1])).IsEqualTo(1);

        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => stream.ReadExactlyAsync(new byte[1]).AsTask());
    }

    [Test]
    [Arguments(90, false)]
    [Arguments(240, true)]
    [Arguments(-1, true)]
    public async Task ExplicitTimeout_OverridesPipelineDefaultAndIncludesBody(int seconds, bool completes)
    {
        var time = new FakeTimeProvider();
        using var client = CreateClient(time);
        var downloader = CreateDownloader(client, time, TimeSpan.FromSeconds(30));
        var options = new DownloadOptions(new Uri("https://example.test/download"))
        {
            Timeout = seconds < 0 ? Timeout.InfiniteTimeSpan : TimeSpan.FromSeconds(seconds),
        };

        if (completes)
        {
            await Assert.That(await downloader.DownloadStringAsync(options)).IsEqualTo("abc");
        }
        else
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() => downloader.DownloadStringAsync(options));
        }
    }

    private static HttpClient CreateClient(FakeTimeProvider time) => new(new CallbackHandler((request, _) =>
        Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StreamContent(new SlowReadStream(time)),
            RequestMessage = request,
        })));

    private static Downloader CreateDownloader(HttpClient client, TimeProvider time, TimeSpan? pipelineTimeout = null)
    {
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
        var logger = Mock.Of<IModuleLoggerAccessor>(x => x.Logger == Mock.Of<IModuleLogger>());
        var options = Microsoft.Extensions.Options.Options.Create(new PipelineOptions
        {
            Http = new PipelineHttpOptions { Timeout = pipelineTimeout },
        });
        var http = new ModularPipelines.Http.Http(factory.Object, logger, Mock.Of<IHttpLogger>(), options, time);
        return new Downloader(logger, http, SystemFileSystemProvider.Instance,
            new PipelineWorkingDirectory(Environment.CurrentDirectory), options);
    }

    private sealed class CallbackHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> send)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => send(request, cancellationToken);
    }

    private sealed class SlowReadStream(FakeTimeProvider time) : Stream
    {
        private int _position;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_position == 3 || buffer.IsEmpty)
            {
                return ValueTask.FromResult(0);
            }

            time.Advance(TimeSpan.FromSeconds(60));
            buffer.Span[0] = (byte) ('a' + _position++);
            return ValueTask.FromResult(1);
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush() => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
