using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Scheduling;
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

        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            Mock.Of<IModuleRunner>(),
            Mock.Of<IAlwaysRunHandler>(),
            Mock.Of<IModuleResultRegistrar>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new StringLogger<ModuleExecutor>(logs));

        await executor.ExecuteAsync([Mock.Of<IModule>()]);

        var logOutput = logs.ToString();
        await Assert.That(logOutput).DoesNotContain("Cancellation triggered");
        scheduler.Verify(x => x.CancelPendingModules(), Times.Once);
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
        var module = new Mock<IModule>();
        module.SetupGet(x => x.ModuleRunType).Returns(ModuleRunType.OnSuccessfulDependencies);
        var state = new ModuleState(module.Object, typeof(IModule));
        var moduleStates = new ConcurrentDictionary<Type, ModuleState>();
        moduleStates[state.ModuleType] = state;
        var tracker = CreateModuleStateTracker(logs, moduleStates);

        tracker.CancelPendingModules();

        await Assert.That(logs.ToString()).Contains("Cancelling 1 pending/queued modules");
    }

    private static ModuleStateTracker CreateModuleStateTracker(
        StringBuilder logs,
        ConcurrentDictionary<Type, ModuleState> moduleStates)
    {
        return new ModuleStateTracker(
            new StringLogger<ModuleStateTracker>(logs),
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
    }
}
