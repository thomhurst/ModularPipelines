using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleSchedulerDynamicCycleTests
{
    private abstract class DynamicBaseModule : Module<string>
    {
    }

    [ModularPipelines.Attributes.DependsOn<DynamicModule>(Optional = true)]
    private class ExistingModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(ExistingModule));
    }

    [ModularPipelines.Attributes.DependsOn<ExistingModule>]
    private class DynamicModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(DynamicModule));
    }

    [ModularPipelines.Attributes.DependsOnAllModulesInheritingFrom<DynamicBaseModule>]
    private class ExistingPredicateModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(ExistingPredicateModule));
    }

    [ModularPipelines.Attributes.DependsOn<ExistingPredicateModule>]
    private class DynamicPredicateModule : DynamicBaseModule
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(DynamicPredicateModule));
    }

    private class IndependentDynamicModule : DynamicBaseModule
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(IndependentDynamicModule));
    }

    private class FluentExistingModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<IndependentDynamicModule>()
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(FluentExistingModule));
    }

    private class CompletedDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(CompletedDependencyModule));
    }

    [ModularPipelines.Attributes.NotInParallel]
    private class DeferredConstraintModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(DeferredConstraintModule));
    }

    private class ConditionalExistingModule : Module<string>
    {
        public bool IncludeDependency { get; set; } = true;

        protected override void DeclareDependencies(IDependencyDeclaration deps)
        {
            deps.DependsOnIf<CompletedDependencyModule>(IncludeDependency);
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(ConditionalExistingModule));
    }

    [ModularPipelines.Attributes.DependsOn<CompletedDependencyModule>]
    private class LateDynamicModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(LateDynamicModule));
    }

    [Test]
    public async Task AddModule_WhenModuleIntroducesCycle_ThrowsAndRollsBack()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingModule()]);

        await Assert.That(() => scheduler.AddModule(new DynamicModule()))
            .Throws<DependencyCollisionException>()
            .And.HasMessageEqualTo(
                "Dependency collision detected: **ExistingModule** -> DynamicModule -> **ExistingModule**");
        await Assert.That(scheduler.GetModuleState(typeof(DynamicModule))).IsNull();
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

        await Task.Delay(150);

        statusReporter.Verify(
            x => x.LogStatusIfIntervalElapsed(
                It.IsAny<ModuleStateQueries>(),
                It.IsAny<ReaderWriterLockSlim>()),
            Times.Never);

        scheduler.MarkModuleCompleted(module.ModuleType, success: true);
        await schedulerTask.WaitAsync(TimeSpan.FromSeconds(5));

        constraintEvaluator.Verify(
            x => x.CanQueue(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()),
            Times.Never);
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
    public async Task AddModule_WhenExistingPredicateCreatesCycle_ThrowsAndRollsBack()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingPredicateModule()]);

        await Assert.That(() => scheduler.AddModule(new DynamicPredicateModule()))
            .Throws<DependencyCollisionException>();
        await Assert.That(scheduler.GetModuleState(typeof(DynamicPredicateModule))).IsNull();
        await Assert.That(scheduler.GetModuleState(typeof(ExistingPredicateModule))!.Dependencies
            .ContainsKey(typeof(DynamicPredicateModule))).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AddModule_DoesNotChangeActiveModuleDependencies(bool executing)
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingPredicateModule()]);
        var existingState = scheduler.GetModuleState(typeof(ExistingPredicateModule))!;
        existingState.State = executing ? ModuleExecutionState.Executing : ModuleExecutionState.Queued;

        scheduler.AddModule(new DynamicPredicateModule());

        await Assert.That(existingState.Dependencies.ContainsKey(typeof(DynamicPredicateModule))).IsFalse();
        await Assert.That(existingState.UnresolvedDependencies).DoesNotContain(typeof(DynamicPredicateModule));
        await Assert.That(scheduler.GetModuleState(typeof(DynamicPredicateModule))!.UnresolvedDependencies)
            .Contains(typeof(ExistingPredicateModule));
    }

    [Test]
    public async Task AddModule_WhenDependencyAlreadyCompleted_IsReady()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new CompletedDependencyModule()]);
        scheduler.MarkModuleCompleted(typeof(CompletedDependencyModule), success: true);

        scheduler.AddModule(new LateDynamicModule());

        var state = scheduler.GetModuleState(typeof(LateDynamicModule));
        await Assert.That(state).IsNotNull();
        await Assert.That(state!.UnresolvedDependencies).IsEmpty();
        await Assert.That(state.IsReadyToExecute).IsTrue();
    }

    [Test]
    public async Task AddModule_DoesNotCreateCycleThroughCompletedDependent()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingModule()]);
        scheduler.MarkModuleCompleted(typeof(ExistingModule), success: true);

        scheduler.AddModule(new DynamicModule());

        var state = scheduler.GetModuleState(typeof(DynamicModule));
        await Assert.That(state).IsNotNull();
        await Assert.That(state!.IsReadyToExecute).IsTrue();
    }

    [Test]
    public async Task AddModule_ReconcilesExistingPredicateDependencies()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingPredicateModule()]);

        scheduler.AddModule(new IndependentDynamicModule());

        var existingState = scheduler.GetModuleState(typeof(ExistingPredicateModule));
        await Assert.That(existingState).IsNotNull();
        await Assert.That(existingState!.UnresolvedDependencies).Contains(typeof(IndependentDynamicModule));
    }

    [Test]
    public async Task AddModule_ReconcilesExistingFluentDependencies()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new FluentExistingModule()]);

        scheduler.AddModule(new IndependentDynamicModule());

        var existingState = scheduler.GetModuleState(typeof(FluentExistingModule));
        await Assert.That(existingState).IsNotNull();
        await Assert.That(existingState!.UnresolvedDependencies).Contains(typeof(IndependentDynamicModule));
    }

    [Test]
    public async Task AddModule_PreservesInitiallyDeclaredConditionalDependency()
    {
        var conditionalModule = new ConditionalExistingModule();
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new CompletedDependencyModule(), conditionalModule]);
        conditionalModule.IncludeDependency = false;

        scheduler.AddModule(new IndependentDynamicModule());

        var state = scheduler.GetModuleState(typeof(ConditionalExistingModule));
        await Assert.That(state).IsNotNull();
        await Assert.That(state!.UnresolvedDependencies).Contains(typeof(CompletedDependencyModule));
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
            new ModuleMetadataRegistry(Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions())),
            Mock.Of<IMetricsCollector>(),
            constraintEvaluator ?? Mock.Of<IModuleConstraintEvaluator>(),
            statusReporter ?? Mock.Of<ISchedulerStatusReporter>());
    }
}
