using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class CloudflaredTunnelTests
{
    // This OS utility never emits a tunnel URL. No cloudflared install or network is required.
    private static SignalRTunnelOptions TunnelOptions => new()
    {
        Enabled = true,
        CloudflaredPath = "whoami",
        StartupTimeout = TimeSpan.Zero,
    };

    [Test]
    public async Task Caller_Cancellation_Preserves_The_Callers_Token()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await using var tunnel = new CloudflaredTunnel();

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => tunnel.StartAsync(
            new Uri("http://127.0.0.1:12345"),
            TunnelOptions with { StartupTimeout = TimeSpan.FromSeconds(30) },
            NullLogger.Instance,
            cancellation.Token));

        await Assert.That(exception!.CancellationToken).IsEqualTo(cancellation.Token);
    }

    [Test]
    public async Task Startup_Timeout_Allows_Repeated_Disposal()
    {
        await using var tunnel = new CloudflaredTunnel();

        await Assert.ThrowsAsync<TimeoutException>(() => tunnel.StartAsync(
            new Uri("http://127.0.0.1:12345"), TunnelOptions, NullLogger.Instance, default));

        await tunnel.DisposeAsync();
        await tunnel.DisposeAsync();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Failed_Tunnel_Startup_Releases_The_Master_Port(bool cancel)
    {
        using var cancellation = new CancellationTokenSource();
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.Log(
                It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Callback(new InvocationAction(invocation =>
            {
                if (cancel && invocation.Arguments[2]?.ToString()?.StartsWith("Cloudflared:", StringComparison.Ordinal) == true)
                {
                    cancellation.Cancel();
                }
            }));
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);
        var coordinator = new InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions()));
        var state = new SignalRMasterState(coordinator, "tunnel-test");
        var options = new SignalRDistributedOptions
        {
            ListenUrl = new Uri("http://127.0.0.1:0"),
            Tunnel = TunnelOptions with { StartupTimeout = cancel ? TimeSpan.FromSeconds(30) : TimeSpan.Zero },
        };
        await using var host = new MasterServerHost();

        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                host.StartAsync(options, state, null, loggerFactory.Object, cancellation.Token));
        }
        else
        {
            await Assert.ThrowsAsync<TimeoutException>(() =>
                host.StartAsync(options, state, null, loggerFactory.Object, cancellation.Token));
        }

        await host.DisposeAsync();
        await host.DisposeAsync();
        using var listener = new TcpListener(IPAddress.Loopback, host.BoundUrl.Port);
        listener.Start();
    }
}
