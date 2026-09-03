using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedModuleExecutorTests
{
    // Real, empty registries so the master's pre-distribution dependency validation is a no-op
    // for these dependency-free test modules (matches the production DI singletons).
    private static ModuleDependencyRegistry NewDependencyRegistry() => new();

    private static ModuleMetadataRegistry NewMetadataRegistry() =>
        new(new ModuleAttributeEventService());

    private static ModuleResultRegistrar NewResultRegistrar(IModuleResultRegistry resultRegistry) =>
        new ModuleResultRegistrar(resultRegistry, NullLogger<ModuleResultRegistrar>.Instance);

    private static AlwaysRunHandler NewAlwaysRunHandler(IModuleRunner moduleRunner)
    {
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(2);
        return new AlwaysRunHandler(
            moduleRunner,
            parallelLimitProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLogger<AlwaysRunHandler>.Instance,
            TimeProvider.System);
    }

    // --- Test module types ---

    private class SimpleResult
    {
        public string Message { get; set; } = string.Empty;
    }

    private class DistributedModule : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<SimpleResult>(new SimpleResult { Message = "done" });
    }

    private class ShortTimeoutDistributedModule : Module<SimpleResult>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
                .WithTimeout(TimeSpan.FromMilliseconds(50));

        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<SimpleResult>(new SimpleResult { Message = "done" });
    }

    [RunIf<OnLinux>]
    private class LinuxOnlyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("linux done");
    }

    [RunIf<OnUnix>]
    private class UnixModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("unix done");
    }

    [GroupedOperatingSystem<OnLinux>]
    [GroupedOperatingSystem<OnWindows>]
    private sealed class GroupedOperatingSystemModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult("grouped OS done");
    }

    [GroupedOperatingSystem<OnLinux>]
    [GroupedNonPlatformCondition]
    private sealed class MixedGroupedOperatingSystemModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult("mixed grouped OS done");
    }

    [GroupedOperatingSystem<OnLinux>]
    [GroupedWorkerCondition]
    private sealed class MixedWorkerGroupedOperatingSystemModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult("mixed worker grouped OS done");
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class GroupedOperatingSystemAttribute<TCondition> : RunIfAnyAttribute,
        IGroupedConditionAttribute
        where TCondition : IRunCondition, new()
    {
        public Type ConditionGroupType => typeof(GroupedOperatingSystemAttribute<>);

        public override string ConditionNames => typeof(TCondition).Name;

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            new TCondition().EvaluateAsync(context);
    }

    private sealed class GroupedNonPlatformConditionAttribute : RunIfAnyAttribute,
        IGroupedConditionAttribute,
        IPlanningConditionAttribute
    {
        public Type ConditionGroupType => typeof(GroupedOperatingSystemAttribute<>);

        public override string ConditionNames => nameof(GroupedNonPlatformConditionAttribute);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            Task.FromResult(false);
    }

    private sealed class GroupedWorkerConditionAttribute : RunIfAnyAttribute,
        IGroupedConditionAttribute
    {
        public Type ConditionGroupType => typeof(GroupedOperatingSystemAttribute<>);

        public override string ConditionNames => nameof(GroupedWorkerConditionAttribute);

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            Task.FromResult(true);
    }

    private class AnotherDistributedModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    private sealed class AlwaysRunDistributedModule : Module<int>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(42);
    }

    private sealed class ShortTimeoutAlwaysRunDistributedModule : Module<int>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun()
            .WithTimeout(TimeSpan.FromMilliseconds(100));

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(42);
    }

    [ProducesArtifact("distributed-output", "missing-output.txt")]
    private class ArtifactLoggingModule : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(new SimpleResult { Message = "done" });
    }

    [ConsumesArtifact(typeof(ArtifactLoggingModule), "distributed-output")]
    private class ArtifactDownloadModule : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(new SimpleResult { Message = "done" });
    }

    [ModularPipelines.DependsOn<DistributedModule>]
    private class DependsOnDistributedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("dependent done");
    }

    /// <summary>
    /// Wraps an <see cref="IDistributedMasterCoordinator"/> so that <see cref="DequeueModuleAsync"/>
    /// returns null immediately by default. Tests can instead gate delegation until their
    /// simulated external worker has claimed its assignment.
    /// </summary>
    private class ResultTrackingCoordinator(
        IDistributedMasterCoordinator inner,
        bool dequeueAfterRelease = false) : IDistributedMasterCoordinator
    {
        private readonly ConcurrentDictionary<string, TaskCompletionSource> _resultWaits = new();
        private readonly ConcurrentDictionary<string, TaskCompletionSource> _publishedResults = new();
        private readonly TaskCompletionSource _assignmentDequeued =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _dequeueCount;

        public TaskCompletionSource ResultWaitStarted { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ConcurrentDictionary<string, CancellationToken> ResultWaitTokens { get; } = new();

        public TaskCompletionSource WorkerQueryStarted { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource CompletionSignaled { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public int DequeueCount => Volatile.Read(ref _dequeueCount);

        private TaskCompletionSource WorkerQueryRelease { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void ReleaseWorkerQuery() => WorkerQueryRelease.TrySetResult();

        public Task WaitForAssignmentDequeuedAsync() => _assignmentDequeued.Task;

        public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
            => inner.EnqueueModuleAsync(assignment, cancellationToken);

        public async Task<ModuleAssignment?> DequeueModuleAsync(
            IReadOnlySet<Capability> workerCapabilities,
            CancellationToken cancellationToken)
        {
            if (!dequeueAfterRelease)
            {
                return null;
            }

            await WorkerQueryRelease.Task.WaitAsync(cancellationToken);
            var assignment = await inner.DequeueModuleAsync(workerCapabilities, cancellationToken);
            if (assignment is not null)
            {
                Interlocked.Increment(ref _dequeueCount);
                _assignmentDequeued.TrySetResult();
            }

            return assignment;
        }

        public async Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
        {
            await inner.PublishResultAsync(result, cancellationToken);
            _publishedResults.GetOrAdd(
                result.ModuleTypeName,
                _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)).TrySetResult();
        }

        public Task WaitForResultPublishedAsync(Type moduleType) =>
            _publishedResults.GetOrAdd(
                moduleType.FullName!,
                _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)).Task;

        public Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
        {
            ResultWaitStarted.TrySetResult();
            ResultWaitTokens[moduleTypeName] = cancellationToken;
            _resultWaits.GetOrAdd(
                moduleTypeName,
                _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)).TrySetResult();
            return inner.WaitForResultAsync(moduleTypeName, cancellationToken);
        }

        public Task WaitForResultStartedAsync(Type moduleType) =>
            _resultWaits.GetOrAdd(
                moduleType.FullName!,
                _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously)).Task;

        public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
            => inner.RegisterWorkerAsync(registration, cancellationToken);

        public async Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(
            CancellationToken cancellationToken)
        {
            WorkerQueryStarted.TrySetResult();
            await WorkerQueryRelease.Task.WaitAsync(cancellationToken);
            return await inner.GetRegisteredWorkersAsync(cancellationToken);
        }

        public Task<IReadOnlyList<WorkerStatus>> GetWorkerStatusesAsync(
            CancellationToken cancellationToken) =>
            inner.GetWorkerStatusesAsync(cancellationToken);

        public async Task SignalCompletionAsync(CancellationToken cancellationToken)
        {
            await inner.SignalCompletionAsync(cancellationToken);
            CompletionSignaled.TrySetResult();
        }

        public Task BroadcastCancellationAsync(CancellationToken cancellationToken)
            => inner.BroadcastCancellationAsync(cancellationToken);

        public Task SendHeartbeatAsync(WorkerStatus status, CancellationToken cancellationToken)
            => inner.SendHeartbeatAsync(status, cancellationToken);

        public Task WaitForCancellationAsync(CancellationToken cancellationToken)
            => inner.WaitForCancellationAsync(cancellationToken);
    }

    // --- Helpers ---

    private static ModuleResult<T> CreateSuccessResult<T>(
        T value,
        string moduleName,
        ModuleStatus status = ModuleStatus.Succeeded)
        where T : notnull
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<T>.Success(value)
        {
            Name = moduleName,
            TypeName = moduleName,
            Duration = TimeSpan.FromMilliseconds(100),
            StartTime = now,
            EndTime = now.AddMilliseconds(100),
            Status = status,
        };
    }

    /// <summary>
    /// Creates a properly-typed failure result that can be serialized as ModuleResult&lt;T&gt;.
    /// </summary>
    private static IModuleResult CreateTypedFailureResult<TModule>(TModule module, Exception exception) where TModule : IModule
    {
        var moduleType = typeof(TModule);
        var ctx = new ModuleExecutionContext(module, moduleType);
        return ModuleResultFactory.CreateException(module.ResultType, exception, ctx);
    }

    /// <summary>
    /// Creates a mock IModuleScheduler that yields the given module states, then completes.
    /// </summary>
    private static Mock<IModuleScheduler> CreateMockScheduler(params ModuleState[] modulesToYield)
    {
        var scheduler = new Mock<IModuleScheduler>();
        var channel = Channel.CreateUnbounded<ModuleState>();

        foreach (var ms in modulesToYield)
        {
            channel.Writer.TryWrite(ms);
        }
        channel.Writer.Complete();

        scheduler.Setup(s => s.ReadyModules).Returns(channel.Reader);
        scheduler.Setup(s => s.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(ct =>
            {
                var tcs = new TaskCompletionSource();
                ct.Register(() => tcs.TrySetCanceled(ct));
                return tcs.Task;
            });
        scheduler.Setup(s => s.MarkModuleStarted(It.IsAny<Type>())).Returns(true);
        scheduler.Setup(s => s.CancelPendingModules()).Returns([]);

        return scheduler;
    }

    private static DistributedModuleExecutor CreateExecutor(
        Mock<IModuleScheduler> scheduler,
        Mock<IModuleRunner>? moduleRunner = null,
        IModuleResultRegistry? resultRegistry = null,
        IDistributedMasterCoordinator? coordinator = null,
        DistributedResultCollector? resultCollector = null,
        ArtifactLifecycleManager? artifactManager = null,
        DistributedOptions? distributedOptions = null,
        IInternalModuleLogger? moduleLogger = null,
        ILogger<DistributedModuleExecutor>? executorLogger = null,
        IAlwaysRunHandler? alwaysRunHandler = null,
        CancellationToken applicationStopping = default,
        IModuleConditionHandler? conditionHandler = null)
    {
        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(l => l.ApplicationStopping).Returns(applicationStopping);

        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(f => f.Create()).Returns(scheduler.Object);

        var regEventExecutor = new Mock<IRegistrationEventExecutor>();
        regEventExecutor.Setup(r => r.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);

        coordinator ??= new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(typeRegistry);
        resultRegistry ??= new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry,
            conditionHandler: conditionHandler);
        resultCollector ??= new DistributedResultCollector(coordinator, serializer);
        moduleRunner ??= new Mock<IModuleRunner>();
        alwaysRunHandler ??= Mock.Of<IAlwaysRunHandler>();

        return new DistributedModuleExecutor(
            lifetime.Object,
            factory.Object,
            moduleRunner.Object,
            alwaysRunHandler,
            regEventExecutor.Object,
            coordinator,
            coordinator,
            publisher,
            resultCollector,
            typeRegistry,
            serializer,
            resultRegistry,
            NewResultRegistrar(resultRegistry),
            NewDependencyRegistry(),
            NewMetadataRegistry(),
            Microsoft.Extensions.Options.Options.Create(distributedOptions ?? new DistributedOptions()),
            NewModuleLoggerScopeFactory(moduleLogger),
            artifactManager,
            executorLogger ?? NullLogger<DistributedModuleExecutor>.Instance);
    }

    private static IServiceScopeFactory NewModuleLoggerScopeFactory(IInternalModuleLogger? moduleLogger = null)
    {
        moduleLogger ??= Mock.Of<IInternalModuleLogger>();
        var loggerProvider = new Mock<IInternalModuleLoggerAccessor>();
        loggerProvider.Setup(provider => provider.GetLogger(It.IsAny<Type>())).Returns(moduleLogger);
        var services = new ServiceCollection();
        services.AddScoped(_ => loggerProvider.Object);
        return services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    // =================================================================
    // Result Registration Tests
    // =================================================================

    [Test]
    public async Task Distributed_Module_Success_Registers_Result_In_Registry()
    {
        // Arrange
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Simulate worker publishing a result
        var successResult = CreateSuccessResult(new SimpleResult { Message = "done" }, "DistributedModule");
        var serialized = serializer.Serialize(
            successResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        // Act
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        // Assert
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(registeredResult.Name).IsEqualTo("DistributedModule");
    }

    [Test]
    public async Task Distributed_Module_Failure_Registers_Failure_Result_In_Registry()
    {
        // Arrange
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Create a properly typed failure result.
        var failureResult = CreateTypedFailureResult(module, new InvalidOperationException("Worker error"));
        var serialized = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        // Act
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        // Assert
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.ExceptionOrDefault).IsNotNull();
    }

    [Test]
    public async Task History_Restored_Module_Is_Precompleted_Before_Distribution()
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler();
        scheduler.Setup(instance => instance.GetModuleState(typeof(DistributedModule)))
            .Returns(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var historyResult = CreateSuccessResult(
            new SimpleResult { Message = "history" },
            "DistributedModule",
            ModuleStatus.RestoredFromHistory);
        resultRegistry.RegisterResult(typeof(DistributedModule), historyResult);

        var executor = CreateExecutor(scheduler, resultRegistry: resultRegistry);

        await executor.ExecuteAsync([module]);

        await Assert.That(moduleState.Result).IsSameReferenceAs(historyResult);
        scheduler.Verify(instance => instance.MarkModuleCompleted(
            typeof(DistributedModule),
            true,
            null,
            ModuleStatus.RestoredFromHistory), Times.Once());
    }

    [Test]
    public async Task Cancelled_Distributed_Module_Registers_Failure_Result()
    {
        // Arrange: coordinator throws OperationCanceledException on WaitForResult
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException("Cancelled by test"));
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator.Object,
            resultCollector: resultCollector);

        // Act
        await executor.ExecuteAsync([module]);

        // Assert — cancellation should register a failure result
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.ExceptionOrDefault).IsNotNull();
    }

    [Test]
    public async Task Collection_Exception_Registers_Failure_Result_And_Cancels_Pipeline()
    {
        // Arrange: coordinator throws a non-cancellation exception
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Deserialization failed"));
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator.Object,
            resultCollector: resultCollector);

        // Act
        await executor.ExecuteAsync([module]);

        // Assert
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.ExceptionOrDefault).IsNotNull();
    }

    [Test]
    [Timeout(5_000)]
    public async Task Missing_Result_Times_Out_And_Completes_Pipeline(CancellationToken testCancellation)
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new ResultTrackingCoordinator(new InMemoryDistributedCoordinator());
        var options = new DistributedOptions { ModuleResultTimeout = TimeSpan.FromSeconds(1) };
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator,
            distributedOptions: options);

        await executor.ExecuteAsync([module]).WaitAsync(TimeSpan.FromSeconds(3), testCancellation);

        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.ExceptionOrDefault).IsTypeOf<TimeoutException>();
        scheduler.Verify(
            instance => instance.MarkModuleCompleted(typeof(DistributedModule), false, null, null),
            Times.Once());
    }

    [Test]
    [Timeout(5_000)]
    public async Task Module_Timeout_Takes_Precedence_Over_Default_Result_Timeout(CancellationToken testCancellation)
    {
        var module = new ShortTimeoutDistributedModule();
        var moduleState = new ModuleState(module, typeof(ShortTimeoutDistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new ResultTrackingCoordinator(new InMemoryDistributedCoordinator());
        var options = new DistributedOptions { ModuleResultTimeout = TimeSpan.FromSeconds(30) };
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator,
            distributedOptions: options);

        await executor.ExecuteAsync([module]).WaitAsync(TimeSpan.FromSeconds(2), testCancellation);

        var registeredResult = resultRegistry.GetResult(typeof(ShortTimeoutDistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.ExceptionOrDefault).IsTypeOf<TimeoutException>();
    }

    [Test]
    [Timeout(5_000)]
    public async Task Zero_Result_Timeout_Waits_Until_Result_Is_Published(CancellationToken testCancellation)
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var innerCoordinator = new InMemoryDistributedCoordinator();
        var coordinator = new ResultTrackingCoordinator(innerCoordinator);
        var options = new DistributedOptions { ModuleResultTimeout = TimeSpan.Zero };
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator,
            distributedOptions: options);

        var execution = executor.ExecuteAsync([module]);

        await Assert.That(async () => await execution.WaitAsync(TimeSpan.FromMilliseconds(100), testCancellation))
            .Throws<TimeoutException>();

        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var successResult = CreateSuccessResult(new SimpleResult { Message = "done" }, "DistributedModule");
        var serialized = serializer.Serialize(
            successResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            1);

        await innerCoordinator.PublishResultAsync(serialized, testCancellation);
        await execution.WaitAsync(TimeSpan.FromSeconds(2), testCancellation);

        await Assert.That(resultRegistry.GetResult(typeof(DistributedModule))).IsNotNull();
    }

    // =================================================================
    // Fail-Fast Cascade Tests
    // =================================================================

    [Test]
    public async Task Failed_Module_Cancels_Pipeline_For_Remaining_Modules()
    {
        // Arrange: two modules — first fails, second should be cancelled
        var moduleA = new DistributedModule();
        var moduleB = new AnotherDistributedModule();
        var stateA = new ModuleState(moduleA, typeof(DistributedModule));
        var stateB = new ModuleState(moduleB, typeof(AnotherDistributedModule));

        var scheduler = CreateMockScheduler(stateA, stateB);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(AnotherDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Simulate: module A gets a failure result, module B gets nothing
        var failureResult = CreateTypedFailureResult(moduleA, new Exception("A failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        // Act
        var executionTask = executor.ExecuteAsync([moduleA, moduleB]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
        // Don't publish moduleB result — it should be cancelled
        await executionTask;

        // Assert — module A has failure, module B also gets a failure (cancelled)
        var resultA = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(resultA).IsNotNull();
        await Assert.That(resultA!.ExceptionOrDefault).IsNotNull();

        var resultB = resultRegistry.GetResult(typeof(AnotherDistributedModule));
        await Assert.That(resultB).IsNotNull();
        await Assert.That(resultB!.ExceptionOrDefault).IsNotNull();
    }

    [Test]
    public async Task FailFast_Does_Not_Cancel_InFlight_AlwaysRun_Result()
    {
        var failedModule = new DistributedModule();
        var alwaysRunModule = new AlwaysRunDistributedModule();
        var scheduler = CreateMockScheduler(
            new ModuleState(failedModule, typeof(DistributedModule)),
            new ModuleState(alwaysRunModule, typeof(AlwaysRunDistributedModule)));
        var pipelineCancelled = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        scheduler
            .Setup(x => x.CancelPendingModules())
            .Callback(() => pipelineCancelled.TrySetResult())
            .Returns([]);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(AlwaysRunDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var failureResult = CreateTypedFailureResult(failedModule, new Exception("failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        var alwaysRunResult = CreateSuccessResult(42, nameof(AlwaysRunDistributedModule));
        var serializedAlwaysRunResult = serializer.Serialize(
            alwaysRunResult,
            typeof(AlwaysRunDistributedModule).FullName!,
            typeof(int).FullName!,
            workerIndex: 1);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var releaseAlwaysRun = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var alwaysRunStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler
            .Setup(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.Is<IReadOnlyList<IModule>>(modules => modules.Contains(alwaysRunModule)),
                It.IsAny<Func<ModuleState, Task>>()))
            .Callback(() => alwaysRunStarted.TrySetResult())
            .Returns(releaseAlwaysRun.Task);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);
        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            alwaysRunHandler: alwaysRunHandler.Object,
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        var executionTask = executor.ExecuteAsync([failedModule, alwaysRunModule]);
        await noDequeue.WaitForResultStartedAsync(typeof(DistributedModule)).WaitAsync(TimeSpan.FromSeconds(2));
        await noDequeue.WaitForResultStartedAsync(typeof(AlwaysRunDistributedModule)).WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
        await pipelineCancelled.Task.WaitAsync(TimeSpan.FromSeconds(2));

        var alwaysRunWaitToken = noDequeue.ResultWaitTokens[typeof(AlwaysRunDistributedModule).FullName!];
        await Assert.That(alwaysRunWaitToken.IsCancellationRequested).IsFalse();
        await Assert.That(executionTask.IsCompleted).IsFalse();

        await coordinator.PublishResultAsync(serializedAlwaysRunResult, CancellationToken.None);
        await alwaysRunStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await Assert.That(executionTask.IsCompleted).IsFalse();
        await Assert.That(noDequeue.CompletionSignaled.Task.IsCompleted).IsFalse();

        releaseAlwaysRun.TrySetResult();
        await executionTask.WaitAsync(TimeSpan.FromSeconds(2));
        await noDequeue.CompletionSignaled.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await Assert.That(resultRegistry.GetResult(typeof(AlwaysRunDistributedModule))?.ExceptionOrDefault).IsNull();
        alwaysRunHandler.VerifyAll();
    }

    [Test]
    public async Task FailFast_Classifies_Subsequent_AlwaysRun_Timeout()
    {
        var failedModule = new DistributedModule();
        var alwaysRunModule = new ShortTimeoutAlwaysRunDistributedModule();
        var scheduler = CreateMockScheduler(
            new ModuleState(failedModule, typeof(DistributedModule)),
            new ModuleState(alwaysRunModule, typeof(ShortTimeoutAlwaysRunDistributedModule)));
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(ShortTimeoutAlwaysRunDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var failureResult = CreateTypedFailureResult(failedModule, new Exception("failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            alwaysRunHandler: Mock.Of<IAlwaysRunHandler>(),
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: new DistributedResultCollector(noDequeue, serializer));

        var executionTask = executor.ExecuteAsync([failedModule, alwaysRunModule]);
        await noDequeue.WaitForResultStartedAsync(typeof(DistributedModule)).WaitAsync(TimeSpan.FromSeconds(2));
        await noDequeue.WaitForResultStartedAsync(typeof(ShortTimeoutAlwaysRunDistributedModule))
            .WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
        await executionTask.WaitAsync(TimeSpan.FromSeconds(2));

        var result = resultRegistry.GetResult(typeof(ShortTimeoutAlwaysRunDistributedModule));
        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Status).IsEqualTo(ModuleStatus.TimedOut);
        await Assert.That(result.ExceptionOrDefault).IsTypeOf<TimeoutException>();
    }

    [Test]
    public async Task FailFast_LateStarts_AlwaysRun_Through_Distributed_Path()
    {
        var failedModule = new DistributedModule();
        var failedState = new ModuleState(failedModule, typeof(DistributedModule));
        var alwaysRunModule = new AlwaysRunDistributedModule();
        var alwaysRunState = new ModuleState(alwaysRunModule, typeof(AlwaysRunDistributedModule));
        var coordinator = new InMemoryDistributedCoordinator();
        var trackingCoordinator = new ResultTrackingCoordinator(coordinator, dequeueAfterRelease: true);
        var scheduler = CreateMockScheduler(failedState);
        scheduler.Setup(x => x.GetModuleState(typeof(AlwaysRunDistributedModule))).Returns(alwaysRunState);
        scheduler.Setup(x => x.GetModuleCompletionTask(typeof(AlwaysRunDistributedModule)))
            .Returns(alwaysRunState.CompletionSource.Task);
        scheduler.Setup(x => x.MarkModuleStarted(typeof(AlwaysRunDistributedModule)))
            .Callback(() => alwaysRunState.State = ModuleExecutionState.Executing)
            .Returns(true);
        scheduler.Setup(x => x.CancelPendingModules())
            .Callback(trackingCoordinator.ReleaseWorkerQuery)
            .Returns([]);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(AlwaysRunDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var failureResult = CreateTypedFailureResult(failedModule, new Exception("failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        var moduleRunner = new Mock<IModuleRunner>();
        moduleRunner.Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                It.Is<ModuleState>(state => ReferenceEquals(state.Module, alwaysRunModule)),
                It.IsAny<IModuleScheduler>(),
                It.IsAny<CancellationToken>()))
            .Callback(() =>
            {
                ModuleCompletionSourceApplicator.TryApply(
                    alwaysRunModule,
                    CreateSuccessResult(42, nameof(AlwaysRunDistributedModule)));
                alwaysRunState.State = ModuleExecutionState.Completed;
                alwaysRunState.CompletionSource.TrySetResult(alwaysRunModule);
            })
            .Returns(Task.CompletedTask);
        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            resultRegistry,
            trackingCoordinator,
            new DistributedResultCollector(trackingCoordinator, serializer),
            alwaysRunHandler: NewAlwaysRunHandler(moduleRunner.Object));

        var executionTask = executor.ExecuteAsync([failedModule, alwaysRunModule]);
        await trackingCoordinator.WaitForResultStartedAsync(typeof(DistributedModule))
            .WaitAsync(TimeSpan.FromSeconds(2));
        var failedAssignment = await coordinator.DequeueModuleAsync(
            new HashSet<Capability>(),
            CancellationToken.None);
        await Assert.That(failedAssignment?.ModuleTypeName).IsEqualTo(typeof(DistributedModule).FullName);
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);

        await trackingCoordinator.WaitForResultPublishedAsync(typeof(AlwaysRunDistributedModule))
            .WaitAsync(TimeSpan.FromSeconds(2));
        await executionTask.WaitAsync(TimeSpan.FromSeconds(2));
        await Assert.That(resultRegistry.GetResult(typeof(AlwaysRunDistributedModule))?.Status)
            .IsEqualTo(ModuleStatus.Succeeded);
        moduleRunner.VerifyAll();
    }

    [Test]
    public async Task AlwaysRun_Handler_Failure_Is_Rethrown_After_Worker_Shutdown()
    {
        var failure = new InvalidOperationException("AlwaysRun failed");
        var module = new DistributedModule();
        var scheduler = CreateMockScheduler(new ModuleState(module, typeof(DistributedModule)));
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);
        var coordinator = new InMemoryDistributedCoordinator();
        var trackingCoordinator = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var failureResult = CreateTypedFailureResult(module, new Exception("pipeline failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler.Setup(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<Func<ModuleState, Task>>()))
            .ThrowsAsync(failure);
        var executor = CreateExecutor(
            scheduler,
            coordinator: trackingCoordinator,
            resultCollector: new DistributedResultCollector(trackingCoordinator, serializer),
            alwaysRunHandler: alwaysRunHandler.Object);

        var executionTask = executor.ExecuteAsync([module]);
        await trackingCoordinator.WaitForResultStartedAsync(typeof(DistributedModule))
            .WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
        var exception = await Assert.That(async () => await executionTask)
            .Throws<InvalidOperationException>();

        await Assert.That(exception).IsSameReferenceAs(failure);
        await trackingCoordinator.CompletionSignaled.Task.WaitAsync(TimeSpan.FromSeconds(2));
        alwaysRunHandler.VerifyAll();
    }

    [Test]
    public async Task FailFast_Skips_Queued_NonAlwaysRun_Master_Assignments()
    {
        var failedModule = new DistributedModule();
        var queuedModule = new AnotherDistributedModule();
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator, dequeueAfterRelease: true);
        var scheduler = CreateMockScheduler(
            new ModuleState(failedModule, typeof(DistributedModule)),
            new ModuleState(queuedModule, typeof(AnotherDistributedModule)));
        var pipelineCancelled = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        scheduler
            .Setup(x => x.CancelPendingModules())
            .Callback(() =>
            {
                noDequeue.ReleaseWorkerQuery();
                pipelineCancelled.TrySetResult();
            })
            .Returns([]);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(AnotherDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var failureResult = CreateTypedFailureResult(failedModule, new Exception("failed"));
        var serializedFailure = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);
        var moduleRunner = new Mock<IModuleRunner>();
        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler
            .Setup(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<Func<ModuleState, Task>>()))
            .Returns(noDequeue.WaitForAssignmentDequeuedAsync());
        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            alwaysRunHandler: alwaysRunHandler.Object,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        var executionTask = executor.ExecuteAsync([failedModule, queuedModule]);
        await noDequeue.WaitForResultStartedAsync(typeof(DistributedModule)).WaitAsync(TimeSpan.FromSeconds(2));
        await noDequeue.WaitForResultStartedAsync(typeof(AnotherDistributedModule)).WaitAsync(TimeSpan.FromSeconds(2));
        var externalWorkerAssignment = await coordinator.DequeueModuleAsync(
            new HashSet<Capability>(),
            CancellationToken.None);
        await Assert.That(externalWorkerAssignment?.ModuleTypeName)
            .IsEqualTo(typeof(DistributedModule).FullName);
        await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
        await pipelineCancelled.Task.WaitAsync(TimeSpan.FromSeconds(2));

        await executionTask.WaitAsync(TimeSpan.FromSeconds(2));
        await Assert.That(noDequeue.DequeueCount).IsEqualTo(1);
        moduleRunner.Verify(runner => runner.ExecuteWithoutDependencyWaitAsync(
            It.IsAny<ModuleState>(),
            It.IsAny<IModuleScheduler>(),
            It.IsAny<CancellationToken>()), Times.Never);
        alwaysRunHandler.VerifyAll();
    }

    // =================================================================
    // Master-as-Worker Tests
    // =================================================================

    [Test]
    public async Task Master_Worker_Loop_Executes_Module_And_Publishes_Result()
    {
        // Arrange: the master dequeues a module from the work queue, executes it, and publishes the result
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator, serializer);
        var moduleRunner = new Mock<IModuleRunner>();

        // Track what scheduler was passed to ExecuteWithoutDependencyWaitAsync
        IModuleScheduler? capturedScheduler = null;
        var assignmentExecutionScopeWasActive = false;
        moduleRunner.Setup(r => r.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(), It.IsAny<IModuleScheduler>(), It.IsAny<CancellationToken>()))
            .Callback<ModuleState, IModuleScheduler, CancellationToken>((_, sched, _) =>
            {
                capturedScheduler = sched;
                assignmentExecutionScopeWasActive = DistributedAssignmentExecutionScope.IsActive;
                // Simulate successful execution by setting the module's CompletionSource
                var result = CreateSuccessResult(new SimpleResult { Message = "master-executed" }, "DistributedModule");
                ModuleCompletionSourceApplicator.TryApply(module, result);
            })
            .Returns(Task.CompletedTask);

        var executor = CreateExecutor(scheduler,
            moduleRunner: moduleRunner,
            resultRegistry: resultRegistry,
            coordinator: coordinator,
            resultCollector: resultCollector);

        // Act
        await executor.ExecuteAsync([module]);

        // Assert — the master worker loop used a WorkerModuleScheduler (no-op)
        await Assert.That(capturedScheduler).IsNotNull();
        await Assert.That(capturedScheduler).IsTypeOf<WorkerModuleScheduler>();
        await Assert.That(assignmentExecutionScopeWasActive).IsTrue();

        // The result was published through the coordinator and collected by the result collector
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Master_Worker_ArtifactLogging_UsesModuleScope()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"distributed-artifact-logging-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);
        var module = new ArtifactLoggingModule();
        var moduleState = new ModuleState(module, typeof(ArtifactLoggingModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(ArtifactLoggingModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator, serializer);
        var moduleRunner = new Mock<IModuleRunner>();
        var fallbackLogger = new Mock<ILogger<ArtifactLifecycleManager>>();
        var moduleLogger = new Mock<IInternalModuleLogger>();
        moduleLogger.Setup(logger => logger.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        IModuleLogger? ambientLogger = null;

        moduleRunner.Setup(runner => runner.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<IModuleScheduler>(),
                It.IsAny<CancellationToken>()))
            .Callback<ModuleState, IModuleScheduler, CancellationToken>((_, _, _) =>
            {
                ambientLogger = ModuleLogger.Values.Value;
                var result = CreateSuccessResult(
                    new SimpleResult { Message = "master-executed" },
                    nameof(ArtifactLoggingModule));
                ModuleCompletionSourceApplicator.TryApply(module, result);
            })
            .Returns(Task.CompletedTask);
        var artifactManager = new ArtifactLifecycleManager(
            Mock.Of<IDistributedArtifactStore>(),
            Microsoft.Extensions.Options.Options.Create(new ArtifactOptions()),
            fallbackLogger.Object,
            workingDirectory);
        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            resultRegistry,
            coordinator,
            resultCollector,
            artifactManager,
            moduleLogger: moduleLogger.Object);

        try
        {
            await executor.ExecuteAsync([module]);

            await Assert.That(ambientLogger).IsSameReferenceAs(moduleLogger.Object);
            moduleLogger.Verify(
                logger => logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("No files matched pattern")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            fallbackLogger.VerifyNoOtherCalls();
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Master_Worker_ArtifactDownloadFailure_MarksModuleLoggerFailed()
    {
        var failure = new InvalidOperationException("download failed");
        var module = new ArtifactDownloadModule();
        var scheduler = CreateMockScheduler(new ModuleState(module, typeof(ArtifactDownloadModule)));
        var moduleRunner = new Mock<IModuleRunner>();
        var moduleLogger = new Mock<IInternalModuleLogger>();
        moduleLogger.Setup(logger => logger.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.ListArtifactsAsync(
                typeof(ArtifactLoggingModule).FullName!,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var artifactManager = new ArtifactLifecycleManager(
            store.Object,
            Microsoft.Extensions.Options.Options.Create(new ArtifactOptions()),
            NullLogger<ArtifactLifecycleManager>.Instance);
        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            artifactManager: artifactManager,
            moduleLogger: moduleLogger.Object);

        await executor.ExecuteAsync([module]);

        moduleLogger.Verify(logger => logger.SetException(failure), Times.Once);
        moduleRunner.Verify(runner => runner.ExecuteWithoutDependencyWaitAsync(
            It.IsAny<ModuleState>(),
            It.IsAny<IModuleScheduler>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Master_Worker_ArtifactUploadFailure_UsesModuleLogger()
    {
        var workingDirectory = Path.Combine(Path.GetTempPath(), $"distributed-artifact-upload-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);
        await File.WriteAllTextAsync(Path.Combine(workingDirectory, "missing-output.txt"), "output");
        var module = new ArtifactLoggingModule();
        var scheduler = CreateMockScheduler(new ModuleState(module, typeof(ArtifactLoggingModule)));
        var moduleRunner = new Mock<IModuleRunner>();
        var moduleLogger = new Mock<IInternalModuleLogger>();
        moduleLogger.Setup(logger => logger.IsEnabled(It.IsAny<LogLevel>())).Returns(true);
        var executorLogger = new Mock<ILogger<DistributedModuleExecutor>>();
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.UploadAsync(
                It.IsAny<ArtifactDescriptor>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("upload failed"));
        moduleRunner.Setup(runner => runner.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<IModuleScheduler>(),
                It.IsAny<CancellationToken>()))
            .Callback<ModuleState, IModuleScheduler, CancellationToken>((_, _, _) =>
            {
                var result = CreateSuccessResult(
                    new SimpleResult { Message = "master-executed" },
                    nameof(ArtifactLoggingModule));
                ModuleCompletionSourceApplicator.TryApply(module, result);
            })
            .Returns(Task.CompletedTask);
        var artifactManager = new ArtifactLifecycleManager(
            store.Object,
            Microsoft.Extensions.Options.Options.Create(new ArtifactOptions()),
            NullLogger<ArtifactLifecycleManager>.Instance,
            workingDirectory);
        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            artifactManager: artifactManager,
            moduleLogger: moduleLogger.Object,
            executorLogger: executorLogger.Object);

        try
        {
            await executor.ExecuteAsync([module]);

            moduleLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Failed to upload artifacts for")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
            executorLogger.Verify(
                logger => logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Failed to upload artifacts for")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never);
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Master_Worker_Populates_Required_Dependency_Metadata()
    {
        var dependency = new DistributedModule();
        var dependent = new DependsOnDistributedModule();
        var scheduler = CreateMockScheduler(
            new ModuleState(dependent, typeof(DependsOnDistributedModule)));
        var resultRegistry = new ModuleResultRegistry();
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        typeRegistry.Register(typeof(DependsOnDistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator, serializer);
        var moduleRunner = new Mock<IModuleRunner>();
        ModuleState? capturedState = null;

        moduleRunner.Setup(runner => runner.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<IModuleScheduler>(),
                It.IsAny<CancellationToken>()))
            .Callback<ModuleState, IModuleScheduler, CancellationToken>((state, _, _) =>
            {
                capturedState = state;
                var result = CreateSuccessResult("dependent done", "DependsOnDistributedModule");
                ModuleCompletionSourceApplicator.TryApply(dependent, result);
            })
            .Returns(Task.CompletedTask);

        var executor = CreateExecutor(
            scheduler,
            moduleRunner,
            resultRegistry,
            coordinator,
            resultCollector);

        await executor.ExecuteAsync([dependency, dependent]);

        await Assert.That(capturedState).IsNotNull();
        await Assert.That(capturedState!.Dependencies[typeof(DistributedModule)]).IsFalse();
    }

    [Test]
    public async Task CreateAssignment_Auto_Detects_Linux_Capability_From_RunIfAll()
    {
        // Arrange
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(LinuxOnlyModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var module = new LinuxOnlyModule();

        // Act
        var assignment = publisher.CreateAssignment(module);

        // Assert — "linux" capability auto-detected from [RunIf<OnLinux>]
        await Assert.That(assignment.RequiredCapabilities).Contains("linux");
    }

    [Test]
    public async Task CreateAssignment_Routes_Unix_Group_To_Linux_Or_MacOS_Workers()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(UnixModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new UnixModule());
        var requiredCapability = assignment.RequiredCapabilities.Single();

        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows))
                .DoesNotContain(requiredCapability);
        }
    }

    [Test]
    public async Task CreateAssignment_Unions_Grouped_Operating_System_Alternatives()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(GroupedOperatingSystemModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new GroupedOperatingSystemModule());
        var requiredCapability = assignment.RequiredCapabilities.Single();

        using (Assert.Multiple())
        {
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Linux))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.Windows))
                .Contains(requiredCapability);
            await Assert.That(OperatingSystemConditions.GetWorkerCapabilities(OperatingSystemConditions.MacOS))
                .DoesNotContain(requiredCapability);
        }
    }

    [Test]
    public async Task CreateAssignment_Leaves_Planning_Safe_Mixed_Group_Unrestricted()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MixedGroupedOperatingSystemModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new MixedGroupedOperatingSystemModule());

        await Assert.That(assignment.RequiredCapabilities)
            .DoesNotContain(OperatingSystemConditions.Linux);
    }

    [Test]
    public async Task CreateAssignment_Leaves_Worker_Only_Group_Unrestricted()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MixedWorkerGroupedOperatingSystemModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);

        var assignment = publisher.CreateAssignment(new MixedWorkerGroupedOperatingSystemModule());

        await Assert.That(assignment.RequiredCapabilities)
            .DoesNotContain(OperatingSystemConditions.Linux);
    }

    [Test]
    public async Task CreateAssignment_Omits_Os_Routing_When_Local_Alternative_Matched()
    {
        var coordinator = new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(MixedGroupedOperatingSystemModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultRegistry = new ModuleResultRegistry();
        var conditionRouting = new DistributedConditionRouting();
        var module = new MixedGroupedOperatingSystemModule();
        var conditionHandler = new Mock<IModuleConditionHandler>();
        conditionHandler
            .Setup(handler => handler.PrepareDistributedRoutingAsync(
                module,
                It.IsAny<CancellationToken>()))
            .Callback(() => conditionRouting.MarkLocallySatisfied(
                module,
                typeof(GroupedOperatingSystemAttribute<>)))
            .Returns(Task.CompletedTask);
        var publisher = new DistributedWorkPublisher(
            coordinator,
            typeRegistry,
            serializer,
            resultRegistry,
            conditionRouting: conditionRouting,
            conditionHandler: conditionHandler.Object);

        var assignment = await publisher.CreateAssignmentAsync(
            module,
            CancellationToken.None);

        await Assert.That(assignment.RequiredCapabilities)
            .DoesNotContain(OperatingSystemConditions.Linux);
        conditionHandler.Verify(handler => handler.PrepareDistributedRoutingAsync(
            module,
            CancellationToken.None));
    }

    // =================================================================
    // Completion Signal Tests
    // =================================================================

    [Test]
    public async Task Executor_Signals_Completion_To_Workers_After_Success()
    {
        // Arrange
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());

        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var executor = CreateExecutor(scheduler,
            coordinator: coordinator.Object,
            resultCollector: resultCollector);

        // Act
        await executor.ExecuteAsync([module]);

        // Assert
        coordinator.Verify(c => c.SignalCompletionAsync(CancellationToken.None), Times.Once());
    }

    [Test]
    public async Task Executor_Signals_Completion_Even_When_Execution_Fails()
    {
        // Arrange
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Boom"));
        coordinator.Setup(c => c.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var executor = CreateExecutor(scheduler,
            coordinator: coordinator.Object,
            resultCollector: resultCollector);

        // Act
        await executor.ExecuteAsync([module]);

        // Assert — always signals completion, even on failure
        coordinator.Verify(c => c.BroadcastCancellationAsync(CancellationToken.None), Times.Once());
        coordinator.Verify(c => c.SignalCompletionAsync(CancellationToken.None), Times.Once());
    }

    [Test]
    public async Task Executor_Broadcasts_Cancellation_When_Assignment_Creation_Fails()
    {
        var failure = new InvalidOperationException("Routing failed");
        var conditionHandler = new Mock<IModuleConditionHandler>();
        conditionHandler.Setup(handler => handler.PrepareDistributedRoutingAsync(
                It.IsAny<IModule>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(instance => instance.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var module = new DistributedModule();
        var executor = CreateExecutor(
            CreateMockScheduler(new ModuleState(module, typeof(DistributedModule))),
            coordinator: coordinator.Object,
            conditionHandler: conditionHandler.Object);

        var exception = await Assert.That(async () => await executor.ExecuteAsync([module]))
            .Throws<InvalidOperationException>();

        await Assert.That(exception).IsSameReferenceAs(failure);
        coordinator.Verify(
            instance => instance.BroadcastCancellationAsync(CancellationToken.None),
            Times.Once());
        coordinator.Verify(
            instance => instance.SignalCompletionAsync(CancellationToken.None),
            Times.Once());
    }

    [Test]
    public async Task Executor_Signals_Shutdown_When_Worker_Wait_Fails()
    {
        var failure = new InvalidOperationException("Worker registration failed");
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(instance => instance.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(failure);
        coordinator.Setup(instance => instance.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(instance => instance.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var module = new DistributedModule();
        var executor = CreateExecutor(
            CreateMockScheduler(new ModuleState(module, typeof(DistributedModule))),
            coordinator: coordinator.Object,
            distributedOptions: new DistributedOptions { TotalInstances = 2 });

        var exception = await Assert.That(async () => await executor.ExecuteAsync([module]))
            .Throws<InvalidOperationException>();

        await Assert.That(exception).IsSameReferenceAs(failure);
        coordinator.Verify(
            instance => instance.BroadcastCancellationAsync(CancellationToken.None),
            Times.Once());
        coordinator.Verify(
            instance => instance.SignalCompletionAsync(CancellationToken.None),
            Times.Once());
    }

    [Test]
    public async Task Executor_Broadcasts_Cancellation_When_Application_Is_Stopping()
    {
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        using var stopping = new CancellationTokenSource();
        stopping.Cancel();
        var module = new DistributedModule();
        var scheduler = CreateMockScheduler(new ModuleState(module, typeof(DistributedModule)));
        var executor = CreateExecutor(
            scheduler,
            coordinator: coordinator.Object,
            applicationStopping: stopping.Token);

        await executor.ExecuteAsync([module]);

        coordinator.Verify(
            c => c.BroadcastCancellationAsync(CancellationToken.None),
            Times.Once());
        coordinator.Verify(
            c => c.SignalCompletionAsync(CancellationToken.None),
            Times.Once());
    }

    // =================================================================
    // Empty Module List
    // =================================================================

    [Test]
    public async Task Empty_Module_List_Returns_Immediately()
    {
        var scheduler = CreateMockScheduler();
        var executor = CreateExecutor(scheduler);

        var result = await executor.ExecuteAsync([]);

        await Assert.That(result.Count()).IsEqualTo(0);
    }

    // =================================================================
    // Scheduler Interaction Tests
    // =================================================================

    [Test]
    public async Task Distributed_Module_Marks_Scheduler_Started_And_Completed()
    {
        // Arrange
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Simulate worker result
        var successResult = CreateSuccessResult(new SimpleResult { Message = "ok" }, "DistributedModule");
        var serialized = serializer.Serialize(successResult, typeof(DistributedModule).FullName!, typeof(SimpleResult).FullName!, 1);
        // Act
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        // Assert
        scheduler.Verify(s => s.MarkModuleStarted(typeof(DistributedModule)), Times.Once());
        scheduler.Verify(s => s.MarkModuleCompleted(typeof(DistributedModule), true, null, null), Times.Once());
    }

    [Test]
    public async Task Rejected_Start_Does_Not_Publish_Or_Collect_Result()
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        scheduler.Setup(s => s.MarkModuleStarted(typeof(DistributedModule))).Returns(false);

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var executor = CreateExecutor(scheduler, coordinator: coordinator.Object);

        await executor.ExecuteAsync([module]);

        coordinator.Verify(
            c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()),
            Times.Never());
        coordinator.Verify(
            c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never());
        scheduler.Verify(
            s => s.MarkModuleCompleted(
                typeof(DistributedModule),
                It.IsAny<bool>(),
                It.IsAny<Exception?>(),
                It.IsAny<ModuleStatus?>()),
            Times.Never());
    }

    [Test]
    public async Task Deferred_Start_Publishes_Only_After_Scheduler_Requeues_Module()
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState, moduleState);
        scheduler.SetupSequence(s => s.MarkModuleStarted(typeof(DistributedModule)))
            .Returns(false)
            .Returns(true);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);
        var executor = CreateExecutor(
            scheduler,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        var successResult = CreateSuccessResult(new SimpleResult { Message = "ok" }, "DistributedModule");
        var serialized = serializer.Serialize(
            successResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            1);
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        scheduler.Verify(s => s.MarkModuleStarted(typeof(DistributedModule)), Times.Exactly(2));
        scheduler.Verify(
            s => s.MarkModuleCompleted(typeof(DistributedModule), true, null, null),
            Times.Once());
    }

    [Test]
    [Timeout(5_000)]
    public async Task Cancellation_After_Start_Claim_Completes_Module_Result(
        CancellationToken testCancellation)
    {
        using var stoppingCts = new CancellationTokenSource();
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        scheduler.Setup(s => s.CancelPendingModules())
            .Returns([]);
        scheduler.Setup(s => s.MarkModuleStarted(typeof(DistributedModule)))
            .Returns(() =>
            {
                stoppingCts.Cancel();
                return true;
            });

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(
                It.IsAny<ModuleAssignment>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleAssignment, CancellationToken>((_, token) => Task.FromCanceled(token));
        coordinator.Setup(c => c.WaitForResultAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .Returns<string, CancellationToken>((_, token) =>
                Task.FromCanceled<SerializedModuleResult>(token));
        coordinator.Setup(c => c.DequeueModuleAsync(
                It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleAssignment?) null);
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator.Object,
            applicationStopping: stoppingCts.Token);

        await executor.ExecuteAsync([module])
            .WaitAsync(TimeSpan.FromSeconds(3), testCancellation);
        var moduleResult = await ((IInternalModule) module).ResultTask
            .WaitAsync(TimeSpan.FromSeconds(1), testCancellation);

        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.ExceptionOrDefault)
            .IsTypeOf<OperationCanceledException>();
        await Assert.That(resultRegistry.GetResult(typeof(DistributedModule)))
            .IsSameReferenceAs(moduleResult);
        scheduler.Verify(
            s => s.MarkModuleCompleted(typeof(DistributedModule), false, null, null),
            Times.Once());
    }

    [Test]
    [Timeout(5_000)]
    public async Task Publish_Failure_Completes_Module_Result(
        CancellationToken testCancellation)
    {
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        scheduler.Setup(s => s.CancelPendingModules())
            .Returns([]);

        var publishException = new InvalidOperationException("Broker unavailable");
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(
                It.IsAny<ModuleAssignment>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(publishException);
        coordinator.Setup(c => c.DequeueModuleAsync(
                It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleAssignment?) null);
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator.Object);

        await executor.ExecuteAsync([module])
            .WaitAsync(TimeSpan.FromSeconds(3), testCancellation);
        var moduleResult = await ((IInternalModule) module).ResultTask
            .WaitAsync(TimeSpan.FromSeconds(1), testCancellation);

        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.ExceptionOrDefault)
            .IsSameReferenceAs(publishException);
        await Assert.That(resultRegistry.GetResult(typeof(DistributedModule)))
            .IsSameReferenceAs(moduleResult);
        coordinator.Verify(
            c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never());
        scheduler.Verify(
            s => s.MarkModuleCompleted(typeof(DistributedModule), false, publishException, null),
            Times.Once());
    }

    [Test]
    [Timeout(5_000)]
    public async Task Publish_Timeout_Completes_Module_Result(
        CancellationToken testCancellation)
    {
        var module = new ShortTimeoutDistributedModule();
        var moduleState = new ModuleState(module, typeof(ShortTimeoutDistributedModule));
        var scheduler = CreateMockScheduler(moduleState);

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(
                It.IsAny<ModuleAssignment>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleAssignment, CancellationToken>(
                (_, token) => Task.Delay(Timeout.InfiniteTimeSpan, token));
        coordinator.Setup(c => c.DequeueModuleAsync(
                It.IsAny<IReadOnlySet<Capability>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleAssignment?) null);
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.BroadcastCancellationAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var resultRegistry = new ModuleResultRegistry();
        var executor = CreateExecutor(
            scheduler,
            resultRegistry: resultRegistry,
            coordinator: coordinator.Object);

        await executor.ExecuteAsync([module])
            .WaitAsync(TimeSpan.FromSeconds(3), testCancellation);
        var moduleResult = await ((IInternalModule) module).ResultTask
            .WaitAsync(TimeSpan.FromSeconds(1), testCancellation);

        await Assert.That(moduleResult.ExceptionOrDefault)
            .IsTypeOf<TimeoutException>();
        await Assert.That(resultRegistry.GetResult(typeof(ShortTimeoutDistributedModule)))
            .IsSameReferenceAs(moduleResult);
        coordinator.Verify(
            c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never());
        scheduler.Verify(
            s => s.MarkModuleCompleted(typeof(ShortTimeoutDistributedModule), false, null, null),
            Times.Once());
        coordinator.Verify(
            c => c.BroadcastCancellationAsync(CancellationToken.None),
            Times.Once());
    }

    [Test]
    public async Task Distributed_Module_Failure_Marks_Scheduler_With_Success_False()
    {
        // Arrange
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Simulate worker failure (properly-typed so serializer accepts it)
        var failureResult = CreateTypedFailureResult(module, new Exception("Failed"));
        var serialized = serializer.Serialize(failureResult, typeof(DistributedModule).FullName!, typeof(SimpleResult).FullName!, 1);
        // Act
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        // Assert
        scheduler.Verify(s => s.MarkModuleCompleted(typeof(DistributedModule), false, null, null), Times.Once());
    }

    // =================================================================
    // Module Type Registration
    // =================================================================

    [Test]
    public async Task Executor_Registers_All_Module_Types_In_TypeRegistry()
    {
        // Arrange
        var moduleA = new DistributedModule();
        var moduleB = new AnotherDistributedModule();
        var stateA = new ModuleState(moduleA, typeof(DistributedModule));
        var stateB = new ModuleState(moduleB, typeof(AnotherDistributedModule));

        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var scheduler = CreateMockScheduler(stateA, stateB);
        var typeRegistry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(l => l.ApplicationStopping).Returns(CancellationToken.None);
        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(f => f.Create()).Returns(scheduler.Object);
        var regEventExecutor = new Mock<IRegistrationEventExecutor>();
        regEventExecutor.Setup(r => r.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator.Object, typeRegistry, serializer, resultRegistry);
        var moduleRunner = new Mock<IModuleRunner>();

        var executor = new DistributedModuleExecutor(
            lifetime.Object, factory.Object, moduleRunner.Object, Mock.Of<IAlwaysRunHandler>(), regEventExecutor.Object,
            coordinator.Object, coordinator.Object, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, NewResultRegistrar(resultRegistry), NewDependencyRegistry(), NewMetadataRegistry(),
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            NewModuleLoggerScopeFactory(),
            null, NullLogger<DistributedModuleExecutor>.Instance);

        // Act
        await executor.ExecuteAsync([moduleA, moduleB]);

        // Assert — both types are registered and resolvable
        var resolvedA = typeRegistry.Resolve(typeof(DistributedModule).FullName!);
        var resolvedB = typeRegistry.Resolve(typeof(AnotherDistributedModule).FullName!);
        await Assert.That(resolvedA).IsNotNull();
        await Assert.That(resolvedB).IsNotNull();
    }

    // =================================================================
    // Worker Readiness Barrier Tests
    // =================================================================

    [Test]
    public async Task Executor_Waits_For_Workers_Before_Distributing_Work()
    {
        // Arrange: configure 2 total instances (1 master + 1 worker)
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new ResultTrackingCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);
        var resultRegistry = new ModuleResultRegistry();

        var distributedOptions = new DistributedOptions { TotalInstances = 2, CapabilityTimeout = TimeSpan.FromSeconds(10) };

        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(l => l.ApplicationStopping).Returns(CancellationToken.None);
        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(f => f.Create()).Returns(scheduler.Object);
        var regEventExecutor = new Mock<IRegistrationEventExecutor>();
        regEventExecutor.Setup(r => r.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);
        var moduleRunner = new Mock<IModuleRunner>();
        var publisher = new DistributedWorkPublisher(noDequeue, typeRegistry, serializer, resultRegistry);

        var executor = new DistributedModuleExecutor(
            lifetime.Object, factory.Object, moduleRunner.Object, Mock.Of<IAlwaysRunHandler>(), regEventExecutor.Object,
            noDequeue, noDequeue, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, NewResultRegistrar(resultRegistry), NewDependencyRegistry(), NewMetadataRegistry(),
            Microsoft.Extensions.Options.Options.Create(distributedOptions),
            NewModuleLoggerScopeFactory(),
            null, NullLogger<DistributedModuleExecutor>.Instance);

        // Act
        var executionTask = executor.ExecuteAsync([module]);
        await noDequeue.WorkerQueryStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        await coordinator.RegisterWorkerAsync(
            new WorkerRegistration(1, [], DateTimeOffset.UtcNow),
            CancellationToken.None);
        noDequeue.ReleaseWorkerQuery();
        await noDequeue.ResultWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        var successResult = CreateSuccessResult(new SimpleResult { Message = "ok" }, "DistributedModule");
        var serialized = serializer.Serialize(
            successResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            1);
        await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        await executionTask;

        // Assert — work was distributed and result collected (if barrier didn't work, result would be lost)
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Executor_Skips_Worker_Wait_When_TotalInstances_Is_One()
    {
        // Arrange: TotalInstances = 1 means no workers expected
        var scheduler = CreateMockScheduler(); // no modules
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var distributedOptions = new DistributedOptions { TotalInstances = 1 };

        var executor = CreateExecutor(scheduler, coordinator: coordinator.Object);

        // Act — should return quickly without calling GetRegisteredWorkersAsync
        await executor.ExecuteAsync([]);

        // Assert — GetRegisteredWorkersAsync should never be called
        coordinator.Verify(c => c.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    [Timeout(15_000)]
    public async Task Executor_Proceeds_After_Worker_Registration_Timeout(CancellationToken testCancellation)
    {
        // Arrange: expect 3 workers but only 1 registers — should timeout and proceed
        var distributedOptions = new DistributedOptions { TotalInstances = 4, CapabilityTimeout = TimeSpan.FromSeconds(3) };

        // Use mock coordinator to track GetRegisteredWorkersAsync calls and timing
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        var registeredWorkers = new List<WorkerRegistration>
        {
            new(1, [], DateTimeOffset.UtcNow),
        };
        coordinator.Setup(c => c.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => registeredWorkers.AsReadOnly());
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new OperationCanceledException());
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);

        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(coordinator.Object, serializer);

        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(l => l.ApplicationStopping).Returns(CancellationToken.None);
        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(f => f.Create()).Returns(scheduler.Object);
        var regEventExecutor = new Mock<IRegistrationEventExecutor>();
        regEventExecutor.Setup(r => r.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);
        var moduleRunner = new Mock<IModuleRunner>();
        var resultRegistry = new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator.Object, typeRegistry, serializer, resultRegistry);

        var executor = new DistributedModuleExecutor(
            lifetime.Object, factory.Object, moduleRunner.Object, Mock.Of<IAlwaysRunHandler>(), regEventExecutor.Object,
            coordinator.Object, coordinator.Object, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, NewResultRegistrar(resultRegistry), NewDependencyRegistry(), NewMetadataRegistry(),
            Microsoft.Extensions.Options.Options.Create(distributedOptions),
            NewModuleLoggerScopeFactory(),
            null, NullLogger<DistributedModuleExecutor>.Instance);

        // Act — should proceed after 3 seconds timeout even though only 1/3 workers registered
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await executor.ExecuteAsync([module]);
        sw.Stop();

        // Assert — waited roughly 3 seconds (the timeout), not the full test timeout
        await Assert.That(sw.Elapsed.TotalSeconds).IsGreaterThanOrEqualTo(2.5);
        await Assert.That(sw.Elapsed.TotalSeconds).IsLessThan(10);

        // Verify GetRegisteredWorkersAsync was polled multiple times
        coordinator.Verify(c => c.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()), Times.AtLeast(2));
    }
}
