using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Files;
using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

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
}
