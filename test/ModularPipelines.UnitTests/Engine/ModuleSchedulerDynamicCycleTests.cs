using Microsoft.Extensions.Logging.Abstractions;
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

    private class CompletedDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(nameof(CompletedDependencyModule));
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
    public async Task AddModule_WhenExistingPredicateCreatesCycle_ThrowsAndRollsBack()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingPredicateModule()]);

        await Assert.That(() => scheduler.AddModule(new DynamicPredicateModule()))
            .Throws<DependencyCollisionException>();
        await Assert.That(scheduler.GetModuleState(typeof(DynamicPredicateModule))).IsNull();
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
    public async Task AddModule_ReconcilesExistingPredicateDependencies()
    {
        using var scheduler = CreateScheduler();
        scheduler.InitializeModules([new ExistingPredicateModule()]);

        scheduler.AddModule(new IndependentDynamicModule());

        var existingState = scheduler.GetModuleState(typeof(ExistingPredicateModule));
        await Assert.That(existingState).IsNotNull();
        await Assert.That(existingState!.UnresolvedDependencies).Contains(typeof(IndependentDynamicModule));
    }

    private static ModuleScheduler CreateScheduler()
    {
        return new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new SchedulerOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions())),
            Mock.Of<IMetricsCollector>(),
            Mock.Of<IModuleConstraintEvaluator>(),
            Mock.Of<ISchedulerStatusReporter>());
    }
}
