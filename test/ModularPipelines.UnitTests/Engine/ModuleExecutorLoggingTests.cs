using System.Text;
using System.Threading.Channels;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
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
        await Assert.That(logOutput).DoesNotContain("Cancelling 0 pending");
        scheduler.Verify(x => x.CancelPendingModules(), Times.Once);
    }
}
