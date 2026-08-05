using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel]
public class TaskCompletionSourceTests
{
    private sealed class TestModule : Module<int>
    {
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(1);
    }

    [Test]
    public async Task ModuleState_CompletionSource_RunsContinuationsAsynchronously()
    {
        var state = new ModuleState(Mock.Of<IModule>(), typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(state.CompletionSource.Task)).IsTrue();
    }

    [Test]
    public async Task ModuleAndExecutionContext_CompletionSources_RunContinuationsAsynchronously()
    {
        var module = new TestModule();
        var context = new ModuleExecutionContext<int>(module, module.GetType());

        using (Assert.Multiple())
        {
            await Assert.That(RunsContinuationsAsynchronously(module.CompletionSource.Task))
                .IsTrue();
            await Assert.That(RunsContinuationsAsynchronously(context.ExecutionTask))
                .IsTrue();
            await Assert.That(RunsContinuationsAsynchronously(context.TypedExecutionTask))
                .IsTrue();
        }
    }

    [Test]
    public async Task ModuleResultRegistry_CompletionSource_RunsContinuationsAsynchronously()
    {
        var registry = new ModuleResultRegistry();
        registry.RegisterModule(typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(registry.GetCompletionTask(typeof(IModule))!)).IsTrue();
    }

    [Test]
    public async Task ModuleState_Failure_DoesNotRaiseUnobservedTaskException()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateFaultedModuleCompletion(expectedException));
    }

    [Test]
    public async Task SubModuleTracker_Failure_DoesNotRaiseUnobservedTaskException()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateFaultedSubModuleCompletion(expectedException));
    }

    [Test]
    public async Task TimeoutHelper_FastCancellation_ObservesAbandonedFault()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateFastCancellationFaultAsync(expectedException));
    }

    [Test]
    public async Task TimeoutHelper_ExternalCancellation_ObservesAbandonedFault()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateExternalCancellationFaultAsync(expectedException));
    }

    [Test]
    public async Task TimeoutHelper_GraceExpiry_ObservesAbandonedFault()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateGraceExpiryFaultAsync(expectedException));
    }

    [Test]
    public async Task ModuleExecutor_WorkerFault_ObservesAbandonedSchedulerFault()
    {
        var expectedException = new InvalidOperationException(Guid.NewGuid().ToString());

        await AssertNoUnobservedTaskException(
            expectedException,
            () => CreateAbandonedSchedulerFaultAsync(expectedException));
    }

    private static bool RunsContinuationsAsynchronously(Task task) =>
        task.CreationOptions.HasFlag(TaskCreationOptions.RunContinuationsAsynchronously);

    private static Task AssertNoUnobservedTaskException(
        Exception expectedException,
        Func<WeakReference> createFaultedOwner) =>
        AssertNoUnobservedTaskException(
            expectedException,
            () => Task.FromResult(createFaultedOwner()));

    private static async Task AssertNoUnobservedTaskException(
        Exception expectedException,
        Func<Task<WeakReference>> createFaultedOwner)
    {
        var exceptionObservedByFinalizer = 0;

        void OnUnobservedTaskException(object? _, UnobservedTaskExceptionEventArgs args)
        {
            if (ContainsException(args.Exception, expectedException))
            {
                Interlocked.Exchange(ref exceptionObservedByFinalizer, 1);
                args.SetObserved();
            }
        }

        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        try
        {
            var ownerReference = await createFaultedOwner();

            for (var attempt = 0; attempt < 10 && ownerReference.IsAlive; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                await Task.Yield();
            }

            await Assert.That(ownerReference.IsAlive).IsFalse();
            await Assert.That(Volatile.Read(ref exceptionObservedByFinalizer)).IsEqualTo(0);
        }
        finally
        {
            TaskScheduler.UnobservedTaskException -= OnUnobservedTaskException;
        }
    }

    private static bool ContainsException(Exception candidate, Exception expected)
    {
        if (ReferenceEquals(candidate, expected))
        {
            return true;
        }

        return candidate switch
        {
            AggregateException aggregate => aggregate.InnerExceptions.Any(x => ContainsException(x, expected)),
            { InnerException: not null } => ContainsException(candidate.InnerException, expected),
            _ => false,
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateFaultedModuleCompletion(Exception exception)
    {
        var module = Mock.Of<IModule>();
        var state = new ModuleState(module, module.GetType());
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var tracker = new ModuleStateTracker(
            NullLogger.Instance,
            TimeProvider.System,
            Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            moduleStates,
            [],
            [],
            new ModuleStateQueries(moduleStates),
            new ReaderWriterLockSlim(),
            new SemaphoreSlim(0),
            () => false);

        tracker.MarkModuleCompleted(state.ModuleType, success: false, exception);
        return new WeakReference(state.CompletionSource.Task);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateFaultedSubModuleCompletion(Exception exception)
    {
        var tracker = new SubModuleTracker("test", typeof(IModule));
        var executionTask = tracker.ExecuteAsync<int>(() => Task.FromException<int>(exception));

        try
        {
            executionTask.GetAwaiter().GetResult();
        }
        catch (InvalidOperationException caught) when (ReferenceEquals(caught, exception))
        {
            // The caller-observable failure was handled.
        }

        return new WeakReference(tracker);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> CreateFastCancellationFaultAsync(Exception exception)
    {
        var abandonedTaskSource = new TaskCompletionSource<int>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationTokenSource = new CancellationTokenSource();
        var invocation = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => abandonedTaskSource.Task,
            timeout: null,
            cancellationTokenSource.Token);

        cancellationTokenSource.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await invocation);
        abandonedTaskSource.SetException(exception);

        return new WeakReference(abandonedTaskSource.Task);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> CreateExternalCancellationFaultAsync(Exception exception)
    {
        var abandonedTaskSource = new TaskCompletionSource<int>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellationTokenSource = new CancellationTokenSource();
        var invocation = TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => abandonedTaskSource.Task,
            TimeSpan.FromMinutes(1),
            cancellationTokenSource.Token);

        cancellationTokenSource.Cancel();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await invocation);
        abandonedTaskSource.SetException(exception);

        return new WeakReference(abandonedTaskSource.Task);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> CreateGraceExpiryFaultAsync(Exception exception)
    {
        var abandonedTaskSource = new TaskCompletionSource<int>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var result = await TimeoutHelper.ExecuteWithTimeoutAndDetailsAsync(
            _ => abandonedTaskSource.Task,
            TimeSpan.FromMilliseconds(1),
            CancellationToken.None);

        await Assert.That(result.TimedOut).IsTrue();
        await Assert.That(result.WasCancellationTokenRespected).IsFalse();
        abandonedTaskSource.SetException(exception);

        return new WeakReference(abandonedTaskSource.Task);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> CreateAbandonedSchedulerFaultAsync(Exception exception)
    {
        var workerException = new DependencyCollisionException("Worker fault");
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        readyModules.Writer.Complete(workerException);
        var schedulerTaskSource = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(schedulerTaskSource.Task);
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);
        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);
        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler
            .Setup(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);
        var registrationEventExecutor = new Mock<IRegistrationEventExecutor>();
        registrationEventExecutor
            .Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);
        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            Mock.Of<IModuleRunner>(),
            alwaysRunHandler.Object,
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEventExecutor.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<ISecondaryExceptionContainer>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLogger<ModuleExecutor>.Instance);

        await Assert.ThrowsAsync<DependencyCollisionException>(
            async () => await executor.ExecuteAsync([new TestModule()]));
        schedulerTaskSource.SetException(exception);

        return new WeakReference(schedulerTaskSource.Task);
    }
}
