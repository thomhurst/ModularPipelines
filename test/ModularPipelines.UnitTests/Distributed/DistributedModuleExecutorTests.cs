using ModularPipelines.Distributed.Master;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Distributed;

public class DistributedModuleExecutorTests
{
    [Test]
    public void CompleteCancelledModules_RegistersTerminatedResults()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var cancellationToken = cancellationTokenSource.Token;
        IReadOnlyList<IModule> cancelledModules = [Mock.Of<IModule>()];
        var scheduler = new Mock<IModuleScheduler>();
        var resultRegistrar = new Mock<IModuleResultRegistrar>();
        scheduler
            .Setup(x => x.CancelPendingModules(false))
            .Returns(cancelledModules);

        DistributedModuleExecutor.CompleteCancelledModules(
            scheduler.Object,
            resultRegistrar.Object,
            cancellationToken);

        resultRegistrar.Verify(
            x => x.RegisterTerminatedResultsForCancelledModules(
                cancelledModules,
                It.Is<OperationCanceledException>(
                    exception => exception.CancellationToken == cancellationToken)),
            Times.Once);
    }
}
