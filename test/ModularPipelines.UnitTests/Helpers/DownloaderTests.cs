using System.Net;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.Context.Domains.Network;
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
    public async Task DownloadOptions_DisableResponseBodyLoggingByDefault()
    {
        var options = new DownloadOptions(new Uri("https://example.test/file"));

        await Assert.That(options.LoggingType.HasFlag(ModularPipelines.Http.HttpLoggingType.Response)).IsFalse();
    }

    [Test]
    public async Task Can_Download()
    {
        var downloader = await GetService<IDownloaderContext>();
        var checksum = await GetService<IChecksumContext>();
        await using var server = LocalHttpServer.Start("local download fixture"u8.ToArray());

        var file = await downloader.DownloadFileAsync(new DownloadFileOptions(server.Uri));

        await Assert.That(checksum.Md5(file)).IsEqualTo("AEDF5D7C23744269F358814E602AFE89");
    }

    [Test]
    public async Task DownloadStringAsync_DisposesResponse()
    {
        var content = new TrackingStringContent("download");
        var downloader = CreateDownloader(content);

        var result = await downloader.DownloadStringAsync(
            new DownloadOptions(new Uri("https://example.test/download")));

        using (Assert.Multiple())
        {
            await Assert.That(result).IsEqualTo("download");
            await Assert.That(content.IsDisposed).IsTrue();
        }
    }

    [Test]
    public async Task DownloadFileAsync_DisposesResponse()
    {
        var content = new TrackingStringContent("download");
        var fileSystemProvider = new Mock<IFileSystemProvider>();
        fileSystemProvider
            .Setup(x => x.Create(It.IsAny<string>()))
            .Returns(() => new MemoryStream());
        var downloader = CreateDownloader(content, fileSystemProvider.Object);

        await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/download"))
        {
            SavePath = "download.bin",
        });

        await Assert.That(content.IsDisposed).IsTrue();
    }

    [Test]
    public async Task DownloadResponseAsync_DisposesResponseWhenStatusValidationFails()
    {
        var content = new TrackingStringContent("failure");
        var downloader = CreateDownloader(content, statusCode: HttpStatusCode.BadRequest);

        await Assert.ThrowsAsync<HttpResponseException>(() =>
            downloader.DownloadResponseAsync(
                new DownloadOptions(new Uri("https://example.test/download"))));

        await Assert.That(content.IsDisposed).IsTrue();
    }

    private static Downloader CreateDownloader(
        HttpContent content,
        IFileSystemProvider? fileSystemProvider = null,
        HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var http = new Mock<IHttpContext>();
        http.Setup(x => x.SendAsync(
                It.IsAny<HttpOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new HttpResponseMessage(statusCode)
            {
                Content = content,
            });
        var moduleLoggerProvider = new Mock<IModuleLoggerProvider>();
        moduleLoggerProvider
            .Setup(x => x.GetLogger())
            .Returns(Mock.Of<IModuleLogger>());

        return new Downloader(
            moduleLoggerProvider.Object,
            http.Object,
            fileSystemProvider ?? SystemFileSystemProvider.Instance);
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
