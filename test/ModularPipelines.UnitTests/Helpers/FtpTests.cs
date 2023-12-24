using System.Net;
using FluentFTP;
using ModularPipelines.Ftp;
using ModularPipelines.Ftp.Options;
using ModularPipelines.Helpers;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

[Parallelizable(ParallelScope.Self)]
[Ignore("FTP tests flaky due to server load")]
public class FtpTests : TestBase
{
    [Test]
    [Order(1)]
    [Ignore("FTP tests flaky due to server load")]
    public async Task Can_Download()
    {
        var ftp = await GetService<IFtp>();

        var client = await ftp.GetFtpClientAsync(new FtpOptions("ftp.pureftpd.org", new NetworkCredential())
        {
            ClientConfigurator = client => { },
        });

        var localPath = File.GetNewTemporaryFilePath();

        var response = await client.DownloadFile(localPath, "/6jack/README.markdown");

        var fileContents = await localPath.ReadAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.EqualTo(FtpStatus.Success));
            Assert.That(fileContents, Does.StartWith("6jack"));
        });
    }

    [Test]
    [Ignore("FTP tests flaky due to server load")]
    public async Task Client_Is_Disposed_Properly()
    {
        var ftp = await GetService<IFtp>();

        var client = await ftp.GetFtpClientAsync(new FtpOptions("ftp.pureftpd.org", new NetworkCredential())
        {
            ClientConfigurator = client => { },
        });

        Assert.That(client.IsDisposed, Is.False);

        await Disposer.DisposeObjectAsync(ftp);

        Assert.That(client.IsDisposed, Is.True);
    }
}
