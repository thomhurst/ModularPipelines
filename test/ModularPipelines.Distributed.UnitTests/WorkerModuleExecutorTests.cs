using System.Collections.Concurrent;
using Moq;
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
using ModularPipelines.Models;
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

    private sealed class CyclicOutput
    {
        public CyclicOutput? Next { get; set; }
    }

    private sealed class CyclicOutputModule : Module<CyclicOutput>
    {
        protected internal override Task<CyclicOutput> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var output = new CyclicOutput();
            output.Next = output;
            return Task.FromResult(output);
        }
    }

    [Test]
    public async Task Unserializable_Success_Publishes_A_Serialization_Failure(CancellationToken cancellationToken)
    {
        var (_, result) = await ExecuteWorkerModuleAsync<CyclicOutputModule, CyclicOutput>(
            null, cancellationToken);

        await Assert.That(result?.ExceptionOrDefault).IsNotNull();
        await Assert.That(result!.ExceptionOrDefault!.Message).Contains("cycle");
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(result.ValueOrDefault).IsNull();
    }

    private sealed class RejectFirstPublicationCoordinator(IDistributedWorkerCoordinator inner) : IDistributedWorkerCoordinator
    {
        private int _publications;

        public Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken) =>
            Interlocked.Increment(ref _publications) == 1
                ? Task.FromException(new InvalidOperationException("Publication was rejected."))
                : inner.PublishResultAsync(result, cancellationToken);

        public Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<Capability> capabilities, CancellationToken cancellationToken) =>
            inner.DequeueModuleAsync(capabilities, cancellationToken);

        public Task<SerializedModuleResult> WaitForResultAsync(ModuleId moduleId, CancellationToken cancellationToken) =>
            inner.WaitForResultAsync(moduleId, cancellationToken);

        public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken) =>
            inner.RegisterWorkerAsync(registration, cancellationToken);

        public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken) =>
            inner.SendHeartbeatAsync(status, cancellationToken);

        public Task WaitForCancellationAsync(CancellationToken cancellationToken) =>
            inner.WaitForCancellationAsync(cancellationToken);
    }

    [Test]
    [Arguments("")]
    [Arguments("different-build")]
    public async Task Schema_Mismatch_Publishes_Failure_Without_Executing_Module(
        string schemaVersion, CancellationToken cancellationToken)
    {
        var (module, result) = await ExecuteWorkerModuleAsync<DeclarativeRetryModule, int>(
            null, cancellationToken, schemaVersion: schemaVersion);

        await Assert.That(module.AttemptCount).IsEqualTo(0);
        await Assert.That(result?.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(result!.ExceptionOrDefault!.Message).Contains("schema mismatch");
        await Assert.That(result.ExceptionOrDefault.Message).Contains("master assignment");
    }

    [Test]
    public Task Publication_Rejection_Does_Not_Replace_Accepted_Success(CancellationToken cancellationToken) =>
        AssertWorkerRetriesAsync<DeclarativeRetryModule>(null, cancellationToken, rejectFirstPublication: true);

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
        }

        foreach (var module in modules)
        {
            await coordinator.EnqueueModuleAsync(CreateAssignment(module, typeRegistry), cancellationToken);
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

    [Test]
    [Timeout(10_000)]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Failures_Publish_Results_For_All_Claimed_Assignments(bool cancelled, CancellationToken cancellationToken)
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton(new WorkerConcurrencyProbe());
        builder.AddModule<ParallelWorkerModuleA>();
        builder.AddModule<ParallelWorkerModuleB>();
        await using var pipeline = await builder.BuildAsync();
        var modules = pipeline.Services.GetServices<IModule>().ToArray();
        var typeRegistry = new ModuleTypeRegistry();
        foreach (var module in modules)
        {
            typeRegistry.Register(module.GetType());
        }

        var assignments = new ConcurrentQueue<ModuleAssignment>(modules.Select(module => CreateAssignment(module, typeRegistry)));
        var published = new ConcurrentQueue<SerializedModuleResult>();
        var secondDequeued = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseFirst = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var stop = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var coordinator = new Mock<IDistributedWorkerCoordinator>();
        var dequeueCount = 0;
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                assignments.TryDequeue(out var assignment);
                if (Interlocked.Increment(ref dequeueCount) == 2)
                {
                    secondDequeued.TrySetResult();
                }

                return Task.FromResult(assignment);
            });
        coordinator.Setup(x => x.WaitForCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(token => Task.Delay(Timeout.InfiniteTimeSpan, token));
        coordinator.Setup(x => x.PublishResultAsync(It.IsAny<SerializedModuleResult>(), It.IsAny<CancellationToken>()))
            .Returns<SerializedModuleResult, CancellationToken>((result, token) =>
            {
                token.ThrowIfCancellationRequested();
                published.Enqueue(result);
                return Task.CompletedTask;
            });
        var runner = new Mock<IModuleRunner>();
        var executionCount = 0;
        runner.Setup(x => x.ExecuteWithoutDependencyWaitAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>(async (_, token) =>
            {
                if (Interlocked.Increment(ref executionCount) == 1)
                {
                    await releaseFirst.Task.WaitAsync(cancellationToken);
                }

                if (cancelled)
                {
                    token.ThrowIfCancellationRequested();
                }

                throw new InvalidOperationException("Worker execution failed.");
            });
        var serializer = new ModuleResultSerializer(typeRegistry);
        var registry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        var executor = new WorkerModuleExecutor(
            pipeline.Services.GetRequiredService<IHostApplicationLifetime>(), coordinator.Object,
            modules, typeRegistry, serializer, runner.Object, registry,
            pipeline.Services.GetRequiredService<IModuleDependencyRegistry>(),
            pipeline.Services.GetRequiredService<IModuleMetadataRegistry>(),
            MsOptions.Create(new DistributedOptions { InstanceIndex = 1, AutoDetectOsCapability = false, MaxParallelism = 1 }),
            pipeline.Services.GetRequiredService<IParallelLimitProvider>(),
            pipeline.Services.GetRequiredService<IServiceScopeFactory>(), null,
            NullLogger<WorkerModuleExecutor>.Instance);
        var run = executor.ExecuteAsync(modules, new Dictionary<Type, TimeSpan>(),
            new ExecutionBackendContext(registry), stop.Token);
        try
        {
            await secondDequeued.Task.WaitAsync(cancellationToken);
            if (cancelled)
            {
                await stop.CancelAsync();
            }
        }
        finally
        {
            releaseFirst.TrySetResult();
        }

        await run.WaitAsync(cancellationToken);
        await Assert.That(published.Select(result => result.ModuleId.Value).Order())
            .IsEquivalentTo(modules.Select(module => module.GetType().FullName!).Order());
        foreach (var result in published)
        {
            var failure = serializer.Deserialize(result)!;
            await Assert.That(failure.ExceptionOrDefault).IsNotNull();
            await Assert.That(failure.Status).IsEqualTo(cancelled ? ModuleStatus.Cancelled : ModuleStatus.Failed);
        }
    }

    private static async Task AssertWorkerRetriesAsync<TModule>(
        Action<PipelineBuilder>? configureBuilder,
        CancellationToken cancellationToken,
        bool rejectFirstPublication = false)
        where TModule : RetryingModule
    {
        var (module, result) = await ExecuteWorkerModuleAsync<TModule, int>(
            configureBuilder, cancellationToken, rejectFirstPublication);

        using (Assert.Multiple())
        {
            await Assert.That(module.AttemptCount).IsEqualTo(3);
            await Assert.That(module.AmbientModuleType).IsEqualTo(typeof(TModule));
            await Assert.That(module.AmbientLogger).IsNotNull();
            await Assert.That(result?.ExceptionOrDefault).IsNull();
            await Assert.That(result?.ValueOrDefault).IsEqualTo(3);
        }
    }

    private static async Task<(TModule Module, IModuleResult? Result)> ExecuteWorkerModuleAsync<TModule, TResult>(
        Action<PipelineBuilder>? configureBuilder,
        CancellationToken cancellationToken,
        bool rejectFirstPublication = false,
        string? schemaVersion = null)
        where TModule : Module<TResult>
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

            [],
            DateTimeOffset.UtcNow,
            new ModuleAssignmentOptions(null, false))
        {
            PipelineSchemaVersion = schemaVersion ?? typeRegistry.GetPipelineSchemaVersion(),
        };
        await coordinator.EnqueueModuleAsync(assignment, cancellationToken);
        var executor = new WorkerModuleExecutor(
            pipeline.Services.GetRequiredService<IHostApplicationLifetime>(),
            rejectFirstPublication ? new RejectFirstPublicationCoordinator(coordinator) : coordinator,
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
        try
        {
            var serializedResult = await coordinator.WaitForResultAsync(
                typeof(TModule).FullName!,
                cancellationToken).WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            await Assert.That(serializedResult.ExecutionTelemetry).IsNotNull();
            await Assert.That(serializedResult.ExecutionTelemetry!.ClaimedAt).IsNotEqualTo(default(DateTimeOffset));
            return (module, serializer.Deserialize(serializedResult));
        }
        finally
        {
            await coordinator.SignalCompletionAsync(CancellationToken.None);
            await executionTask.WaitAsync(TimeSpan.FromSeconds(5), CancellationToken.None);
        }
    }

    private static ModuleAssignment CreateAssignment(IModule module, ModuleTypeRegistry registry) => new(
        module.GetType().FullName!,
        [],
        DateTimeOffset.UtcNow,
        new ModuleAssignmentOptions(null, false))
    {
        PipelineSchemaVersion = registry.GetPipelineSchemaVersion(),
    };

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
