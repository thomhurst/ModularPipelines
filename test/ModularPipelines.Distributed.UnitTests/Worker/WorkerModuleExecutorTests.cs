using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class WorkerModuleExecutorTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Cancellation_Observer_Retries_After_Transient_Failure(
        CancellationToken testCancellation)
    {
        var attempts = 0;
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator.Setup(instance => instance.RegisterWorkerAsync(
                It.IsAny<WorkerRegistration>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.SendHeartbeatAsync(
                It.IsAny<WorkerStatus>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns<WorkerId, IReadOnlySet<Capability>, CancellationToken>(async (_, _, cancellationToken) =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return null;
            });
        coordinator.Setup(instance => instance.WaitForCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException<DistributedCancellationReason>(new InvalidOperationException("Transient coordinator failure"))
                : Task.FromResult(DistributedCancellationReason.Stopped));
        var typeRegistry = new ModuleTypeRegistry();
        var resultRegistry = new ModuleResultRegistry();
        var executor = new WorkerModuleExecutor(
            Mock.Of<IHostApplicationLifetime>(),
            coordinator.Object,
            registeredModules: [],
            typeRegistry,
            new ModuleResultSerializer(typeRegistry),
            Mock.Of<IModuleRunner>(),
            resultRegistry,
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                WorkerHeartbeatInterval = TimeSpan.FromMilliseconds(1),
            }),
            Mock.Of<IParallelLimitProvider>(
                provider => provider.GetMaxDegreeOfParallelism() == 2),
            Mock.Of<IServiceScopeFactory>(),
            artifactLifecycleManager: null,
            NullLogger<WorkerModuleExecutor>.Instance);

        var result = await executor.ExecuteAsync(
            new ExecutionBackendRequest
            {
                Modules = [],
                EstimatedDurations = new Dictionary<ModuleId, TimeSpan>(),
                Context = new ExecutionBackendContext(resultRegistry),
            },
            testCancellation).WaitAsync(testCancellation);

        await Assert.That(result).IsEmpty();
        await Assert.That(attempts).IsEqualTo(2);
    }

    [Test]
    [Timeout(30_000)]
    public async Task Lost_Master_Cancels_Pending_Work_And_Fails_The_Worker(
        CancellationToken testCancellation)
    {
        var dequeueCanceled = false;
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns<WorkerId, IReadOnlySet<Capability>, CancellationToken>(async (_, _, cancellationToken) =>
            {
                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    dequeueCanceled = true;
                    throw;
                }

                return null;
            });
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Lost);

        await Assert.That(async () => await ExecuteAsync(coordinator.Object, testCancellation))
            .Throws<DistributedMasterLostException>();
        await Assert.That(dequeueCanceled).IsTrue();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Claim_Loop_Ended_By_A_Lost_Master_Fails_The_Worker(
        CancellationToken testCancellation)
    {
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Lost);

        await Assert.That(async () => await ExecuteAsync(coordinator.Object, testCancellation, TimeSpan.FromMinutes(1)))
            .Throws<DistributedMasterLostException>();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Claim_Loop_Ended_By_Completion_Succeeds(
        CancellationToken testCancellation)
    {
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Completed);

        var result = await ExecuteAsync(coordinator.Object, testCancellation);

        await Assert.That(result).IsEmpty();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Claim_Loop_Ended_While_The_Master_Is_Unreachable_Fails_The_Worker(
        CancellationToken testCancellation)
    {
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("Coordinator unreachable"));

        await Assert.That(async () => await ExecuteAsync(
                coordinator.Object,
                testCancellation,
                masterTimeout: TimeSpan.FromMilliseconds(200)))
            .Throws<DistributedMasterLostException>();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Final_Master_State_Check_Retries_After_A_Transient_Failure(
        CancellationToken testCancellation)
    {
        var attempts = 0;
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException<DistributedMasterState>(new TimeoutException("Transient coordinator failure"))
                : Task.FromResult(DistributedMasterState.Running));

        var result = await ExecuteAsync(
            coordinator.Object,
            testCancellation,
            heartbeatInterval: TimeSpan.FromMilliseconds(10),
            masterTimeout: TimeSpan.FromSeconds(10));

        await Assert.That(result).IsEmpty();
        await Assert.That(attempts).IsGreaterThanOrEqualTo(2);
    }

    [Test]
    [Timeout(30_000)]
    public async Task Completed_Master_Cancels_Remaining_Work_Without_Failing(
        CancellationToken testCancellation)
    {
        var dequeueCanceled = false;
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns<WorkerId, IReadOnlySet<Capability>, CancellationToken>(async (_, _, cancellationToken) =>
            {
                try
                {
                    await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    dequeueCanceled = true;
                    throw;
                }

                return null;
            });
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Completed);

        var result = await ExecuteAsync(coordinator.Object, testCancellation);

        await Assert.That(result).IsEmpty();
        await Assert.That(dequeueCanceled).IsTrue();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Unreachable_Master_Fails_The_Worker_After_The_Master_Timeout(
        CancellationToken testCancellation)
    {
        var coordinator = CreateCoordinator();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns<WorkerId, IReadOnlySet<Capability>, CancellationToken>(async (_, _, cancellationToken) =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return null;
            });
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("Coordinator unreachable"));

        await Assert.That(async () => await ExecuteAsync(
                coordinator.Object,
                testCancellation,
                masterTimeout: TimeSpan.FromMilliseconds(200)))
            .Throws<DistributedMasterLostException>();
    }

    [Test]
    [Timeout(30_000)]
    public async Task Work_That_Ignores_Cancellation_Does_Not_Keep_A_Worker_Alive_After_Its_Master_Is_Lost(
        CancellationToken testCancellation)
    {
        var coordinator = CreateCoordinator();
        var neverCompletes = new TaskCompletionSource<ModuleLease?>();
        coordinator.Setup(instance => instance.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns(neverCompletes.Task);
        coordinator.Setup(instance => instance.GetMasterStateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DistributedMasterState.Lost);

        await Assert.That(async () => await ExecuteAsync(
                coordinator.Object,
                testCancellation,
                canceledWorkGracePeriod: TimeSpan.FromMilliseconds(100)))
            .Throws<DistributedMasterLostException>();
    }

    private static Mock<IDistributedWorkerCoordinator> CreateCoordinator()
    {
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator.Setup(instance => instance.RegisterWorkerAsync(
                It.IsAny<WorkerRegistration>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.SendHeartbeatAsync(
                It.IsAny<WorkerStatus>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.WaitForCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(async cancellationToken =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return DistributedCancellationReason.Stopped;
            });
        return coordinator;
    }

    private static Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
        IDistributedWorkerCoordinator coordinator,
        CancellationToken cancellationToken,
        TimeSpan? heartbeatInterval = null,
        TimeSpan? masterTimeout = null,
        TimeSpan? canceledWorkGracePeriod = null)
    {
        var typeRegistry = new ModuleTypeRegistry();
        var resultRegistry = new ModuleResultRegistry();
        var executor = new WorkerModuleExecutor(
            Mock.Of<IHostApplicationLifetime>(),
            coordinator,
            registeredModules: [],
            typeRegistry,
            new ModuleResultSerializer(typeRegistry),
            Mock.Of<IModuleRunner>(),
            resultRegistry,
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                WorkerHeartbeatInterval = heartbeatInterval ?? TimeSpan.FromMilliseconds(10),
                MasterTimeout = masterTimeout ?? TimeSpan.FromMinutes(1),
            }),
            Mock.Of<IParallelLimitProvider>(
                provider => provider.GetMaxDegreeOfParallelism() == 2),
            Mock.Of<IServiceScopeFactory>(),
            artifactLifecycleManager: null,
            NullLogger<WorkerModuleExecutor>.Instance);
        if (canceledWorkGracePeriod is { } gracePeriod)
        {
            executor.CanceledWorkGracePeriod = gracePeriod;
        }

        return executor.ExecuteAsync(
            new ExecutionBackendRequest
            {
                Modules = [],
                EstimatedDurations = new Dictionary<ModuleId, TimeSpan>(),
                Context = new ExecutionBackendContext(resultRegistry),
            },
            cancellationToken).WaitAsync(cancellationToken);
    }
}
