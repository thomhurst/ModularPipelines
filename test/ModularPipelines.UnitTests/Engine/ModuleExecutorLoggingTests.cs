using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.UnitTests.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleExecutorLoggingTests
{
    private class FaultingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class LaterModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class QueuedAlwaysRunModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
                .WithAlwaysRun();

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class UnrelatedModule : LaterModule;

    [Test]
    [Timeout(30_000)]
    public async Task Deferred_Module_Releases_Worker_Slot_For_Unrelated_Work(CancellationToken cancellationToken)
    {
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var running = new FaultingModule();
        var deferred = new LaterModule();
        var unrelated = new UnrelatedModule();
        IModule[] modules = [running, deferred, unrelated];
        var states = modules.ToDictionary(module => module.GetType(), module => new ModuleState(module, module.GetType()));
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        foreach (var module in modules)
        {
            readyModules.Writer.TryWrite(states[module.GetType()]);
        }

        var runningStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var deferredAttempted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var unrelatedStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        scheduler.Setup(x => x.GetModuleState(It.IsAny<Type>()))
            .Returns((Type moduleType) => states[moduleType]);
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);
        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(x => x.Create()).Returns(scheduler.Object);
        var limits = new Mock<IParallelLimitProvider>();
        limits.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(2);
        var registry = new ModuleResultRegistry();
        var attempts = 0;
        var runner = new Mock<IModuleRunner>();
        runner.Setup(x => x.ExecuteAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>(async (state, token) =>
            {
                if (state.Module == running)
                {
                    runningStarted.TrySetResult();
                    await unrelatedStarted.Task.WaitAsync(token);
                    Complete(state);
                    // Model the scheduler re-queuing a deferred module when its constraint clears.
                    readyModules.Writer.TryWrite(states[deferred.GetType()]);
                    readyModules.Writer.TryComplete();
                }
                else if (state.Module == deferred && Interlocked.Increment(ref attempts) == 1)
                {
                    await runningStarted.Task.WaitAsync(token);
                    state.ExecutionDeferred = true;
                    deferredAttempted.TrySetResult();
                }
                else
                {
                    if (state.Module == unrelated)
                    {
                        await deferredAttempted.Task.WaitAsync(token);
                        unrelatedStarted.TrySetResult();
                    }

                    Complete(state);
                }
            });
        var executor = new ModuleExecutor(factory.Object, runner.Object, Mock.Of<IAlwaysRunHandler>(),
            new ModuleResultRegistrar(registry, NullLogger<ModuleResultRegistrar>.Instance), registry,
            limits.Object, Mock.Of<IRegistrationEventExecutor>(), Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(), new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            new SecondaryExceptionContainer(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLogger<ModuleExecutor>.Instance);

        var execution = executor.ExecuteAsync(modules, new Dictionary<Type, TimeSpan>(),
            new ExecutionBackendContext(registry), cancellation.Token);
        try
        {
            await deferredAttempted.Task.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            await unrelatedStarted.Task.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            var results = await execution.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken);
            await Assert.That(results).Count().IsEqualTo(3);
            await Assert.That(attempts).IsEqualTo(2);
        }
        finally
        {
            await cancellation.CancelAsync();
            readyModules.Writer.TryComplete();
            await ((Task) execution).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
        }

        void Complete(ModuleState state)
        {
            var module = (Module<bool>) state.Module;
            var result = ModuleResult<bool>.CreateSuccess(true,
                new ModuleExecutionContext(module, module.GetType()) { Status = ModuleStatus.Succeeded });
            module.CompletionSource.TrySetResult(result);
            registry.RegisterResult(module.GetType(), result);
            state.ExecutionDeferred = false;
            state.State = ModuleExecutionState.Completed;
        }
    }

    [Test]
    [Arguments(FailureMode.FailFast)]
    [Arguments(FailureMode.ContinueOnFailure)]
    public async Task Caller_Cancellation_Stops_Ordinary_Work_And_Drains_AlwaysRun(FailureMode failureMode)
    {
        using var cancellation = new CancellationTokenSource();
        var running = new FaultingModule();
        var queued = new LaterModule();
        var alwaysRun = new QueuedAlwaysRunModule();
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        foreach (var module in new IModule[] { running, queued, alwaysRun })
        {
            readyModules.Writer.TryWrite(new ModuleState(module, module.GetType()));
        }

        readyModules.Writer.Complete();
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        var states = new IModule[] { running, queued, alwaysRun }
            .ToDictionary(module => module.GetType(), module => new ModuleState(module, module.GetType()));
        scheduler.Setup(x => x.GetModuleState(It.IsAny<Type>()))
            .Returns((Type moduleType) => states[moduleType]);
        var factory = new Mock<IModuleSchedulerFactory>();
        factory.Setup(x => x.Create()).Returns(scheduler.Object);
        var limits = new Mock<IParallelLimitProvider>();
        limits.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);
        var runner = new Mock<IModuleRunner>();
        var alwaysRunCompleted = false;
        runner.Setup(x => x.ExecuteAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>(async (state, token) =>
            {
                if (state.Module == running)
                {
                    started.TrySetResult();
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                }
                else if (state.Module == alwaysRun)
                {
                    token.ThrowIfCancellationRequested();
                    alwaysRunCompleted = true;
                    alwaysRun.CompletionSource.TrySetResult(
                        ModuleResult<bool>.CreateSuccess(true, new ModuleExecutionContext(alwaysRun, alwaysRun.GetType())));
                }
            });
        var registry = new ModuleResultRegistry();
        var executor = new ModuleExecutor(factory.Object, runner.Object, Mock.Of<IAlwaysRunHandler>(),
            new ModuleResultRegistrar(registry, NullLogger<ModuleResultRegistrar>.Instance), registry,
            limits.Object, Mock.Of<IRegistrationEventExecutor>(), Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(), new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            new SecondaryExceptionContainer(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions { FailureMode = failureMode }),
            NullLogger<ModuleExecutor>.Instance);

        var execution = executor.ExecuteAsync([running, queued, alwaysRun], new Dictionary<Type, TimeSpan>(),
            new ExecutionBackendContext(registry), cancellation.Token);
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(10));
            await cancellation.CancelAsync();
            await execution.WaitAsync(TimeSpan.FromSeconds(10));
        }
        finally
        {
            await cancellation.CancelAsync();
        }

        await Assert.That(alwaysRunCompleted).IsTrue();
        await Assert.That(registry.GetResult(running.GetType())!.Status).IsEqualTo(ModuleStatus.Cancelled);
        await Assert.That(registry.GetResult(queued.GetType())!.Status).IsEqualTo(ModuleStatus.Cancelled);
        runner.Verify(x => x.ExecuteAsync(It.Is<ModuleState>(state => state.Module == queued),
            It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task SuccessfulCompletion_DoesNotLogCancellation()
    {
        var logs = new StringBuilder();
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        readyModules.Writer.Complete();

        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);

        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents.Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);

        var module = new Mock<IModule>();
        module.SetupGet(x => x.Configuration).Returns(ModuleConfiguration.Default);

        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            Mock.Of<IModuleRunner>(),
            Mock.Of<IAlwaysRunHandler>(),
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<ISecondaryExceptionContainer>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new StringLogger<ModuleExecutor>(logs));

        await executor.ExecuteAsync([module.Object]);

        var logOutput = logs.ToString();
        await Assert.That(logOutput).DoesNotContain("Cancellation triggered");
        scheduler.Verify(x => x.CancelPendingModules(), Times.Once);
    }

    [Test]
    public async Task SchedulerAndAlwaysRunFaults_AreAggregatedAfterRegisteringTerminatedResults()
    {
        var schedulerException = new DependencyCollisionException("Scheduler fault");
        var alwaysRunException = new InvalidOperationException("AlwaysRun fault");
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        readyModules.Writer.Complete(schedulerException);
        var cancelledModule = new LaterModule();
        IReadOnlyList<IModule> cancelledModules = [cancelledModule];
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        scheduler.Setup(x => x.CancelPendingModules())
            .Returns(cancelledModules);
        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);
        var resultRegistrar = new Mock<IModuleResultRegistrar>();
        var terminatedResultsRegistered = false;
        resultRegistrar
            .Setup(x => x.RegisterTerminatedResultsForCancelledModules(
                cancelledModules,
                schedulerException))
            .Callback(() => terminatedResultsRegistered = true);
        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler
            .Setup(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(() =>
            {
                if (!terminatedResultsRegistered)
                {
                    return Task.FromException(new InvalidOperationException(
                        "Terminated results were not registered before AlwaysRun processing."));
                }

                return Task.FromException(new AggregateException(
                    schedulerException,
                    alwaysRunException));
            });
        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents
            .Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);
        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            Mock.Of<IModuleRunner>(),
            alwaysRunHandler.Object,
            resultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<ISecondaryExceptionContainer>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLogger<ModuleExecutor>.Instance);

        var exception = await Assert.ThrowsAsync<AggregateException>(
            async () => await executor.ExecuteAsync([cancelledModule]));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.InnerExceptions[0]).IsSameReferenceAs(schedulerException);
            await Assert.That(exception.InnerExceptions[1]).IsSameReferenceAs(alwaysRunException);
            await Assert.That(exception.InnerExceptions).Count().IsEqualTo(2);
            resultRegistrar.Verify(x => x.RegisterTerminatedResultsForCancelledModules(
                cancelledModules,
                schedulerException), Times.Once);
            alwaysRunHandler.Verify(x => x.WaitForAlwaysRunModulesAsync(
                scheduler.Object,
                It.IsAny<IReadOnlyList<IModule>>()), Times.Once);
        }
    }

    [Test]
    public async Task FlattenDistinctExceptions_PreservesEmptyAggregates()
    {
        var pipelineException = new AggregateException();
        var teardownException = new AggregateException();
        var nestedTeardownException = new AggregateException(teardownException);

        var exceptions = ModuleExecutor.FlattenDistinctExceptions(
            pipelineException,
            nestedTeardownException);

        using (Assert.Multiple())
        {
            await Assert.That(exceptions).Count().IsEqualTo(2);
            await Assert.That(exceptions[0]).IsSameReferenceAs(pipelineException);
            await Assert.That(exceptions[1]).IsSameReferenceAs(teardownException);
        }
    }

    [Test]
    public async Task FailFast_SurfacesAllConcurrentWorkerFaults()
    {
        var faultingModule = new FaultingModule();
        var laterModule = new LaterModule();
        var workersStarted = 0;
        var bothWorkersStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executor = CreateFailFastExecutor(
            faultingModule,
            laterModule,
            async (moduleState, _) =>
            {
                if (Interlocked.Increment(ref workersStarted) == 2)
                {
                    bothWorkersStarted.TrySetResult();
                }

                await bothWorkersStarted.Task.WaitAsync(TimeSpan.FromSeconds(5), CancellationToken.None);
                throw new InvalidOperationException(moduleState.ModuleType.Name);
            });

        var exception = await Assert.That(async () =>
                await executor.ExecuteAsync([faultingModule, laterModule]))
            .Throws<AggregateException>();
        await Assert.That(exception!.InnerExceptions.Select(x => x.Message))
            .IsEquivalentTo([nameof(FaultingModule), nameof(LaterModule)]);
    }

    [Test]
    public async Task FailFast_DoesNotDuplicateSharedWorkerFault()
    {
        var sharedException = new InvalidOperationException("Shared failure");
        var faultingModule = new FaultingModule();
        var laterModule = new LaterModule();
        var executor = CreateFailFastExecutor(
            faultingModule,
            laterModule,
            (_, _) => Task.FromException(sharedException));

        var exception = await Assert.That(async () =>
                await executor.ExecuteAsync([faultingModule, laterModule]))
            .Throws<InvalidOperationException>();

        await Assert.That(exception).IsSameReferenceAs(sharedException);
    }

    [Test]
    public async Task FailFast_LateStartsQueuedAlwaysRunModule()
    {
        var faultingModule = new FaultingModule();
        var alwaysRunModule = new QueuedAlwaysRunModule();
        var faultingState = new ModuleState(faultingModule, typeof(FaultingModule))
        {
            State = ModuleExecutionState.Queued,
        };
        var alwaysRunState = new ModuleState(alwaysRunModule, typeof(QueuedAlwaysRunModule))
        {
            State = ModuleExecutionState.Queued,
        };
        var moduleStates = new Dictionary<Type, ModuleState>
        {
            [typeof(FaultingModule)] = faultingState,
            [typeof(QueuedAlwaysRunModule)] = alwaysRunState,
        };
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        readyModules.Writer.TryWrite(faultingState);
        readyModules.Writer.TryWrite(alwaysRunState);
        readyModules.Writer.Complete();

        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);
        scheduler
            .Setup(x => x.GetModuleState(It.IsAny<Type>()))
            .Returns((Type moduleType) => moduleStates[moduleType]);
        scheduler
            .Setup(x => x.GetModuleCompletionTask(It.IsAny<Type>()))
            .Returns((Type moduleType) => moduleStates[moduleType].CompletionSource.Task);

        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);

        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents.Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);

        var moduleRunner = new Mock<IModuleRunner>();
        var primaryException = new InvalidOperationException("Primary failure");
        moduleRunner
            .Setup(x => x.ExecuteAsync(
                faultingState,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(primaryException);
        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                alwaysRunState,
                CancellationToken.None))
            .Returns(() =>
            {
                alwaysRunState.State = ModuleExecutionState.Completed;
                alwaysRunState.CompletionSource.TrySetResult(alwaysRunModule);
                return Task.CompletedTask;
            });

        var pipelineOptions = Microsoft.Extensions.Options.Options.Create(new PipelineOptions
        {
            FailureMode = FailureMode.FailFast,
        });
        var alwaysRunHandler = new AlwaysRunHandler(
            moduleRunner.Object,
            parallelLimitProvider.Object,
            pipelineOptions,
            NullLogger<AlwaysRunHandler>.Instance,
            TimeProvider.System);
        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            moduleRunner.Object,
            alwaysRunHandler,
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            new SecondaryExceptionContainer(),
            pipelineOptions,
            NullLogger<ModuleExecutor>.Instance);

        var exception = await Assert.That(async () =>
                await executor.ExecuteAsync([faultingModule, alwaysRunModule]))
            .Throws<InvalidOperationException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception).IsSameReferenceAs(primaryException);
            await Assert.That(alwaysRunState.State).IsEqualTo(ModuleExecutionState.Completed);
        }

        moduleRunner.Verify(
            x => x.ExecuteWithoutDependencyWaitAsync(
                alwaysRunState,
                CancellationToken.None),
            Times.Once());
    }

    [Test]
    public async Task FailFast_IgnoresWorkerCancellation()
    {
        var faultingModule = new FaultingModule();
        var laterModule = new LaterModule();
        var workersStarted = 0;
        var bothWorkersStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executor = CreateFailFastExecutor(
            faultingModule,
            laterModule,
            async (moduleState, cancellationToken) =>
            {
                if (Interlocked.Increment(ref workersStarted) == 2)
                {
                    bothWorkersStarted.TrySetResult();
                }

                await bothWorkersStarted.Task.WaitAsync(TimeSpan.FromSeconds(5), CancellationToken.None);

                if (moduleState.ModuleType == typeof(FaultingModule))
                {
                    throw new InvalidOperationException("Primary failure");
                }

                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            });

        var exception = await Assert.That(async () =>
                await executor.ExecuteAsync([faultingModule, laterModule]))
            .Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).IsEqualTo("Primary failure");
    }

    [Test]
    public async Task FailFast_SurfacesIndependentWorkerCancellation()
    {
        var faultingModule = new FaultingModule();
        var laterModule = new LaterModule();
        var workersStarted = 0;
        var bothWorkersStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var failFastCancellationObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var independentCancellationToken = new CancellationToken(canceled: true);
        var executor = CreateFailFastExecutor(
            faultingModule,
            laterModule,
            async (moduleState, cancellationToken) =>
            {
                if (Interlocked.Increment(ref workersStarted) == 2)
                {
                    bothWorkersStarted.TrySetResult();
                }

                await bothWorkersStarted.Task.WaitAsync(TimeSpan.FromSeconds(5), CancellationToken.None);

                if (moduleState.ModuleType == typeof(FaultingModule))
                {
                    throw new InvalidOperationException("Primary failure");
                }

                using var registration = cancellationToken.Register(failFastCancellationObserved.SetResult);
                // Observe pool cancellation before throwing the unrelated cancellation under test.
                await failFastCancellationObserved.Task.WaitAsync(TimeSpan.FromSeconds(5), CancellationToken.None);
                throw new OperationCanceledException(independentCancellationToken);
            });

        var exception = await Assert.That(async () =>
                await executor.ExecuteAsync([faultingModule, laterModule]))
            .Throws<AggregateException>();

        await Assert.That(exception!.InnerExceptions.Select(x => x.GetType()))
            .IsEquivalentTo([typeof(InvalidOperationException), typeof(OperationCanceledException)]);
    }

    [Test]
    public async Task ContinueOnFailure_WorkerFault_DoesNotStopRemainingModules()
    {
        var dependencyRegistry = new ModuleDependencyRegistry();
        var metadataRegistry = new ModuleMetadataRegistry(new ModuleAttributeEventService());
        var scheduler = new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            dependencyRegistry,
            metadataRegistry,
            Mock.Of<IMetricsCollector>(),
            new ModuleConstraintEvaluator(NullLogger<ModuleConstraintEvaluator>.Instance),
            Mock.Of<ISchedulerStatusReporter>());
        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler);

        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents.Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);

        var laterModuleRan = false;
        var secondaryExceptionContainer = new Mock<ISecondaryExceptionContainer>();
        var resultRegistrar = new Mock<IModuleResultRegistrar>();
        var moduleRunner = new Mock<IModuleRunner>();
        moduleRunner
            .Setup(x => x.ExecuteAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>((moduleState, _) =>
            {
                if (moduleState.ModuleType == typeof(FaultingModule))
                {
                    throw new InvalidOperationException("Worker fault");
                }

                if (!moduleState.Scheduler!.MarkModuleStarted(moduleState.ModuleType))
                {
                    throw new InvalidOperationException("Later module could not start");
                }

                laterModuleRan = true;
                ((LaterModule) moduleState.Module).CompletionSource.TrySetResult(
                    ModuleResult<bool>.CreateSuccess(true, new ModuleExecutionContext(moduleState.Module, moduleState.ModuleType)));
                moduleState.Scheduler.MarkModuleCompleted(moduleState.ModuleType, success: true);
                return Task.CompletedTask;
            });

        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            moduleRunner.Object,
            Mock.Of<IAlwaysRunHandler>(),
            resultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            dependencyRegistry,
            metadataRegistry,
            secondaryExceptionContainer.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                FailureMode = FailureMode.ContinueOnFailure,
            }),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ModuleExecutor>>());

        await executor.ExecuteAsync([new FaultingModule(), new LaterModule()])
            .WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(laterModuleRan).IsTrue();
        secondaryExceptionContainer.Verify(
            x => x.RegisterException(It.Is<InvalidOperationException>(exception => exception.Message == "Worker fault")),
            Times.Once);
        resultRegistrar.Verify(
            x => x.RegisterTerminatedResult(
                It.IsAny<FaultingModule>(),
                typeof(FaultingModule),
                It.Is<InvalidOperationException>(exception => exception.Message == "Worker fault")),
            Times.Once);
    }

    [Test]
    public async Task ContinueOnFailure_RecoveryFault_DoesNotStopRemainingModules()
    {
        var faultingModule = new FaultingModule();
        var laterModule = new LaterModule();
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        await readyModules.Writer.WriteAsync(new ModuleState(faultingModule, typeof(FaultingModule)));
        await readyModules.Writer.WriteAsync(new ModuleState(laterModule, typeof(LaterModule)));
        readyModules.Writer.Complete();

        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        scheduler
            .Setup(x => x.MarkModuleCompleted(
                typeof(FaultingModule),
                false,
                It.IsAny<Exception>(),
                It.IsAny<ModuleStatus?>()))
            .Throws(new InvalidOperationException("Recovery fault"));

        scheduler.Setup(x => x.GetModuleState(typeof(FaultingModule)))
            .Returns(new ModuleState(faultingModule, typeof(FaultingModule)));
        scheduler.Setup(x => x.GetModuleState(typeof(LaterModule)))
            .Returns(new ModuleState(laterModule, typeof(LaterModule)));

        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);

        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents.Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(1);

        var laterModuleRan = false;
        var moduleRunner = new Mock<IModuleRunner>();
        moduleRunner
            .Setup(x => x.ExecuteAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>((moduleState, _) =>
            {
                if (moduleState.ModuleType == typeof(FaultingModule))
                {
                    throw new InvalidOperationException("Worker fault");
                }

                laterModuleRan = true;
                ((LaterModule) moduleState.Module).CompletionSource.TrySetResult(
                    ModuleResult<bool>.CreateSuccess(true, new ModuleExecutionContext(moduleState.Module, moduleState.ModuleType)));
                return Task.CompletedTask;
            });

        var secondaryExceptionContainer = new Mock<ISecondaryExceptionContainer>();
        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            moduleRunner.Object,
            Mock.Of<IAlwaysRunHandler>(),
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            secondaryExceptionContainer.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                FailureMode = FailureMode.ContinueOnFailure,
            }),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ModuleExecutor>>());

        await executor.ExecuteAsync([faultingModule, laterModule]);

        await Assert.That(laterModuleRan).IsTrue();
        secondaryExceptionContainer.Verify(
            x => x.RegisterException(It.Is<InvalidOperationException>(exception => exception.Message == "Recovery fault")),
            Times.Once);
    }

    [Test]
    public async Task MarkModuleCompleted_WhenAlreadyCompleted_RecordsMetricsOnce()
    {
        var module = new FaultingModule();
        var state = new ModuleState(module, typeof(FaultingModule));
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var metricsCollector = new Mock<IMetricsCollector>();
        var tracker = CreateModuleStateTracker(new StringBuilder(), moduleStates, metricsCollector.Object);

        tracker.MarkModuleCompleted(state.ModuleType, success: false, new InvalidOperationException("First"));
        tracker.MarkModuleCompleted(state.ModuleType, success: false, new InvalidOperationException("Second"));

        metricsCollector.Verify(
            x => x.RecordModuleCompleted(
                state.ModuleType,
                It.IsAny<DateTimeOffset>(),
                false,
                false,
                ModuleStatus.Failed),
            Times.Once);
    }

    [Test]
    [Arguments(LogLevel.Debug, false)]
    [Arguments(LogLevel.Trace, true)]
    public async Task MarkModuleCompleted_Logs_Dependent_Detail_Only_At_Trace(
        LogLevel minimumLevel,
        bool expectsDependentDetail)
    {
        var logs = new StringBuilder();
        var completedState = new ModuleState(new FaultingModule(), typeof(FaultingModule));
        var dependentState = new ModuleState(new LaterModule(), typeof(LaterModule));
        dependentState.UnresolvedDependencies.Add(completedState.ModuleType);
        completedState.DependentModules.Add(dependentState);
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>(
        [
            new(completedState.ModuleType, completedState),
            new(dependentState.ModuleType, dependentState),
        ]);
        var tracker = CreateModuleStateTracker(logs, moduleStates, minimumLevel: minimumLevel);

        tracker.MarkModuleCompleted(completedState.ModuleType, success: true);

        var output = logs.ToString();
        await Assert.That(output).Contains("completion unblocks 1 dependents");
        await Assert.That(output.Contains("now ready to execute"))
            .IsEqualTo(expectsDependentDetail);
    }

    [Test]
    public async Task CancelPendingModules_WithNoCancellableModules_DoesNotLogCancellation()
    {
        var logs = new StringBuilder();
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        var tracker = CreateModuleStateTracker(logs, moduleStates);

        tracker.CancelPendingModules();

        await Assert.That(logs.ToString()).DoesNotContain("Cancelling");
    }

    [Test]
    public async Task CancelPendingModules_WithPendingModule_LogsCancellation()
    {
        var logs = new StringBuilder();
        var module = new LaterModule();
        var state = new ModuleState(module, module.GetType());
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var tracker = CreateModuleStateTracker(logs, moduleStates);

        tracker.CancelPendingModules();

        await Assert.That(logs.ToString()).Contains("Cancelling 1 pending/queued modules");
    }

    [Test]
    public async Task CancelPendingModules_LeavesModuleResultForRegistrarCompletion()
    {
        var module = new LaterModule();
        var state = new ModuleState(module, module.GetType());
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var tracker = CreateModuleStateTracker(new StringBuilder(), moduleStates);

        tracker.CancelPendingModules();

        using (Assert.Multiple())
        {
            await Assert.That(state.CompletionSource.Task.IsCanceled).IsTrue();
            await Assert.That(((IInternalModule) module).ResultTask.IsCompleted).IsFalse();
        }
    }

    private static ModuleExecutor CreateFailFastExecutor(
        FaultingModule faultingModule,
        LaterModule laterModule,
        Func<ModuleState, CancellationToken, Task> executeModule)
    {
        var readyModules = Channel.CreateUnbounded<ModuleState>();
        readyModules.Writer.TryWrite(new ModuleState(faultingModule, typeof(FaultingModule)));
        readyModules.Writer.TryWrite(new ModuleState(laterModule, typeof(LaterModule)));
        readyModules.Writer.Complete();

        var scheduler = new Mock<IModuleScheduler>();
        scheduler.SetupGet(x => x.ReadyModules).Returns(readyModules.Reader);
        scheduler.Setup(x => x.RunSchedulerAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        scheduler.Setup(x => x.GetModuleState(typeof(FaultingModule)))
            .Returns(new ModuleState(faultingModule, typeof(FaultingModule)));
        scheduler.Setup(x => x.GetModuleState(typeof(LaterModule)))
            .Returns(new ModuleState(laterModule, typeof(LaterModule)));
        scheduler.Setup(x => x.CancelPendingModules()).Returns([]);

        var schedulerFactory = new Mock<IModuleSchedulerFactory>();
        schedulerFactory.Setup(x => x.Create()).Returns(scheduler.Object);

        var registrationEvents = new Mock<IRegistrationEventExecutor>();
        registrationEvents.Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider.Setup(x => x.GetMaxDegreeOfParallelism()).Returns(2);

        var moduleRunner = new Mock<IModuleRunner>();
        moduleRunner
            .Setup(x => x.ExecuteAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleState, CancellationToken>(executeModule);

        var alwaysRunHandler = new Mock<IAlwaysRunHandler>();
        alwaysRunHandler
            .Setup(x => x.WaitForAlwaysRunModulesAsync(
                It.IsAny<IModuleScheduler>(),
                It.IsAny<IReadOnlyList<IModule>>()))
            .Returns(Task.CompletedTask);

        return new ModuleExecutor(
            schedulerFactory.Object,
            moduleRunner.Object,
            alwaysRunHandler.Object,
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            new SecondaryExceptionContainer(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                FailureMode = FailureMode.FailFast,
            }),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ModuleExecutor>>());
    }

    private static ModuleStateTracker CreateModuleStateTracker(
        StringBuilder logs,
        ConcurrentDictionary<Type, ModuleState> moduleStates,
        IMetricsCollector? metricsCollector = null,
        LogLevel minimumLevel = LogLevel.Trace)
    {
        return new ModuleStateTracker(
            new StringLogger<ModuleStateTracker>(logs, minimumLevel),
            TimeProvider.System,
            metricsCollector ?? Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            moduleStates,
            [],
            [],
            [],
            new ModuleStateQueries(moduleStates),
            new ReaderWriterLockSlim(),
            new SemaphoreSlim(0),
            () => false);
    }
}
