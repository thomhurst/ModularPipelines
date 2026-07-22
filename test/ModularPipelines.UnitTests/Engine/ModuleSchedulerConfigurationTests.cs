using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleSchedulerConfigurationTests
{
    [ModularPipelines.Attributes.NotInParallel("direct-lock")]
    [Priority(ModulePriority.Critical)]
    [ExecutionHint(ExecutionType.IoIntensive)]
    private sealed class DirectAttributedModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Default;

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    [Test]
    public async Task InitializeModules_PreservesDirectModuleSchedulingAttributes()
    {
        var scheduler = CreateScheduler();

        scheduler.InitializeModules([new DirectAttributedModule()]);

        var state = scheduler.GetModuleState(typeof(DirectAttributedModule));
        await Assert.That(state).IsNotNull();
        await Assert.That(state!.RequiredLockKeys).IsEquivalentTo(["direct-lock"]);
        await Assert.That(state.Priority).IsEqualTo(ModulePriority.Critical);
        await Assert.That(state.ExecutionType).IsEqualTo(ExecutionType.IoIntensive);
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
