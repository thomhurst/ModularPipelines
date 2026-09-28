using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class DeferredCoordinatorTests
{
    [Test]
    public async Task Master_Wrapper_Forwards_Calls_And_Disposes_The_Created_Coordinator()
    {
        var inner = new DisposableCoordinator();
        var deferred = new DeferredMasterCoordinator(new Factory(inner));

        await deferred.EnqueueModuleAsync(DistributedTestData.Assignment(new ModuleId("Deferred")), CancellationToken.None);
        var withdrawn = await deferred.WithdrawAssignmentAsync(new ModuleId("Deferred"), CancellationToken.None);
        await deferred.DisposeAsync();

        await Assert.That(withdrawn).IsTrue();
        await Assert.That(inner.Disposed).IsTrue();
        await Assert.That(async () => await deferred.GetActiveLeasesAsync(CancellationToken.None))
            .Throws<ObjectDisposedException>();
    }

    [Test]
    public async Task Worker_Wrapper_Disposes_The_Created_Coordinator()
    {
        var inner = new DisposableCoordinator();
        var deferred = new DeferredWorkerCoordinator(new Factory(inner));

        await deferred.SendHeartbeatAsync(new WorkerStatus { WorkerId = DistributedTestData.Worker }, CancellationToken.None);
        await deferred.DisposeAsync();

        await Assert.That(inner.Disposed).IsTrue();
    }

    private sealed class DisposableCoordinator() : InMemoryDistributedCoordinator, IAsyncDisposable
    {
        public bool Disposed { get; private set; }

        public ValueTask DisposeAsync()
        {
            Disposed = true;
            return ValueTask.CompletedTask;
        }
    }

    private sealed class Factory(DisposableCoordinator coordinator) : IDistributedCoordinatorFactory
    {
        public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IDistributedMasterCoordinator>(coordinator);

        public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IDistributedWorkerCoordinator>(coordinator);
    }
}
