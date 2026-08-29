using System.Net;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Security;
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
            await Assert.That(observedOptions.FallbackLogging!.LogResponse).IsFalse();
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
        fileSystemProvider
            .Setup(x => x.GetRandomFileName())
            .Returns("random.tmp");
        var downloader = CreateDownloader(content, fileSystemProvider.Object);

        await downloader.DownloadFileAsync(new DownloadFileOptions(
            new Uri("https://example.test/download"))
        {
            SavePath = "download.bin",
        });

        await Assert.That(content.IsDisposed).IsTrue();
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
        var content = new TrackingStringContent("failure");
        var downloader = CreateDownloader(content, statusCode: HttpStatusCode.BadRequest);

        await Assert.ThrowsAsync<PipelineHttpResponseException>(() =>
            downloader.DownloadResponseAsync(
                new DownloadOptions(new Uri("https://example.test/download"))));

        await Assert.That(content.IsDisposed).IsTrue();
    }

    private static Downloader CreateDownloader(
        HttpContent content,
        IFileSystemProvider? fileSystemProvider = null,
        HttpStatusCode statusCode = HttpStatusCode.OK,
        string? workingDirectory = null,
        Action<HttpOptions>? observeOptions = null)
    {
        var http = new Mock<IHttpContext>();
        http.Setup(x => x.SendAsync(
                It.IsAny<HttpOptions>(),
                It.IsAny<CancellationToken>()))
            .Callback<HttpOptions, CancellationToken>((options, _) => observeOptions?.Invoke(options))
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
            fileSystemProvider ?? SystemFileSystemProvider.Instance,
            new PipelineWorkingDirectory(workingDirectory ?? Environment.CurrentDirectory));
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
