using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
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
    }

    [Test]
    public async Task InitializeModules_PreservesDirectModuleSchedulingAttributes()
    {
        var metricsCollector = new Mock<IMetricsCollector>();
        var scheduler = CreateScheduler(metricsCollector.Object);

        scheduler.InitializeModules([new DirectAttributedModule()]);

        var state = scheduler.GetModuleState(typeof(DirectAttributedModule));
        await Assert.That(state).IsNotNull();
        await Assert.That(state!.RequiredLockKeys).IsEquivalentTo(["direct-lock"]);
        await Assert.That(state.Priority).IsEqualTo(ModulePriority.Critical);
        await Assert.That(state.ExecutionType).IsEqualTo(ExecutionType.IoIntensive);
        metricsCollector.Verify(x => x.RecordModuleInitialized(
            typeof(DirectAttributedModule),
            ModulePriority.Critical,
            ExecutionType.IoIntensive), Times.Once);
    }

    private static ModuleScheduler CreateScheduler(IMetricsCollector metricsCollector)
    {
        return new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new SchedulerOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            metricsCollector,
            Mock.Of<IModuleConstraintEvaluator>(),
            Mock.Of<ISchedulerStatusReporter>());
    }
}
