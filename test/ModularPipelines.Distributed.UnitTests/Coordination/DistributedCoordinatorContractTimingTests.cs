using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Testing.Distributed;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class DistributedCoordinatorContractTimingTests
{
    [Test]
    [Arguments(1)]
    [Arguments(3)]
    public Task Expiration_Budget_Allows_Slow_Backend_Setup(int budget) =>
        DistributedCoordinatorContract.LeaseExpirationBudgetFailsModuleAsync(new SlowSetupCoordinator(budget), budget);

    [Test]
    public Task Expiration_Recovery_Allows_Slow_Backend_Setup()
    {
        var clock = new FakeTimeProvider();
        return RunRecoveryAsync(new SlowSetupCoordinator(2, clock), clock);
    }

    [Test]
    [Arguments(1)]
    [Arguments(4)]
    public Task Expiration_Recovery_Allows_Slow_Publication(int parallelChecks) =>
        Task.WhenAll(Enumerable.Range(0, parallelChecks).Select(_ =>
        {
            var clock = new FakeTimeProvider();
            return RunRecoveryAsync(new SlowPublicationCoordinator(clock), clock);
        }));

    private static Task RunRecoveryAsync(IDistributedMasterCoordinator coordinator, FakeTimeProvider clock) =>
        DistributedCoordinatorContract.LeaseExpirationRecoverySucceedsAsync(coordinator, token =>
        {
            token.ThrowIfCancellationRequested();
            clock.Advance(DistributedCoordinatorContract.LeaseTimeout * 3);
            return Task.CompletedTask;
        });

    private sealed class SlowPublicationCoordinator(TimeProvider clock)
        : InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions
        {
            WorkerTimeout = DistributedCoordinatorContract.LeaseTimeout,
        }), clock), IDistributedMasterCoordinator
    {
        async Task IDistributedWorkerCoordinator.PublishResultAsync(
            SerializedModuleResult result, ModuleLease? lease, CancellationToken cancellationToken)
        {
            // A healthy recovered worker can be descheduled before its result reaches the coordinator.
            await Task.Delay(DistributedCoordinatorContract.LeaseTimeout * 3, cancellationToken);
            await PublishResultAsync(result, lease, cancellationToken);
        }
    }

    private sealed class SlowSetupCoordinator(int budget, TimeProvider? clock = null)
        : InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions
        {
            WorkerTimeout = DistributedCoordinatorContract.LeaseTimeout,
            MaxModuleLeaseExpirations = budget,
        }), clock), IDistributedMasterCoordinator
    {
        private bool _delayed;

        async Task IDistributedMasterCoordinator.EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
        {
            if (!_delayed)
            {
                _delayed = true;
                // Model backend/scheduling latency before a lease exists; exceed the former whole-check deadline.
                await Task.Delay(TimeSpan.FromSeconds(11), cancellationToken);
            }

            await EnqueueModuleAsync(assignment, cancellationToken);
        }
    }
}
