using Kevlar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.UnitTests;

public class WorkerModuleExecutorTests
{
    private sealed class WorkerConcurrencyProbe
    {
        private readonly TaskCompletionSource _twoStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _release =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _active;
        private int _peakActive;

        public Task TwoStarted => _twoStarted.Task;

        public int PeakActive => Volatile.Read(ref _peakActive);

        public async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var active = Interlocked.Increment(ref _active);
            UpdateMaximum(ref _peakActive, active);
            if (active == 2)
            {
                _twoStarted.TrySetResult();
            }

            await _release.Task.WaitAsync(cancellationToken);
            Interlocked.Decrement(ref _active);
            return active;
        }

        public void Release() => _release.TrySetResult();
    }

    private sealed class ParallelWorkerModuleA(WorkerConcurrencyProbe probe) : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => probe.ExecuteAsync(cancellationToken);
    }

    private sealed class ParallelWorkerModuleB(WorkerConcurrencyProbe probe) : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => probe.ExecuteAsync(cancellationToken);
    }

    private abstract class RetryingModule : Module<int>
    {
        public int AttemptCount { get; private set; }

        public Type? AmbientModuleType { get; private set; }

        public IModuleLogger? AmbientLogger { get; private set; }

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            AmbientModuleType = AmbientModuleOutputContext.Current?.ModuleType;
            AmbientLogger = AmbientModuleOutputContext.Current?.Logger;
            if (++AttemptCount < 3)
            {
                throw new InvalidOperationException("Retry this attempt.");
            }

            return Task.FromResult(AttemptCount);
        }
    }

    private sealed class DeclarativeRetryModule : RetryingModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithRetry(2, TimeSpan.Zero);
    }

    private sealed class ShieldFactoryRetryModule : RetryingModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithShield(_ => Shield.Retry(2));
    }

    private sealed class DefaultRetryModule : RetryingModule;

    [Test]
    public Task Worker_Uses_Module_Retry_Configuration(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<DeclarativeRetryModule>(null, cancellationToken);

    [Test]
    public Task Worker_Uses_Module_Shield_Factory(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<ShieldFactoryRetryModule>(null, cancellationToken);

    [Test]
    public Task Worker_Uses_Default_Retry_Configuration(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<DefaultRetryModule>(
            builder => builder.ConfigureOptions(options => options with { DefaultRetryCount = 2 }),
            cancellationToken);

    [Test]
    [Timeout(10_000)]
    public async Task Worker_Executes_Assignments_Concurrently(CancellationToken cancellationToken)
    {
        var probe = new WorkerConcurrencyProbe();
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(probe);
        builder.AddModule<ParallelWorkerModuleA>();
        builder.AddModule<ParallelWorkerModuleB>();
        await using var pipeline = await builder.BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().ToArray();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(typeRegistry);
        foreach (var module in modules)
        {
            typeRegistry.Register(module.GetType());
            await coordinator.EnqueueModuleAsync(CreateAssignment(module), cancellationToken);
        }

        var executor = new WorkerModuleExecutor(
            pipeline.Services.GetRequiredService<IHostApplicationLifetime>(),
            coordinator,
            modules,
            typeRegistry,
            serializer,
            pipeline.Services.GetRequiredService<IModuleRunner>(),
            pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
            pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
            pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
            MsOptions.Create(new DistributedOptions
            {
                InstanceIndex = 1,
                AutoDetectOsCapability = false,
                MaxParallelism = 2,
            }),
            pipeline.Services.GetRequiredService<IParallelLimitProvider>(),
            pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
            null,
            NullLogger<WorkerModuleExecutor>.Instance);

        var executionTask = executor.ExecuteAsync(modules);
        await probe.TwoStarted.WaitAsync(cancellationToken);
        await coordinator.SignalCompletionAsync(cancellationToken);
        probe.Release();

        var results = await executionTask.WaitAsync(cancellationToken);

        using (Assert.Multiple())
        {
            await Assert.That(probe.PeakActive).IsEqualTo(2);
            await Assert.That(results).Count().IsEqualTo(2);
        }
    }

    private static async Task AssertWorkerRetriesAsync<TModule>(
        Action<PipelineBuilder>? configureBuilder,
        CancellationToken cancellationToken)
        where TModule : RetryingModule
    {
        var builder = TestPipelineBuilder.Create();
        configureBuilder?.Invoke(builder);
        builder.AddModule<TModule>();
        await using var pipeline = await builder.BuildAsync();
        var module = pipeline.Services.GetServices<IModule>()
            .OfType<TModule>()
            .Single();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(TModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var assignment = new ModuleAssignment(
            typeof(TModule).FullName!,
            typeof(int).FullName!,
            new HashSet<Capability>(),
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, false));
        await coordinator.EnqueueModuleAsync(assignment, cancellationToken);
        var executor = new WorkerModuleExecutor(
            pipeline.Services.GetRequiredService<IHostApplicationLifetime>(),
            coordinator,
            [module],
            typeRegistry,
            serializer,
            pipeline.Services.GetRequiredService<IModuleRunner>(),
            pipeline.Services.GetRequiredService<IModuleResultRegistry>(),
            pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
            pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
            MsOptions.Create(new DistributedOptions
            {
                InstanceIndex = 1,
                AutoDetectOsCapability = false,
            }),
            pipeline.Services.GetRequiredService<IParallelLimitProvider>(),
            pipeline.Services.GetRequiredService<IServiceScopeFactory>(),
            null,
            NullLogger<WorkerModuleExecutor>.Instance);

        var executionTask = executor.ExecuteAsync([module]);
        var serializedResult = await coordinator.WaitForResultAsync(
            typeof(TModule).FullName!,
            cancellationToken).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        await coordinator.SignalCompletionAsync(cancellationToken);
        await executionTask.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
        var result = serializer.Deserialize(serializedResult);

        using (Assert.Multiple())
        {
            await Assert.That(module.AttemptCount).IsEqualTo(3);
            await Assert.That(module.AmbientModuleType).IsEqualTo(typeof(TModule));
            await Assert.That(module.AmbientLogger).IsNotNull();
            await Assert.That(result?.ExceptionOrDefault).IsNull();
            await Assert.That(result?.ValueOrDefault).IsEqualTo(3);
        }
    }

    private static ModuleAssignment CreateAssignment(IModule module) => new(
        module.GetType().FullName!,
        module.ResultType.FullName!,
        new HashSet<Capability>(),
        DateTimeOffset.UtcNow,
        new ModuleAssignmentConfiguration(null, false));

    private static void UpdateMaximum(ref int maximum, int candidate)
    {
        var current = Volatile.Read(ref maximum);
        while (candidate > current)
        {
            var observed = Interlocked.CompareExchange(ref maximum, candidate, current);
            if (observed == current)
            {
                return;
            }

            current = observed;
        }
    }
}
