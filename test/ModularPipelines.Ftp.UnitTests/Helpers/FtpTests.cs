using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using FluentFTP;
using FluentFTP.Exceptions;
using ModularPipelines.Ftp;
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

    [Test]
    [Arguments(FtpEncryptionMode.None)]
    [Arguments(FtpEncryptionMode.Auto)]
    [Arguments(FtpEncryptionMode.Explicit)]
    public async Task Required_Encryption_Rejects_Plaintext_Without_Sending_Credentials(FtpEncryptionMode encryptionMode)
    {
        await using var ftpServer = LocalFtpServer.Start();
        var ftp = await GetService<IFtp>();
        IAsyncFtpClient? captured = null;
        var options = CreateOptions(ftpServer.Port);
        var configure = options.ClientConfigurator;
        options = options with
        {
            RequireEncryption = true,
            ClientConfigurator = client =>
            {
                captured = client;
                configure!(client);
                client.Config.EncryptionMode = encryptionMode;
            },
        };

        await Assert.ThrowsAsync<FtpException>(() => ftp.GetFtpClientAsync(options));

        using (Assert.Multiple())
        {
            await Assert.That(captured!.IsDisposed).IsTrue();
            await Assert.That(ftpServer.Commands.Any(command =>
                command.StartsWith("USER ", StringComparison.Ordinal)
                || command.StartsWith("PASS ", StringComparison.Ordinal))).IsFalse();
        }
    }

    [Test]
    public async Task Failed_Configuration_Disposes_Client()
    {
        var ftp = await GetService<IFtp>();
        IAsyncFtpClient? captured = null;
        var failure = new InvalidOperationException("configuration failed");
        var options = new FtpOptions("127.0.0.1", new NetworkCredential("user", "password"))
        {
            ClientConfigurator = client =>
            {
                captured = client;
                throw failure;
            },
        };

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => ftp.GetFtpClientAsync(options));
        await Assert.That(exception).IsSameReferenceAs(failure);
        await Assert.That(captured!.IsDisposed).IsTrue();
    }

    [Test]
    public async Task PreCanceled_Connection_Does_Not_Configure_Client()
    {
        var ftp = await GetService<IFtp>();
        using var cancellation = new CancellationTokenSource();
        await cancellation.CancelAsync();
        var configured = false;
        var options = CreateOptions(21) with { ClientConfigurator = _ => configured = true };

        await Assert.ThrowsAsync<OperationCanceledException>(() => ftp.GetFtpClientAsync(options, cancellation.Token));
        await Assert.That(configured).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Cancellation_During_Connection_Disposes_Client(bool requireEncryption)
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var ftp = await GetService<IFtp>();
        using var cancellation = new CancellationTokenSource();
        IAsyncFtpClient? captured = null;
        var options = CreateOptions(((IPEndPoint) listener.LocalEndpoint).Port);
        var configure = options.ClientConfigurator;
        options = options with
        {
            RequireEncryption = requireEncryption,
            ClientConfigurator = client =>
            {
                captured = client;
                configure!(client);
            },
        };

        var connection = ftp.GetFtpClientAsync(options, cancellation.Token);
        using var accepted = await listener.AcceptTcpClientAsync().WaitAsync(TimeSpan.FromSeconds(10));
        await cancellation.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(() => connection.WaitAsync(TimeSpan.FromSeconds(10)));
        await Assert.That(captured!.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Registration_Remains_Discoverable_And_Hidden_From_IntelliSense()
    {
        var method = typeof(FtpExtensions).GetMethod(nameof(FtpExtensions.RegisterFtpContext))!;
        await Assert.That(method.GetCustomAttribute<EditorBrowsableAttribute>()!.State)
            .IsEqualTo(EditorBrowsableState.Never);
        await Assert.That(await GetService<IFtp>()).IsNotNull();
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
