using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine.Execution;

public class AlwaysRunHandlerTests
{
    [Test]
    public async Task WaitForAlwaysRunModulesAsync_ExecutesPendingModulesConcurrently()
    {
        var firstModule = new FirstAlwaysRunModule();
        var secondModule = new SecondAlwaysRunModule();
        var thirdModule = new ThirdAlwaysRunModule();
        var firstState = new ModuleState(firstModule, firstModule.GetType());
        var secondState = new ModuleState(secondModule, secondModule.GetType());
        var thirdState = new ModuleState(thirdModule, thirdModule.GetType());
        var scheduler = CreateScheduler(firstState, secondState, thirdState);
        var moduleRunner = new Mock<IModuleRunner>();
        var releaseExecutions = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var bothStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var executionLock = new object();
        var activeExecutions = 0;
        var maximumActiveExecutions = 0;

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(),
                CancellationToken.None))
            .Returns(async (ModuleState state, CancellationToken _) =>
            {
                lock (executionLock)
                {
                    activeExecutions++;
                    maximumActiveExecutions = Math.Max(maximumActiveExecutions, activeExecutions);
                    if (activeExecutions == 2)
                    {
                        bothStarted.TrySetResult();
                    }
                }

                await releaseExecutions.Task;
                state.State = ModuleExecutionState.Completed;
                state.CompletionSource.TrySetResult(state.Module);

                lock (executionLock)
                {
                    activeExecutions--;
                }
            });

        var handler = CreateHandler(moduleRunner.Object);
        var executionTask = handler.WaitForAlwaysRunModulesAsync(
            scheduler.Object,
            [firstModule, secondModule, thirdModule]);

        var ranConcurrently = true;
        try
        {
            await bothStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        }
        catch (TimeoutException)
        {
            ranConcurrently = false;
        }
        finally
        {
            releaseExecutions.TrySetResult();
            await executionTask;
        }

        using (Assert.Multiple())
        {
            await Assert.That(ranConcurrently).IsTrue();
            await Assert.That(maximumActiveExecutions).IsEqualTo(2);
        }
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_ExecutesQueuedModule()
    {
        var module = new FirstAlwaysRunModule();
        var moduleState = new ModuleState(module, module.GetType())
        {
            State = ModuleExecutionState.Queued,
        };
        var scheduler = CreateScheduler(moduleState);
        var moduleRunner = new Mock<IModuleRunner>();

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None))
            .Returns(() =>
            {
                moduleState.State = ModuleExecutionState.Completed;
                moduleState.CompletionSource.TrySetResult(module);
                return Task.CompletedTask;
            });

        var handler = CreateHandler(moduleRunner.Object);

        await handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [module]);

        moduleRunner.Verify(
            x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None),
            Times.Once());
        await Assert.That(moduleState.State).IsEqualTo(ModuleExecutionState.Completed);
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_RetriesDeferredPendingModule()
    {
        var module = new FirstAlwaysRunModule();
        var blocker = new BlockingModule();
        var moduleState = new ModuleState(module, module.GetType());
        var blockerState = new ModuleState(blocker, blocker.GetType())
        {
            State = ModuleExecutionState.Executing,
        };
        var scheduler = CreateScheduler(moduleState, blockerState);
        var moduleRunner = new Mock<IModuleRunner>();
        var blockerWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var attempts = 0;

        scheduler
            .Setup(x => x.GetModuleCompletionTask(blocker.GetType()))
            .Callback(() => blockerWaitObserved.TrySetResult())
            .Returns(blockerState.CompletionSource.Task);
        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None))
            .Returns(() =>
            {
                if (Interlocked.Increment(ref attempts) == 2)
                {
                    moduleState.State = ModuleExecutionState.Completed;
                    moduleState.CompletionSource.TrySetResult(module);
                }

                return Task.CompletedTask;
            });

        var handler = CreateHandler(moduleRunner.Object);

        var handlerTask = handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [module, blocker]);
        await blockerWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));
        var attemptsBeforeProgress = attempts;

        blockerState.State = ModuleExecutionState.Completed;
        blockerState.CompletionSource.TrySetResult(blocker);
        await handlerTask;

        using (Assert.Multiple())
        {
            await Assert.That(attemptsBeforeProgress).IsEqualTo(1);
            await Assert.That(attempts).IsEqualTo(2);
            await Assert.That(moduleState.State).IsEqualTo(ModuleExecutionState.Completed);
        }
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_RetriesDeferredQueuedModule()
    {
        var module = new FirstAlwaysRunModule();
        var blocker = new BlockingModule();
        var moduleState = new ModuleState(module, module.GetType())
        {
            State = ModuleExecutionState.Queued,
        };
        var blockerState = new ModuleState(blocker, blocker.GetType())
        {
            State = ModuleExecutionState.Executing,
        };
        var scheduler = CreateScheduler(moduleState, blockerState);
        var moduleRunner = new Mock<IModuleRunner>();
        var blockerWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var attempts = 0;

        scheduler
            .Setup(x => x.GetModuleCompletionTask(blocker.GetType()))
            .Callback(() => blockerWaitObserved.TrySetResult())
            .Returns(blockerState.CompletionSource.Task);
        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None))
            .Returns(() =>
            {
                if (Interlocked.Increment(ref attempts) == 2)
                {
                    moduleState.State = ModuleExecutionState.Completed;
                    moduleState.CompletionSource.TrySetResult(module);
                }

                return Task.CompletedTask;
            });

        var handler = CreateHandler(moduleRunner.Object);

        var handlerTask = handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [module, blocker]);
        await blockerWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));
        var attemptsBeforeProgress = attempts;

        blockerState.State = ModuleExecutionState.Completed;
        blockerState.CompletionSource.TrySetResult(blocker);
        await handlerTask;

        using (Assert.Multiple())
        {
            await Assert.That(attemptsBeforeProgress).IsEqualTo(1);
            await Assert.That(attempts).IsEqualTo(2);
            await Assert.That(moduleState.State).IsEqualTo(ModuleExecutionState.Completed);
        }
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_RetriesDeferredModuleAfterSameBatchProgress()
    {
        var firstModule = new FirstAlwaysRunModule();
        var secondModule = new SecondAlwaysRunModule();
        var firstState = new ModuleState(firstModule, firstModule.GetType());
        var secondState = new ModuleState(secondModule, secondModule.GetType());
        var scheduler = CreateScheduler(firstState, secondState);
        var moduleRunner = new Mock<IModuleRunner>();
        var secondAttemptObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var attempts = 0;

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                It.IsAny<ModuleState>(),
                CancellationToken.None))
            .Returns(async (ModuleState state, CancellationToken _) =>
            {
                var attempt = Interlocked.Increment(ref attempts);
                if (attempt == 1)
                {
                    await secondAttemptObserved.Task;
                }
                else if (attempt == 2)
                {
                    secondAttemptObserved.TrySetResult();
                    return;
                }

                state.State = ModuleExecutionState.Completed;
                state.CompletionSource.TrySetResult(state.Module);
            });

        var handler = CreateHandler(moduleRunner.Object);

        await handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [firstModule, secondModule]);

        using (Assert.Multiple())
        {
            await Assert.That(attempts).IsEqualTo(3);
            await Assert.That(firstState.State).IsEqualTo(ModuleExecutionState.Completed);
            await Assert.That(secondState.State).IsEqualTo(ModuleExecutionState.Completed);
        }
    }

    [Test]
    [Timeout(30_000)]
    public async Task WaitForAlwaysRunModulesAsync_PreservesAlwaysRunDependencyOrder(
        CancellationToken cancellationToken)
    {
        for (var iteration = 0; iteration < 25; iteration++)
        {
            await AssertAlwaysRunDependencyOrderAsync(cancellationToken);
        }
    }

    private static async Task AssertAlwaysRunDependencyOrderAsync(CancellationToken cancellationToken)
    {
        var prerequisite = new FirstAlwaysRunModule();
        var dependent = new SecondAlwaysRunModule();
        var prerequisiteState = new ModuleState(prerequisite, prerequisite.GetType());
        var dependentState = new ModuleState(dependent, dependent.GetType());
        dependentState.RecordDependency(prerequisite.GetType(), optional: false);
        var scheduler = CreateScheduler(prerequisiteState, dependentState);
        var moduleRunner = new Mock<IModuleRunner>();
        var prerequisiteStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releasePrerequisite = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var dependentStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        ModuleExecutionState? prerequisiteStateWhenDependentStarted = null;

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                prerequisiteState,
                CancellationToken.None))
            .Returns(async () =>
            {
                prerequisiteStarted.TrySetResult();
                await releasePrerequisite.Task;
                prerequisiteState.State = ModuleExecutionState.Completed;
                prerequisiteState.CompletionSource.TrySetResult(prerequisite);
            });
        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                dependentState,
                CancellationToken.None))
            .Returns(() =>
            {
                prerequisiteStateWhenDependentStarted = prerequisiteState.State;
                dependentStarted.TrySetResult();
                dependentState.State = ModuleExecutionState.Completed;
                dependentState.CompletionSource.TrySetResult(dependent);
                return Task.CompletedTask;
            });

        var handler = CreateHandler(moduleRunner.Object);
        var handlerTask = handler.WaitForAlwaysRunModulesAsync(
            scheduler.Object,
            [prerequisite, dependent]);

        await prerequisiteStarted.Task.WaitAsync(cancellationToken);
        releasePrerequisite.TrySetResult();
        await dependentStarted.Task.WaitAsync(cancellationToken);
        await handlerTask.WaitAsync(cancellationToken);

        await Assert.That(prerequisiteStateWhenDependentStarted)
            .IsEqualTo(ModuleExecutionState.Completed);
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_UsesDedicatedProgressTimeoutWhenModuleTimeoutsAreDisabled()
    {
        var timeProvider = TestPipelineBuilder.CreateFakeTimeProvider();
        var module = new FirstAlwaysRunModule();
        var blocker = new BlockingModule();
        var moduleState = new ModuleState(module, module.GetType());
        var blockerState = new ModuleState(blocker, blocker.GetType())
        {
            State = ModuleExecutionState.Executing,
        };
        var scheduler = CreateScheduler(moduleState, blockerState);
        var moduleRunner = new Mock<IModuleRunner>();
        var progressWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        scheduler
            .Setup(x => x.GetModuleCompletionTask(blocker.GetType()))
            .Callback(() => progressWaitObserved.TrySetResult())
            .Returns(blockerState.CompletionSource.Task);

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None))
            .Returns(Task.CompletedTask);

        var pipelineOptions = new PipelineOptions
        {
            DefaultModuleTimeout = TimeSpan.Zero,
        };
        var handler = CreateHandler(moduleRunner.Object, pipelineOptions, timeProvider);
        var handlerTask = handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [module, blocker]);

        await progressWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));
        timeProvider.Advance(TimeSpan.FromSeconds(30));
        var exception = await Assert.ThrowsAsync<AggregateException>(() => handlerTask);

        await Assert.That(exception!.InnerExceptions).Contains(x => x is TimeoutException);
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_UsesCumulativeSchedulerProgressTimeout()
    {
        var timeProvider = TestPipelineBuilder.CreateFakeTimeProvider();
        var module = new FirstAlwaysRunModule();
        var firstBlocker = new FirstBlockingModule();
        var secondBlocker = new SecondBlockingModule();
        var moduleState = new ModuleState(module, module.GetType());
        var firstBlockerState = new ModuleState(firstBlocker, firstBlocker.GetType())
        {
            State = ModuleExecutionState.Executing,
        };
        var secondBlockerState = new ModuleState(secondBlocker, secondBlocker.GetType())
        {
            State = ModuleExecutionState.Executing,
        };
        var scheduler = CreateScheduler(moduleState, firstBlockerState, secondBlockerState);
        var moduleRunner = new Mock<IModuleRunner>();
        var firstWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var secondBlockerTaskRequests = 0;
        var attempts = 0;

        scheduler
            .Setup(x => x.GetModuleCompletionTask(firstBlocker.GetType()))
            .Callback(() => firstWaitObserved.TrySetResult())
            .Returns(firstBlockerState.CompletionSource.Task);
        scheduler
            .Setup(x => x.GetModuleCompletionTask(secondBlocker.GetType()))
            .Callback(() =>
            {
                if (Interlocked.Increment(ref secondBlockerTaskRequests) == 2)
                {
                    secondWaitObserved.TrySetResult();
                }
            })
            .Returns(secondBlockerState.CompletionSource.Task);
        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                CancellationToken.None))
            .Callback(() => Interlocked.Increment(ref attempts))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler(
            moduleRunner.Object,
            new PipelineOptions
            {
                AlwaysRunProgressTimeout = TimeSpan.FromMilliseconds(200),
            },
            timeProvider);
        var handlerTask = handler.WaitForAlwaysRunModulesAsync(
            scheduler.Object,
            [module, firstBlocker, secondBlocker]);

        await firstWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));
        timeProvider.Advance(TimeSpan.FromMilliseconds(150));
        firstBlockerState.State = ModuleExecutionState.Completed;
        firstBlockerState.CompletionSource.TrySetResult(firstBlocker);
        await secondWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));

        timeProvider.Advance(TimeSpan.FromMilliseconds(50));
        var exception = await Assert.ThrowsAsync<AggregateException>(() => handlerTask);

        secondBlockerState.State = ModuleExecutionState.Completed;
        secondBlockerState.CompletionSource.TrySetResult(secondBlocker);

        using (Assert.Multiple())
        {
            await Assert.That(attempts).IsEqualTo(2);
            await Assert.That(exception!.InnerExceptions).Contains(x => x is TimeoutException);
        }
    }

    private static Mock<IModuleScheduler> CreateScheduler(params ModuleState[] moduleStates)
    {
        var statesByType = moduleStates.ToDictionary(x => x.ModuleType);
        var scheduler = new Mock<IModuleScheduler>();

        scheduler
            .Setup(x => x.GetModuleState(It.IsAny<Type>()))
            .Returns((Type moduleType) => statesByType.GetValueOrDefault(moduleType));
        scheduler
            .Setup(x => x.GetModuleCompletionTask(It.IsAny<Type>()))
            .Returns((Type moduleType) => statesByType.GetValueOrDefault(moduleType)?.CompletionSource.Task);

        return scheduler;
    }

    private static AlwaysRunHandler CreateHandler(
        IModuleRunner moduleRunner,
        PipelineOptions? pipelineOptions = null,
        TimeProvider? timeProvider = null)
    {
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider
            .Setup(x => x.GetMaxDegreeOfParallelism())
            .Returns(2);

        return new AlwaysRunHandler(
            moduleRunner,
            parallelLimitProvider.Object,
            Microsoft.Extensions.Options.Options.Create(pipelineOptions ?? new PipelineOptions()),
            NullLogger<AlwaysRunHandler>.Instance,
            timeProvider ?? TimeProvider.System);
    }

    private abstract class AlwaysRunTestModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            module.WithAlwaysRun();
        }

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class FirstAlwaysRunModule : AlwaysRunTestModule;

    private sealed class SecondAlwaysRunModule : AlwaysRunTestModule;

    private sealed class ThirdAlwaysRunModule : AlwaysRunTestModule;

    private class BlockingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class FirstBlockingModule : BlockingModule;

    private sealed class SecondBlockingModule : BlockingModule;
}
