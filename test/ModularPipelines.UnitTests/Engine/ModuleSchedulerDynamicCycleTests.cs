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
