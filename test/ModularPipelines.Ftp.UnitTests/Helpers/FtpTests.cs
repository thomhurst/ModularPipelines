using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using FluentFTP;
using FluentFTP.Exceptions;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Ftp;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;
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
                client.Config.EncryptAuthenticationOnly = true;
                client.Config.DataConnectionEncryption = false;
            },
        };

        await Assert.ThrowsAsync<FtpException>(() => ftp.GetFtpClientAsync(options));

        using (Assert.Multiple())
        {
            await Assert.That(captured!.IsDisposed).IsTrue();
            await Assert.That(captured.Config.EncryptionMode).IsEqualTo(FtpEncryptionMode.Explicit);
            await Assert.That(captured.Config.EncryptAuthenticationOnly).IsFalse();
            await Assert.That(captured.Config.DataConnectionEncryption).IsTrue();
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
    [Arguments(false, FtpEncryptionMode.None)]
    [Arguments(true, FtpEncryptionMode.None)]
    [Arguments(true, FtpEncryptionMode.Implicit)]
    public async Task Cancellation_During_Connection_Disposes_Client(bool requireEncryption, FtpEncryptionMode encryptionMode)
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
                client.Config.EncryptionMode = encryptionMode;
                client.Config.EncryptAuthenticationOnly = true;
                client.Config.DataConnectionEncryption = false;
            },
        };

        var connection = ftp.GetFtpClientAsync(options, cancellation.Token);
        using var accepted = await listener.AcceptTcpClientAsync().WaitAsync(TimeSpan.FromSeconds(10));
        if (requireEncryption)
        {
            using (Assert.Multiple())
            {
                await Assert.That(captured!.Config.EncryptionMode).IsEqualTo(
                    encryptionMode == FtpEncryptionMode.Implicit ? FtpEncryptionMode.Implicit : FtpEncryptionMode.Explicit);
                await Assert.That(captured.Config.EncryptAuthenticationOnly).IsFalse();
                await Assert.That(captured.Config.DataConnectionEncryption).IsTrue();
            }
        }

        await cancellation.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(() => connection.WaitAsync(TimeSpan.FromSeconds(10)));
        await Assert.That(captured!.IsDisposed).IsTrue();
    }

    [Test]
    public async Task Registration_Remains_Public_Discoverable_And_Hidden_From_IntelliSense()
    {
        var method = typeof(FtpExtensions).GetMethod(nameof(FtpExtensions.RegisterFtpContext))!;

        using (Assert.Multiple())
        {
            await Assert.That(method.IsPublic).IsTrue();
            await Assert.That(method.GetCustomAttribute<ModularPipelinesIntegrationAttribute>()).IsNotNull();
            await Assert.That(method.GetCustomAttribute<EditorBrowsableAttribute>()?.State)
                .IsEqualTo(EditorBrowsableState.Never);
        }

        await Assert.That(await GetService<IFtp>()).IsNotNull();
    }

    [Test]
    public async Task Generated_Tools_Ftp_Remains_Available()
    {
        var result = await await RunModule<FtpAccessModule>();
        await Assert.That(result.ValueOrDefault).IsNotNull();
    }

    [Test]
    public async Task Concurrent_Connections_Are_All_Owned_And_Disposed()
    {
        const int connectionCount = 8;
        var acceptedCount = 0;
        var allAccepted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var ftpServer = LocalFtpServer.Start(async cancellationToken =>
        {
            if (Interlocked.Increment(ref acceptedCount) == connectionCount)
            {
                allAccepted.SetResult();
            }

            await release.Task.WaitAsync(cancellationToken);
        });
        var ftp = await GetService<IFtp>();
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var connections = Enumerable.Range(0, connectionCount)
            .Select(_ => ftp.GetFtpClientAsync(CreateOptions(ftpServer.Port), timeout.Token)).ToArray();
        try
        {
            await allAccepted.Task.WaitAsync(TimeSpan.FromSeconds(10));
            release.SetResult();
            var clients = await Task.WhenAll(connections).WaitAsync(TimeSpan.FromSeconds(10));
            await Assert.That(clients.All(client => client.IsConnected)).IsTrue();

            await ((IAsyncDisposable) ftp).DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(10));
            await Assert.That(clients.All(client => client.IsDisposed)).IsTrue();
        }
        finally
        {
            release.TrySetResult();
            await timeout.CancelAsync();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Disposal_Waits_For_InFlight_Connections_And_Their_Cleanup(bool cancelConnections)
    {
        const int connectionCount = 4;
        var acceptedCount = 0;
        var allAccepted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var connectedServer = LocalFtpServer.Start();
        await using var pendingServer = LocalFtpServer.Start(async cancellationToken =>
        {
            if (Interlocked.Increment(ref acceptedCount) == connectionCount)
            {
                allAccepted.SetResult();
            }

            await release.Task.WaitAsync(cancellationToken);
        });
        var ftp = await GetService<IFtp>();
        var connectedClient = await ftp.GetFtpClientAsync(CreateOptions(connectedServer.Port));
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var captured = new IAsyncFtpClient?[connectionCount];
        var connections = Enumerable.Range(0, connectionCount).Select(index =>
        {
            var options = CreateOptions(pendingServer.Port);
            var configure = options.ClientConfigurator;
            return ftp.GetFtpClientAsync(options with
            {
                ClientConfigurator = client =>
                {
                    captured[index] = client;
                    configure!(client);
                },
            }, cancellation.Token);
        }).ToArray();
        try
        {
            await allAccepted.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var disposal = ((IAsyncDisposable) ftp).DisposeAsync().AsTask();
            var repeatedDisposal = ((IAsyncDisposable) ftp).DisposeAsync().AsTask();
            await Assert.That(disposal.IsCompleted).IsFalse();
            await Assert.That(repeatedDisposal.IsCompleted).IsFalse();

            if (cancelConnections)
            {
                await cancellation.CancelAsync();
            }
            else
            {
                release.SetResult();
            }

            foreach (var connection in connections)
            {
                if (cancelConnections)
                {
                    await Assert.ThrowsAsync<OperationCanceledException>(() => connection.WaitAsync(TimeSpan.FromSeconds(10)));
                }
                else
                {
                    await Assert.ThrowsAsync<ObjectDisposedException>(() => connection.WaitAsync(TimeSpan.FromSeconds(10)));
                }
            }

            await Task.WhenAll(disposal, repeatedDisposal).WaitAsync(TimeSpan.FromSeconds(10));
            await Assert.That(connectedClient.IsDisposed).IsTrue();
            await Assert.That(captured.All(client => client?.IsDisposed == true)).IsTrue();
        }
        finally
        {
            release.TrySetResult();
            await cancellation.CancelAsync();
        }
    }

    [Test]
    public async Task Disposed_Context_Rejects_Connections_Before_Configuration()
    {
        await using var ftpServer = LocalFtpServer.Start();
        var ftp = await GetService<IFtp>();
        await ((IAsyncDisposable) ftp).DisposeAsync();
        var configured = false;
        var options = CreateOptions(ftpServer.Port);
        var configure = options.ClientConfigurator;
        options = options with
        {
            ClientConfigurator = client =>
            {
                configured = true;
                configure!(client);
            },
        };

        await Assert.ThrowsAsync<ObjectDisposedException>(() => ftp.GetFtpClientAsync(options));
        await Assert.That(configured).IsFalse();
    }

    [Test]
    public async Task Disposal_Waits_For_Failed_Configuration_Cleanup()
    {
        var ftp = await GetService<IFtp>();
        var configured = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var release = new ManualResetEventSlim();
        IAsyncFtpClient? captured = null;
        var failure = new InvalidOperationException("configuration failed during disposal");
        var options = CreateOptions(21) with
        {
            ClientConfigurator = client =>
            {
                captured = client;
                configured.SetResult();
                if (!release.Wait(TimeSpan.FromSeconds(10)))
                {
                    throw new TimeoutException("Configuration was not released.");
                }

                throw failure;
            },
        };
        var connection = Task.Run(() => ftp.GetFtpClientAsync(options));
        try
        {
            await configured.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var disposal = ((IAsyncDisposable) ftp).DisposeAsync().AsTask();
            await Assert.That(disposal.IsCompleted).IsFalse();
            release.Set();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => connection.WaitAsync(TimeSpan.FromSeconds(10)));
            await Assert.That(exception).IsSameReferenceAs(failure);
            await disposal.WaitAsync(TimeSpan.FromSeconds(10));
            await Assert.That(captured!.IsDisposed).IsTrue();
        }
        finally
        {
            release.Set();
        }
    }

    private class FtpAccessModule : Module<IFtp>
    {
        protected override Task<IFtp> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(context.Tools.Ftp);
        }
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
