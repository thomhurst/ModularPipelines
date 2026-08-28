using System.Net;
using FluentFTP;
using ModularPipelines.Ftp;
using ModularPipelines.Ftp.Options;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Ftp.UnitTests.Helpers;

public class FtpTests : TestBase
{
    [Test]
    public async Task Can_Download()
    {
        await using var ftpServer = LocalFtpServer.Start();
        var ftp = await GetService<IFtp>();

        var client = await ftp.GetFtpClientAsync(CreateOptions(ftpServer.Port));

        await using var tempFile = new TempFile();

        var response = await client.DownloadFile(
            tempFile.File,
            LocalFtpServer.RemotePath,
            FtpLocalExists.Overwrite);
        var fileContents = await tempFile.File.ReadAsync();

        using (Assert.Multiple())
        {
            await Assert.That(response).IsEqualTo(FtpStatus.Success);
            await Assert.That(fileContents).IsEqualTo(LocalFtpServer.Contents);
            await Assert.That(ftpServer.Commands).Contains(command =>
                command.Equals($"RETR {LocalFtpServer.RemotePath}", StringComparison.Ordinal));
        }
    }

    [Test]
    public async Task Client_Is_Disposed_Properly()
    {
        await using var ftpServer = LocalFtpServer.Start();
        var ftp = await GetService<IFtp>();

        var client = await ftp.GetFtpClientAsync(CreateOptions(ftpServer.Port));
        await Assert.That(client.IsDisposed).IsFalse();

        await ((IAsyncDisposable) ftp).DisposeAsync();
        await Assert.That(client.IsDisposed).IsTrue();
    }

    private static FtpOptions CreateOptions(int port)
    {
        return new FtpOptions("127.0.0.1", new NetworkCredential("user", "password"))
        {
            ClientConfigurator = client =>
            {
                client.Port = port;
                client.Config.EncryptionMode = FtpEncryptionMode.None;
                client.Config.DataConnectionType = FtpDataConnectionType.PASV;
                client.Config.InternetProtocolVersions = FtpIpVersion.IPv4;
                client.Config.ConnectTimeout = 5_000;
                client.Config.ReadTimeout = 5_000;
                client.Config.DataConnectionConnectTimeout = 5_000;
            },
        };
    }
}
