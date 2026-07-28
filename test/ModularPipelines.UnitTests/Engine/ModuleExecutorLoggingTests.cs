using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
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
            new ModuleMetadataRegistry(Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions())),
            Mock.Of<ISecondaryExceptionContainer>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new StringLogger<ModuleExecutor>(logs));

        await executor.ExecuteAsync([module.Object]);

        var logOutput = logs.ToString();
        await Assert.That(logOutput).DoesNotContain("Cancellation triggered");
        scheduler.Verify(x => x.CancelPendingModules(), Times.Once);
    }

    [Test]
    public async Task WaitForAllModules_WorkerFault_DoesNotStopRemainingModules()
    {
        var dependencyRegistry = new ModuleDependencyRegistry();
        var metadataRegistry = new ModuleMetadataRegistry(
            Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions()));
        var scheduler = new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new SchedulerOptions()),
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
        var moduleRunner = new Mock<IModuleRunner>();
        moduleRunner
            .Setup(x => x.ExecuteAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<IModuleScheduler>(),
                It.IsAny<CancellationToken>()))
            .Returns<ModuleState, IModuleScheduler, CancellationToken>((moduleState, moduleScheduler, _) =>
            {
                if (moduleState.ModuleType == typeof(FaultingModule))
                {
                    throw new InvalidOperationException("Worker fault");
                }

                if (!moduleScheduler.MarkModuleStarted(moduleState.ModuleType))
                {
                    throw new InvalidOperationException("Later module could not start");
                }

                laterModuleRan = true;
                moduleScheduler.MarkModuleCompleted(moduleState.ModuleType, success: true);
                return Task.CompletedTask;
            });

        var executor = new ModuleExecutor(
            schedulerFactory.Object,
            moduleRunner.Object,
            Mock.Of<IAlwaysRunHandler>(),
            Mock.Of<IModuleResultRegistrar>(),
            Mock.Of<IModuleResultRegistry>(),
            parallelLimitProvider.Object,
            registrationEvents.Object,
            Mock.Of<IMetricsCollector>(),
            dependencyRegistry,
            metadataRegistry,
            secondaryExceptionContainer.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                ExecutionMode = ExecutionMode.WaitForAllModules,
            }),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<ModuleExecutor>>());

        await executor.ExecuteAsync([new FaultingModule(), new LaterModule()])
            .WaitAsync(TimeSpan.FromSeconds(5));

        await Assert.That(laterModuleRan).IsTrue();
        secondaryExceptionContainer.Verify(
            x => x.RegisterException(It.Is<InvalidOperationException>(exception => exception.Message == "Worker fault")),
            Times.Once);
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
