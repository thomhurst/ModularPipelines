using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Worker;
using ModularPipelines.Engine;

namespace ModularPipelines.Distributed.UnitTests.Worker;

public class WorkerCancellationMonitorTests
{
    [Test]
    public async Task Monitor_Detects_Cancellation_Signal()
    {
        var coordinatorMock = new Mock<IDistributedCoordinator>();
        var callCount = 0;
        coordinatorMock.Setup(c => c.IsCancellationRequestedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                callCount++;
                if (callCount >= 2)
                {
                    return new CancellationSignal("Pipeline cancelled", DateTimeOffset.UtcNow);
                }
                return null;
            });

        // EngineCancellationToken is internal but accessible via InternalsVisibleTo
        // We need to create a real one for this test
        var primaryExMock = new Mock<IPrimaryExceptionContainer>();
        var engineToken = new ModularPipelines.Engine.EngineCancellationToken(primaryExMock.Object);
        var logger = Mock.Of<ILogger<WorkerCancellationMonitor>>();

        var monitor = new WorkerCancellationMonitor(coordinatorMock.Object, engineToken, logger);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        await monitor.StartAsync(cts.Token);
        await Task.Delay(5000); // Give it time to poll and detect cancellation
        await monitor.StopAsync(CancellationToken.None);

        await Assert.That(engineToken.IsCancellationRequested).IsTrue();

        engineToken.Dispose();
    }
}
