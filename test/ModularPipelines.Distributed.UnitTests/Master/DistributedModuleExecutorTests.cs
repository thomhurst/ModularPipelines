using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class DistributedModuleExecutorTests
{
    // --- Test module types ---

    private class SimpleResult
    {
        public string Message { get; set; } = string.Empty;
    }

    private class DistributedModule : Module<SimpleResult>
    {
        protected internal override Task<SimpleResult?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<SimpleResult?>(new SimpleResult { Message = "done" });
    }

    [RunOnLinuxOnly]
    private class LinuxOnlyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("linux done");
    }

    private class AnotherDistributedModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            Context.IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult(42);
    }

    /// <summary>
    /// Wraps an <see cref="IDistributedCoordinator"/> so that <see cref="DequeueModuleAsync"/>
    /// returns null immediately. This prevents the master worker loop from competing with
    /// simulated external workers in tests that verify the distributed collection path.
    /// </summary>
    private class NoDequeueCoordinator(IDistributedCoordinator inner) : IDistributedCoordinator
    {
        public Task EnqueueModuleAsync(ModuleAssignment assignment, CancellationToken cancellationToken)
            => inner.EnqueueModuleAsync(assignment, cancellationToken);

        public Task<ModuleAssignment?> DequeueModuleAsync(IReadOnlySet<string> workerCapabilities, CancellationToken cancellationToken)
            => Task.FromResult<ModuleAssignment?>(null);

        public Task PublishResultAsync(SerializedModuleResult result, CancellationToken cancellationToken)
            => inner.PublishResultAsync(result, cancellationToken);

        public Task<SerializedModuleResult> WaitForResultAsync(string moduleTypeName, CancellationToken cancellationToken)
            => inner.WaitForResultAsync(moduleTypeName, cancellationToken);

        public Task RegisterWorkerAsync(WorkerRegistration registration, CancellationToken cancellationToken)
            => inner.RegisterWorkerAsync(registration, cancellationToken);

        public Task<IReadOnlyList<WorkerRegistration>> GetRegisteredWorkersAsync(CancellationToken cancellationToken)
            => inner.GetRegisteredWorkersAsync(cancellationToken);

        public Task SignalCompletionAsync(CancellationToken cancellationToken)
            => inner.SignalCompletionAsync(cancellationToken);
    }

    // --- Helpers ---

    private static ModuleResult<T> CreateSuccessResult<T>(T value, string moduleName) where T : notnull
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<T>.Success(value)
        {
            ModuleName = moduleName,
            ModuleTypeName = moduleName,
            ModuleDuration = TimeSpan.FromMilliseconds(100),
            ModuleStart = now,
            ModuleEnd = now.AddMilliseconds(100),
            ModuleStatus = Status.Successful,
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

        return scheduler;
    }

    private static DistributedModuleExecutor CreateExecutor(
        Mock<IModuleScheduler> scheduler,
        Mock<IModuleRunner>? moduleRunner = null,
        IModuleResultRegistry? resultRegistry = null,
        IDistributedCoordinator? coordinator = null,
        DistributedResultCollector? resultCollector = null,
        ArtifactLifecycleManager? artifactManager = null)
    {
        var lifetime = new Mock<IHostApplicationLifetime>();
        lifetime.Setup(l => l.ApplicationStopping).Returns(CancellationToken.None);

        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(f => f.Create()).Returns(scheduler.Object);

        var regEventExecutor = new Mock<IRegistrationEventExecutor>();
        regEventExecutor.Setup(r => r.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);

        coordinator ??= new InMemoryDistributedCoordinator();
        var typeRegistry = new ModuleTypeRegistry();
        var serializer = new ModuleResultSerializer(typeRegistry);
        resultRegistry ??= new ModuleResultRegistry();
        var publisher = new DistributedWorkPublisher(coordinator, typeRegistry, serializer, resultRegistry);
        resultCollector ??= new DistributedResultCollector(coordinator, serializer);
        moduleRunner ??= new Mock<IModuleRunner>();

        return new DistributedModuleExecutor(
            lifetime.Object,
            factory.Object,
            moduleRunner.Object,
            regEventExecutor.Object,
            coordinator,
            publisher,
            resultCollector,
            typeRegistry,
            serializer,
            resultRegistry,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            artifactManager,
            NullLogger<DistributedModuleExecutor>.Instance);
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
        var noDequeue = new NoDequeueCoordinator(coordinator);
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
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        });

        // Act
        await executor.ExecuteAsync([module]);

        // Assert
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.IsSuccess).IsTrue();
        await Assert.That(registeredResult.ModuleName).IsEqualTo("DistributedModule");
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
        var noDequeue = new NoDequeueCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);

        var executor = CreateExecutor(scheduler,
            resultRegistry: resultRegistry,
            coordinator: noDequeue,
            resultCollector: resultCollector);

        // Create a properly-typed failure result (FailureWrapper<SimpleResult>)
        var failureResult = CreateTypedFailureResult(module, new InvalidOperationException("Worker error"));
        var serialized = serializer.Serialize(
            failureResult,
            typeof(DistributedModule).FullName!,
            typeof(SimpleResult).FullName!,
            workerIndex: 1);
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        });

        // Act
        await executor.ExecuteAsync([module]);

        // Assert
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.IsFailure).IsTrue();
    }

    [Test]
    public async Task Cancelled_Distributed_Module_Registers_Failure_Result()
    {
        // Arrange: coordinator throws OperationCanceledException on WaitForResult
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();

        var coordinator = new Mock<IDistributedCoordinator>();
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
        await Assert.That(registeredResult!.IsFailure).IsTrue();
    }

    [Test]
    public async Task Collection_Exception_Registers_Failure_Result_And_Cancels_Pipeline()
    {
        // Arrange: coordinator throws a non-cancellation exception
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var resultRegistry = new ModuleResultRegistry();

        var coordinator = new Mock<IDistributedCoordinator>();
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
        await Assert.That(registeredResult!.IsFailure).IsTrue();
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
        var noDequeue = new NoDequeueCoordinator(coordinator);
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
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(serializedFailure, CancellationToken.None);
            // Don't publish moduleB result — it should be cancelled
        });

        // Act
        await executor.ExecuteAsync([moduleA, moduleB]);

        // Assert — module A has failure, module B also gets a failure (cancelled)
        var resultA = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(resultA).IsNotNull();
        await Assert.That(resultA!.IsFailure).IsTrue();

        var resultB = resultRegistry.GetResult(typeof(AnotherDistributedModule));
        await Assert.That(resultB).IsNotNull();
        await Assert.That(resultB!.IsFailure).IsTrue();
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
        moduleRunner.Setup(r => r.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(), It.IsAny<IModuleScheduler>(), It.IsAny<CancellationToken>()))
            .Callback<ModuleState, IModuleScheduler, CancellationToken>((_, sched, _) =>
            {
                capturedScheduler = sched;
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

        // The result was published through the coordinator and collected by the result collector
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.IsSuccess).IsTrue();
    }

    [Test]
    public async Task CreateAssignment_Auto_Detects_Linux_Capability_From_RunOnLinuxOnly()
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

        // Assert — "linux" capability auto-detected from [RunOnLinuxOnly]
        await Assert.That(assignment.RequiredCapabilities).Contains("linux");
    }

    // =================================================================
    // Completion Signal Tests
    // =================================================================

    [Test]
    public async Task Executor_Signals_Completion_To_Workers_After_Success()
    {
        // Arrange
        var coordinator = new Mock<IDistributedCoordinator>();
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
        var coordinator = new Mock<IDistributedCoordinator>();
        coordinator.Setup(c => c.SignalCompletionAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(c => c.WaitForResultAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Boom"));

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
        coordinator.Verify(c => c.SignalCompletionAsync(CancellationToken.None), Times.Once());
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
        var noDequeue = new NoDequeueCoordinator(coordinator);
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
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        });

        // Act
        await executor.ExecuteAsync([module]);

        // Assert
        scheduler.Verify(s => s.MarkModuleStarted(typeof(DistributedModule)), Times.Once());
        scheduler.Verify(s => s.MarkModuleCompleted(typeof(DistributedModule), true, null, null), Times.Once());
    }

    [Test]
    public async Task Distributed_Module_Failure_Marks_Scheduler_With_Success_False()
    {
        // Arrange
        var module = new DistributedModule();
        var moduleState = new ModuleState(module, typeof(DistributedModule));
        var scheduler = CreateMockScheduler(moduleState);
        var coordinator = new InMemoryDistributedCoordinator();
        var noDequeue = new NoDequeueCoordinator(coordinator);
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
        _ = Task.Run(async () =>
        {
            await Task.Delay(50);
            await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        });

        // Act
        await executor.ExecuteAsync([module]);

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

        var coordinator = new Mock<IDistributedCoordinator>();
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
            lifetime.Object, factory.Object, moduleRunner.Object, regEventExecutor.Object,
            coordinator.Object, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
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
        var noDequeue = new NoDequeueCoordinator(coordinator);
        var typeRegistry = new ModuleTypeRegistry();
        typeRegistry.Register(typeof(DistributedModule));
        var serializer = new ModuleResultSerializer(typeRegistry);
        var resultCollector = new DistributedResultCollector(noDequeue, serializer);
        var resultRegistry = new ModuleResultRegistry();

        var distributedOptions = new DistributedOptions { TotalInstances = 2, CapabilityTimeoutSeconds = 10 };

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
            lifetime.Object, factory.Object, moduleRunner.Object, regEventExecutor.Object,
            noDequeue, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, Microsoft.Extensions.Options.Options.Create(distributedOptions),
            null, NullLogger<DistributedModuleExecutor>.Instance);

        // Simulate a worker registering after a short delay
        _ = Task.Run(async () =>
        {
            await Task.Delay(200);
            await coordinator.RegisterWorkerAsync(
                new WorkerRegistration(1, new HashSet<string>(), DateTimeOffset.UtcNow),
                CancellationToken.None);
        });

        // Simulate the worker publishing a result slightly later
        _ = Task.Run(async () =>
        {
            await Task.Delay(500);
            var successResult = CreateSuccessResult(new SimpleResult { Message = "ok" }, "DistributedModule");
            var serialized = serializer.Serialize(successResult, typeof(DistributedModule).FullName!, typeof(SimpleResult).FullName!, 1);
            await coordinator.PublishResultAsync(serialized, CancellationToken.None);
        });

        // Act
        await executor.ExecuteAsync([module]);

        // Assert — work was distributed and result collected (if barrier didn't work, result would be lost)
        var registeredResult = resultRegistry.GetResult(typeof(DistributedModule));
        await Assert.That(registeredResult).IsNotNull();
        await Assert.That(registeredResult!.IsSuccess).IsTrue();
    }

    [Test]
    public async Task Executor_Skips_Worker_Wait_When_TotalInstances_Is_One()
    {
        // Arrange: TotalInstances = 1 means no workers expected
        var scheduler = CreateMockScheduler(); // no modules
        var coordinator = new Mock<IDistributedCoordinator>();
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
        var distributedOptions = new DistributedOptions { TotalInstances = 4, CapabilityTimeoutSeconds = 3 };

        // Use mock coordinator to track GetRegisteredWorkersAsync calls and timing
        var coordinator = new Mock<IDistributedCoordinator>();
        var registeredWorkers = new List<WorkerRegistration>
        {
            new(1, new HashSet<string>(), DateTimeOffset.UtcNow),
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
            lifetime.Object, factory.Object, moduleRunner.Object, regEventExecutor.Object,
            coordinator.Object, publisher, resultCollector, typeRegistry, serializer,
            resultRegistry, Microsoft.Extensions.Options.Options.Create(distributedOptions),
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
