using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class DeferredCoordinatorTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Deferred_Coordinators_Preserve_Lease_Reconciliation(bool masterWrapper)
    {
        var clock = new FakeTimeProvider();
        var inner = new DisposableCoordinator(clock);
        var factory = new Factory(inner);
        IDistributedWorkerCoordinator worker = masterWrapper
            ? new DeferredMasterCoordinator(factory)
            : new DeferredWorkerCoordinator(factory);
        await ModularPipelines.TestHelpers.LeaseReconciliationChecks.ReconnectAfterReassignmentAsync(
            inner, () => { clock.Advance(TimeSpan.FromSeconds(31)); return Task.CompletedTask; }, worker: worker);
        await ((IAsyncDisposable) worker).DisposeAsync();
    }

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

    [Test]
    [Timeout(30_000)]
    public async Task Disposal_Stops_Waiting_For_A_Creation_That_Ignores_Cancellation(CancellationToken cancellationToken)
    {
        var inner = new DisposableCoordinator();
        var creation = new TaskCompletionSource<IDistributedMasterCoordinator>(TaskCreationOptions.RunContinuationsAsynchronously);
        var deferred = new DeferredMasterCoordinator(new HangingFactory(creation.Task));
        using var shortWait = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));

        await Assert.That(async () => await deferred.GetActiveLeasesAsync(shortWait.Token))
            .Throws<OperationCanceledException>();
        // Disposal gives the creation a grace period, then stops waiting.
        await deferred.DisposeAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(15), cancellationToken);

        // The backend a late creation produces is disposed once it exists.
        creation.SetResult(inner);
        while (!inner.Disposed)
        {
            await Task.Delay(10, cancellationToken);
        }
    }

    [Test]
    public async Task Failed_Creation_Is_Retried_By_The_Next_Caller()
    {
        var inner = new DisposableCoordinator();
        var deferred = new DeferredMasterCoordinator(new FailOnceFactory(inner));

        await Assert.That(async () => await deferred.GetActiveLeasesAsync(CancellationToken.None))
            .Throws<InvalidOperationException>();
        var leases = await deferred.GetActiveLeasesAsync(CancellationToken.None);
        await deferred.DisposeAsync();

        await Assert.That(leases).IsEmpty();
        await Assert.That(inner.Disposed).IsTrue();
    }

    private sealed class HangingFactory(Task<IDistributedMasterCoordinator> creation) : IDistributedCoordinatorFactory
    {
        // Ignores cancellation, like a factory blocked on a connection without a timeout.
        public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) => creation;

        public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class FailOnceFactory(DisposableCoordinator coordinator) : IDistributedCoordinatorFactory
    {
        private int _calls;

        public Task<IDistributedMasterCoordinator> CreateMasterAsync(CancellationToken cancellationToken) =>
            Interlocked.Increment(ref _calls) == 1
                ? Task.FromException<IDistributedMasterCoordinator>(new InvalidOperationException("Simulated connection failure"))
                : Task.FromResult<IDistributedMasterCoordinator>(coordinator);

        public Task<IDistributedWorkerCoordinator> CreateWorkerAsync(CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }

    private sealed class DisposableCoordinator(TimeProvider? clock = null)
        : InMemoryDistributedCoordinator(timeProvider: clock), IAsyncDisposable
    {
        private volatile bool _disposed;

        public bool Disposed => _disposed;

        public ValueTask DisposeAsync()
        {
            _disposed = true;
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
