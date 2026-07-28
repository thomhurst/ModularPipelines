using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
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
                scheduler.Object,
                CancellationToken.None))
            .Returns(async (ModuleState state, IModuleScheduler _, CancellationToken _) =>
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
                scheduler.Object,
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
    public async Task WaitForAlwaysRunModulesAsync_PreservesAlwaysRunDependencyOrder()
    {
        var prerequisite = new FirstAlwaysRunModule();
        var dependent = new SecondAlwaysRunModule();
        var prerequisiteState = new ModuleState(prerequisite, prerequisite.GetType());
        var dependentState = new ModuleState(dependent, dependent.GetType());
        dependentState.Dependencies[prerequisite.GetType()] = false;
        var scheduler = CreateScheduler(prerequisiteState, dependentState);
        var moduleRunner = new Mock<IModuleRunner>();
        var prerequisiteStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releasePrerequisite = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var dependentStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                prerequisiteState,
                scheduler.Object,
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
                scheduler.Object,
                CancellationToken.None))
            .Returns(() =>
            {
                dependentStarted.TrySetResult();
                dependentState.State = ModuleExecutionState.Completed;
                dependentState.CompletionSource.TrySetResult(dependent);
                return Task.CompletedTask;
            });

        var handler = CreateHandler(moduleRunner.Object);
        var handlerTask = handler.WaitForAlwaysRunModulesAsync(
            scheduler.Object,
            [prerequisite, dependent]);

        await prerequisiteStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        var dependentStartedBeforePrerequisiteCompleted = await Task.WhenAny(
            dependentStarted.Task,
            Task.Delay(TimeSpan.FromMilliseconds(200))) == dependentStarted.Task;
        releasePrerequisite.TrySetResult();
        await handlerTask;

        await Assert.That(dependentStartedBeforePrerequisiteCompleted).IsFalse();
    }

    [Test]
    public async Task WaitForAlwaysRunModulesAsync_TimesOutWhenSchedulerCannotMakeProgress()
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

        moduleRunner
            .Setup(x => x.ExecuteWithoutDependencyWaitAsync(
                moduleState,
                scheduler.Object,
                CancellationToken.None))
            .Returns(Task.CompletedTask);

        var handler = CreateHandler(moduleRunner.Object, TimeSpan.FromMilliseconds(50));

        var exception = await Assert.ThrowsAsync<AggregateException>(() =>
            handler.WaitForAlwaysRunModulesAsync(scheduler.Object, [module, blocker]));

        await Assert.That(exception!.InnerExceptions).Contains(x => x is TimeoutException);
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
        TimeSpan? schedulerProgressTimeout = null)
    {
        var parallelLimitProvider = new Mock<IParallelLimitProvider>();
        parallelLimitProvider
            .Setup(x => x.GetMaxDegreeOfParallelism())
            .Returns(2);

        return new AlwaysRunHandler(
            moduleRunner,
            parallelLimitProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                DefaultModuleTimeout = schedulerProgressTimeout ?? TimeSpan.FromSeconds(2),
            }),
            NullLogger<AlwaysRunHandler>.Instance);
    }

    private abstract class AlwaysRunTestModule : Module<bool>
    {
        protected override ModuleConfiguration Configure()
        {
            return ModuleConfiguration.Create()
                .WithAlwaysRun()
                .Build();
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

    private sealed class BlockingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
