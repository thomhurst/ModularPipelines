using System.Net;
using FluentFTP;
using ModularPipelines.Ftp;
using ModularPipelines.Ftp.Options;
using ModularPipelines.TestHelpers;
using Disposer = ModularPipelines.Helpers.Disposer;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.Helpers;

[Skip("FTP tests flaky due to server load")]
public class FtpTests : TestBase
{
    [Test]
    [NotInParallel(nameof(FtpTests), Order = 1)]
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
        
        await using (Assert.Multiple())
        {
            await Assert.That(response).IsEqualTo(FtpStatus.Success);
            await Assert.That(fileContents).StartsWith("6jack");
        }
    }

    [Test]
    public async Task Client_Is_Disposed_Properly()
    {
        var ftp = await GetService<IFtp>();

        var client = await ftp.GetFtpClientAsync(new FtpOptions("ftp.pureftpd.org", new NetworkCredential())
        {
            ClientConfigurator = client => { },
        });
        await Assert.That(client.IsDisposed).IsFalse();

        await Disposer.DisposeObjectAsync(ftp);
        await Assert.That(client.IsDisposed).IsTrue();
    }
}
