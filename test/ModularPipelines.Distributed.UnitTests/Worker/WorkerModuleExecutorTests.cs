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
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class WorkerModuleExecutorTests
{
    [Test]
    [Timeout(5_000)]
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
        coordinator.Setup(instance => instance.DequeueModuleAsync(
                It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .Returns<IReadOnlySet<Capability>, CancellationToken>(async (_, cancellationToken) =>
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return null;
            });
        coordinator.Setup(instance => instance.WaitForCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(() => Interlocked.Increment(ref attempts) == 1
                ? Task.FromException(new InvalidOperationException("Transient coordinator failure"))
                : Task.CompletedTask);
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
                AutoDetectOsCapability = false,
                WorkerHeartbeatInterval = TimeSpan.FromMilliseconds(1),
            }),
            Mock.Of<IServiceScopeFactory>(),
            artifactLifecycleManager: null,
            NullLogger<WorkerModuleExecutor>.Instance);

        var result = await executor.ExecuteAsync([]).WaitAsync(testCancellation);

        await Assert.That(result).IsEmpty();
        await Assert.That(attempts).IsEqualTo(2);
    }
}
