using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Distributed.UnitTests.Integration;

public class SkippedDependencyPipelineTests
{
    [Test]
    [Timeout(30_000)]
    public async Task Master_Reuses_Result_After_Worker_Consumes_And_Remote_Copy_Is_Lost(CancellationToken cancellationToken)
    {
        var coordinator = new EvictingCoordinator();
        var runId = Guid.NewGuid().ToString("N");
        PipelineBuilder CreateBuilder(int index)
        {
            var builder = TestPipelineBuilder.Create();
            builder.AddDistributedMode(options =>
            {
                options.InstanceIndex = index;
                options.RunId = runId;
                options.TotalInstances = 2;
                options.MinimumWorkerCount = 1;
                options.CapabilityTimeout = TimeSpan.FromSeconds(10);
                options.ModuleResultTimeout = TimeSpan.FromSeconds(10);
                options.Capabilities = [index == 0 ? "test-master" : "test-worker"];
            });
            builder.Services.AddSingleton(coordinator);
            builder.Services.AddSingleton<IDistributedMasterCoordinator>(coordinator);
            builder.Services.AddSingleton<IDistributedWorkerCoordinator>(coordinator);
            builder.AddModule<RetainedProducer>();
            builder.AddModule<WorkerTransfer>();
            builder.AddModule<CompletedResultConsumer>();
            return builder;
        }

        await using var master = await CreateBuilder(0).BuildAsync();
        await using var worker = await CreateBuilder(1).BuildAsync();
        await Task.WhenAll(master.RunAsync(cancellationToken), worker.RunAsync(cancellationToken));

        var results = master.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(results.GetResult(typeof(CompletedResultConsumer))!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(coordinator.ReadsAfterRemoval).IsEqualTo(0);
        await Assert.That(coordinator.ResultRemoved).IsTrue();
    }

    [Test]
    [Timeout(30_000)]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Skipped_Barrier_Completes_Downstream_Modules(bool includePublishing, CancellationToken cancellationToken)
    {
        var builder = TestPipelineBuilder.Create();
        builder.AddDistributedMode(options =>
        {
            options.TotalInstances = 1;
            options.MaxParallelism = 2;
            options.ModuleResultTimeout = TimeSpan.FromSeconds(10);
        });
        builder.AddModule<PrevalidatedTests>();
        builder.AddModule<TestBarrier>();
        builder.AddModule<Package>();
        if (includePublishing)
        {
            builder.AddModule<Publish>();
        }

        await using var pipeline = await builder.BuildAsync();
        await pipeline.RunAsync(cancellationToken);

        var results = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        await Assert.That(results.GetResult(typeof(TestBarrier))!.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(results.GetResult(typeof(Package))!.Status).IsEqualTo(ModuleStatus.Skipped);
        if (includePublishing)
        {
            await Assert.That(results.GetResult(typeof(Publish))!.Status).IsEqualTo(ModuleStatus.Skipped);
        }
    }

    private sealed class PrevalidatedTests : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Validated by the prerequisite job"));

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The prevalidated test command must not run.");
    }

    [DependsOn<PrevalidatedTests>]
    private sealed class TestBarrier : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The skipped barrier must not run.");
    }

    [DependsOn<TestBarrier>]
    private sealed class Package : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("The package must not run after a skipped barrier.");
    }

    [DependsOn<Package>]
    private sealed class Publish : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("Publishing is disabled"));

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Publishing is disabled.");
    }

    [RequiresCapability("test-master")]
    private sealed class RetainedProducer : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("retained value");
    }

    [RequiresCapability("test-worker")]
    [DependsOn<RetainedProducer>]
    private sealed class WorkerTransfer(EvictingCoordinator coordinator, IOptions<DistributedOptions> options) : Module<int>
    {
        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var producer = await context.GetModule<RetainedProducer>();
            if (producer.Value != "retained value")
            {
                throw new InvalidOperationException("The worker did not receive the producer result.");
            }

            coordinator.RemoveResult();
            return options.Value.InstanceIndex;
        }
    }

    [RequiresCapability("test-master")]
    [DependsOn<RetainedProducer>]
    [DependsOn<WorkerTransfer>]
    private sealed class CompletedResultConsumer : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var worker = await context.GetModule<WorkerTransfer>();
            var producer = await context.GetModule<RetainedProducer>();
            if (worker.Value != 1 || producer.Value != "retained value")
            {
                throw new InvalidOperationException("The consumer did not receive both completed dependency results.");
            }

            return true;
        }
    }

    private sealed class EvictingCoordinator : InMemoryDistributedCoordinator, IDistributedMasterCoordinator
    {
        private int _removed;
        private int _readsAfterRemoval;

        public bool ResultRemoved => Volatile.Read(ref _removed) != 0;

        public int ReadsAfterRemoval => Volatile.Read(ref _readsAfterRemoval);

        public void RemoveResult() => Volatile.Write(ref _removed, 1);

        Task<SerializedModuleResult> IDistributedWorkerCoordinator.WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken)
        {
            if (ResultRemoved && moduleId == ModuleId.FromType(typeof(RetainedProducer)))
            {
                Interlocked.Increment(ref _readsAfterRemoval);
                return Task.FromException<SerializedModuleResult>(new InvalidOperationException("The retained producer result is unavailable."));
            }

            return WaitForResultAsync(moduleId, cancellationToken);
        }
    }
}
