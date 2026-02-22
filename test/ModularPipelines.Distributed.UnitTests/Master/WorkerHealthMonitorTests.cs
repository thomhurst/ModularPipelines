using Microsoft.Extensions.Logging;
using Moq;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Master;

namespace ModularPipelines.Distributed.UnitTests.Master;

public class WorkerHealthMonitorTests
{
    [Test]
    public async Task Monitor_Polls_For_Workers()
    {
        var coordinatorMock = new Mock<IDistributedCoordinator>();
        coordinatorMock.Setup(c => c.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<WorkerRegistration>());

        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions
        {
            HeartbeatIntervalSeconds = 1,
            HeartbeatTimeoutSeconds = 5
        });

        var logger = Mock.Of<ILogger<WorkerHealthMonitor>>();
        var monitor = new WorkerHealthMonitor(coordinatorMock.Object, options, logger);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        await monitor.StartAsync(cts.Token);
        await Task.Delay(1500);
        await monitor.StopAsync(CancellationToken.None);

        coordinatorMock.Verify(c => c.GetRegisteredWorkersAsync(It.IsAny<CancellationToken>()), Times.AtLeastOnce());
    }
}
