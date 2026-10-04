using Moq;
using ModularPipelines.Testing.Distributed;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class ShippedCoordinatorContractTests
{
    [Test]
    public async Task SampleResultEscapesJsonValues()
    {
        const string value = "quoted \"value\"\nwith newline";
        var result = DistributedCoordinatorContract.CreateResult("test", value);
        using var payload = System.Text.Json.JsonDocument.Parse(result.Payload);
        await Assert.That(payload.RootElement.GetProperty("value").GetString()).IsEqualTo(value);
    }

    [Test]
    public async Task ReportsMissingLeaseAsContractViolation()
    {
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(x => x.EnqueueModuleAsync(It.IsAny<ModuleAssignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);

        var exception = await Assert.That(() => DistributedCoordinatorContract.EnqueueAndDequeueRoundTripsAsync(coordinator.Object))
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("Coordinator contract failed");
    }

    [Test]
    public async Task ReportsMissingCancellationAsContractViolation()
    {
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ModuleLease?) null);

        var exception = await Assert.That(() => DistributedCoordinatorContract.CanceledDequeueThrowsAsync(coordinator.Object))
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains(nameof(OperationCanceledException));
    }
}
