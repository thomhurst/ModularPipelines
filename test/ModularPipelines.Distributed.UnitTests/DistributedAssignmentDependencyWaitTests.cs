using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.Distributed.UnitTests;

public class DistributedAssignmentDependencyWaitTests
{
    private sealed class DependencyModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(1);
    }

    [DependsOn<DependencyModule>]
    private sealed class ConsumerModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(2);
    }

    [Test]
    [Arguments("pipeline")]
    [Arguments("lease")]
    [Arguments("timeout")]
    [Timeout(10_000)]
    public async Task Interrupted_Dependency_Wait_Does_Not_Execute_Consumer(string interruption, CancellationToken cancellationToken)
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddModule<DependencyModule>().AddModule<ConsumerModule>();
        await using var pipeline = await builder.BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().ToArray();
        var types = new ModuleTypeRegistry();
        foreach (var module in modules)
        {
            types.Register(module.GetType());
        }

        var clock = new FakeTimeProvider();
        using var execution = new CancellationTokenSource();
        using var leaseLost = new CancellationTokenSource();
        var pending = new TaskCompletionSource<SerializedModuleResult>(TaskCreationOptions.RunContinuationsAsynchronously);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        coordinator.Setup(value => value.WaitForResultAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()))
            .Returns(pending.Task);
        SerializedModuleResult? published = null;
        coordinator.Setup(value => value.PublishResultAsync(
                It.IsAny<SerializedModuleResult>(), It.IsAny<ModuleLease?>(), It.IsAny<CancellationToken>()))
            .Callback<SerializedModuleResult, ModuleLease?, CancellationToken>((result, _, _) => published = result)
            .Returns(Task.CompletedTask);
        var runner = new Mock<IModuleRunner>(MockBehavior.Strict);
        var serializer = new ModuleResultSerializer(types);
        var executor = new DistributedAssignmentExecutor(
            coordinator.Object,
            types,
            serializer,
            runner.Object,
            pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
            pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
            pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
            pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
            null, null, null,
            NullLogger.Instance);
        var assignment = DistributedTestData.Assignment(ModuleId.FromType(typeof(ConsumerModule)), types.GetPipelineSchemaVersion()) with
        {
            DependencyResultReferences = [DistributedTestData.Dependency(ModuleId.FromType(typeof(DependencyModule)))],
        };
        var run = executor.ExecuteAsync(
            DistributedTestData.Lease(assignment),
            clock.GetUtcNow(),
            DependencyResultApplicator.BuildModuleLookup(modules),
            new DependencyResultCache(coordinator.Object, cancellationToken, clock),
            execution.Token,
            leaseLost.Token);

        if (interruption == "timeout")
        {
            clock.Advance(TimeSpan.FromSeconds(30));
        }
        else
        {
            await (interruption == "lease" ? leaseLost : execution).CancelAsync();
        }

        await run.WaitAsync(cancellationToken);

        runner.Verify(value => value.ExecuteWithoutDependencyWaitAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()), Times.Never);
        if (interruption == "lease")
        {
            await Assert.That(published).IsNull();
        }
        else
        {
            await Assert.That(published).IsNotNull();
            var failure = serializer.Deserialize(published!)!;
            await Assert.That(failure.ExceptionOrDefault).IsNotNull();
            await Assert.That(failure.Status).IsEqualTo(interruption == "timeout" ? ModuleStatus.Failed : ModuleStatus.Canceled);
            if (interruption == "timeout")
            {
                await Assert.That(published!.Payload).Contains("Dependency result unavailable");
            }
        }

        // Release the intentionally uncooperative coordinator after checking bounded execution.
        pending.SetCanceled(cancellationToken);
    }
}
