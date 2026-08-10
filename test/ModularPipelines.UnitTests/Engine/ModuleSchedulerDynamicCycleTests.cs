using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleSchedulerDynamicCycleTests
{
    [ModularPipelines.Attributes.DependsOn<DynamicModule>(Optional = true)]
    private class ExistingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(ExistingModule));
    }

    [ModularPipelines.Attributes.DependsOn<ExistingModule>]
    private class DynamicModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(DynamicModule));
    }

    private class CompletedDependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(CompletedDependencyModule));
    }

    private class ReadyDependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(ReadyDependencyModule));
    }

    [ModularPipelines.Attributes.DependsOn<ReadyDependencyModule>]
    private class NewlyReadyDependentModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(NewlyReadyDependentModule));
    }

    [ModularPipelines.Attributes.NotInParallel]
    private class DeferredConstraintModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(DeferredConstraintModule));
    }

    [Test]
    public async Task RunSchedulerAsync_WhenDeadlocked_ThrowsHardError()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingModule(), new DynamicModule()]);

        await Assert.That(() => scheduler.RunSchedulerAsync(CancellationToken.None))
            .Throws<DependencyCollisionException>()
            .And.HasMessageContaining("Scheduler deadlock detected with 2 pending module(s)");
    }

    [Test]
    public async Task RunSchedulerAsync_WhenOnlyModuleIsDeferred_DoesNotReportDeadlock()
    {
        var constraintEvaluator = new Mock<IModuleConstraintEvaluator>();
        constraintEvaluator
            .SetupSequence(x => x.CanQueue(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(true)
            .Returns(false)
            .Returns(true);
        constraintEvaluator
            .SetupSequence(x => x.CanStartExecution(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(false)
            .Returns(true);

        using var scheduler = CreateScheduler(constraintEvaluator.Object);
        scheduler.InitializeModules([new DeferredConstraintModule()]);

        var schedulerTask = scheduler.RunSchedulerAsync(CancellationToken.None);
        var firstAttempt = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(scheduler.MarkModuleStarted(firstAttempt.ModuleType)).IsFalse();

        var secondAttempt = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(scheduler.MarkModuleStarted(secondAttempt.ModuleType)).IsTrue();

        scheduler.MarkModuleCompleted(secondAttempt.ModuleType, success: true);
        await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task RunSchedulerAsync_DoesNotPollWhileModuleIsExecuting()
    {
        var constraintEvaluator = new Mock<IModuleConstraintEvaluator>();
        constraintEvaluator
            .Setup(x => x.CanStartExecution(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(true);
        var statusReporter = new Mock<ISchedulerStatusReporter>();

        using var scheduler = CreateScheduler(
            constraintEvaluator.Object,
            statusReporter.Object,
            new SchedulerOptions
            {
                NotificationTimeout = TimeSpan.FromMilliseconds(20),
            });
        scheduler.InitializeModules([new CompletedDependencyModule()]);

        var schedulerTask = scheduler.RunSchedulerAsync(CancellationToken.None);
        var module = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(scheduler.MarkModuleStarted(module.ModuleType)).IsTrue();

        // This observation window must exceed NotificationTimeout so a regression to
        // timeout-based polling has time to execute another scheduling cycle.
        await Task.Delay(150);

        statusReporter.Verify(
            x => x.LogStatusIfIntervalElapsed(
                It.IsAny<ModuleStateQueries>(),
                It.IsAny<ReaderWriterLockSlim>()),
            Times.Never);
        constraintEvaluator.Verify(
            x => x.CanQueue(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()),
            Times.Never);

        scheduler.MarkModuleCompleted(module.ModuleType, success: true);
        await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task RunSchedulerAsync_QueuesDependentWhenDependencyCompletes()
    {
        var constraintEvaluator = new Mock<IModuleConstraintEvaluator>();
        constraintEvaluator
            .Setup(x => x.CanStartExecution(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(true);
        using var scheduler = CreateScheduler(constraintEvaluator.Object);
        scheduler.InitializeModules([new NewlyReadyDependentModule(), new ReadyDependencyModule()]);

        var schedulerTask = scheduler.RunSchedulerAsync(CancellationToken.None);
        var dependency = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(dependency.ModuleType).IsEqualTo(typeof(ReadyDependencyModule));
        await Assert.That(scheduler.MarkModuleStarted(dependency.ModuleType)).IsTrue();
        scheduler.MarkModuleCompleted(dependency.ModuleType, success: true);

        var dependent = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(dependent.ModuleType).IsEqualTo(typeof(NewlyReadyDependentModule));
        await Assert.That(scheduler.MarkModuleStarted(dependent.ModuleType)).IsTrue();
        scheduler.MarkModuleCompleted(dependent.ModuleType, success: true);

        await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task GetStatistics_TracksStateTransitionsIncrementally()
    {
        var constraintEvaluator = new Mock<IModuleConstraintEvaluator>();
        constraintEvaluator
            .Setup(x => x.CanStartExecution(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(true);

        using var scheduler = CreateScheduler(constraintEvaluator.Object);
        scheduler.InitializeModules([new CompletedDependencyModule()]);

        await Assert.That(scheduler.GetStatistics()).IsEqualTo((1, 0, 0, 0, 1));

        var schedulerTask = scheduler.RunSchedulerAsync(CancellationToken.None);
        var module = await scheduler.ReadyModules.ReadAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(5));
        await Assert.That(scheduler.GetStatistics()).IsEqualTo((1, 1, 0, 0, 0));

        await Assert.That(scheduler.MarkModuleStarted(module.ModuleType)).IsTrue();
        await Assert.That(scheduler.GetStatistics()).IsEqualTo((1, 0, 1, 0, 0));

        scheduler.MarkModuleCompleted(module.ModuleType, success: true);
        await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(scheduler.GetStatistics()).IsEqualTo((1, 0, 0, 1, 0));
    }

    [Test]
    public async Task MarkModuleStarted_AfterQueuedModuleIsCancelled_ReturnsFalse()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new CompletedDependencyModule()]);
        var moduleState = scheduler.GetModuleState(typeof(CompletedDependencyModule))!;
        moduleState.State = ModuleExecutionState.Queued;

        var cancelledModules = scheduler.CancelPendingModules();
        var started = scheduler.MarkModuleStarted(typeof(CompletedDependencyModule));

        using (Assert.Multiple())
        {
            await Assert.That(cancelledModules).HasSingleItem();
            await Assert.That(moduleState.State).IsEqualTo(ModuleExecutionState.Completed);
            await Assert.That(started).IsFalse();
        }
    }

    private static ModuleScheduler CreateScheduler(
        IModuleConstraintEvaluator? constraintEvaluator = null,
        ISchedulerStatusReporter? statusReporter = null,
        SchedulerOptions? schedulerOptions = null)
    {
        return new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(schedulerOptions ?? new SchedulerOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<IMetricsCollector>(),
            constraintEvaluator ?? Mock.Of<IModuleConstraintEvaluator>(),
            statusReporter ?? Mock.Of<ISchedulerStatusReporter>());
    }
}
