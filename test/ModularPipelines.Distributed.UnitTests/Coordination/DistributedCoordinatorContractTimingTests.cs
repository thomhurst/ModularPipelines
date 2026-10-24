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
    public Task Expiration_Recovery_Allows_Slow_Backend_Setup() =>
        DistributedCoordinatorContract.LeaseExpirationRecoverySucceedsAsync(new SlowSetupCoordinator(2));

    private sealed class SlowSetupCoordinator(int budget)
        : InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions
        {
            WorkerTimeout = DistributedCoordinatorContract.LeaseTimeout,
            MaxModuleLeaseExpirations = budget,
        })), IDistributedMasterCoordinator
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
