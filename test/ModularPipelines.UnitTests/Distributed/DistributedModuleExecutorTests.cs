using ModularPipelines.Context;
using ModularPipelines.Distributed.Master;
using ModularPipelines.Enums;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Distributed;

public class DistributedModuleExecutorTests
{
    private sealed class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

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
            .Setup(x => x.CancelPendingModules())
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

    [Test]
    [Arguments(ModuleStatus.TimedOut)]
    [Arguments(ModuleStatus.Failed)]
    public async Task CreateCollectorFailureResult_PreservesTerminalStatus(ModuleStatus status)
    {
        var module = new TestModule();
        var exception = new InvalidOperationException("Collector failed");

        var result = DistributedModuleExecutor.CreateCollectorFailureResult(
            module,
            typeof(TestModule),
            exception,
            status);

        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(status);
            await Assert.That(result.ExceptionOrDefault).IsSameReferenceAs(exception);
        }
    }
}
