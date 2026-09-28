using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.SignalR.Coordination;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

/// <summary>
/// Starts a real SignalR master on a free loopback port and connects workers to it through a
/// discovery stub that captures the advertised endpoint.
/// </summary>
internal sealed class SignalRTestMaster : IAsyncDisposable
{
    public const string RunId = "signalr-test-run";

    private readonly List<IAsyncDisposable> _disposables = [];

    private SignalRTestMaster(SignalRDistributedCoordinatorFactory factory, CapturingDiscovery discovery)
    {
        Factory = factory;
        Discovery = discovery;
    }

    public SignalRDistributedCoordinatorFactory Factory { get; }

    public CapturingDiscovery Discovery { get; }

    public IDistributedMasterCoordinator Master { get; private set; } = null!;

    public static async Task<SignalRTestMaster> StartAsync(
        Action<SignalRDistributedOptions>? configure = null,
        TimeSpan? workerTimeout = null)
    {
        var options = new SignalRDistributedOptions
        {
            ListenUrl = "http://127.0.0.1:0",
            MaxReconnectAttempts = 0,
            ConnectionTimeout = TimeSpan.FromSeconds(10),
        };
        configure?.Invoke(options);
        var discovery = new CapturingDiscovery();
        var factory = new SignalRDistributedCoordinatorFactory(
            MsOptions.Create(options),
            MsOptions.Create(new DistributedOptions
            {
                RunId = RunId,
                WorkerTimeout = workerTimeout ?? TimeSpan.FromSeconds(30),
            }),
            NullLoggerFactory.Instance,
            discovery);
        var master = new SignalRTestMaster(factory, discovery)
        {
            Master = await factory.CreateMasterAsync(CancellationToken.None)
        };
        master._disposables.Add((IAsyncDisposable) master.Master);
        return master;
    }

    public async Task<SignalRWorkerCoordinator> ConnectWorkerAsync()
    {
        var worker = (SignalRWorkerCoordinator) await Factory.CreateWorkerAsync(CancellationToken.None);
        _disposables.Add(worker);
        return worker;
    }

    public static WorkerRegistration Registration(string workerId, string runId = RunId) => new()
    {
        WorkerId = new WorkerId(workerId),
        Capabilities = [new Capability("linux")],
        RegisteredAt = DateTimeOffset.UtcNow,
        MaxParallelism = 2,
        RunId = runId,
    };

    public async ValueTask DisposeAsync()
    {
        for (var i = _disposables.Count - 1; i >= 0; i--)
        {
            await _disposables[i].DisposeAsync();
        }
    }

    internal sealed class CapturingDiscovery : IMasterDiscovery
    {
        private readonly TaskCompletionSource<MasterEndpoint> _endpoint =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task<MasterEndpoint> Endpoint => _endpoint.Task;

        public MasterEndpoint? Override { get; set; }

        public Task AdvertiseMasterEndpointAsync(MasterEndpoint endpoint, CancellationToken cancellationToken)
        {
            _endpoint.TrySetResult(endpoint);
            return Task.CompletedTask;
        }

        public async Task<MasterEndpoint> DiscoverMasterEndpointAsync(CancellationToken cancellationToken) =>
            Override ?? await _endpoint.Task.WaitAsync(cancellationToken);
    }
}
