using Moq;
using ModularPipelines.Testing.Distributed;

namespace ModularPipelines.Distributed.UnitTests.Coordination;

public class ShippedCoordinatorContractTests
{
    [Test]
    public async Task UnexpectedExceptionIncludesContractContext()
    {
        var cause = new IOException("backend failed");
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(cause);

        var exception = await Assert.That(() => DistributedCoordinatorContract.CanceledDequeueThrowsAsync(coordinator.Object))
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.InnerException).IsSameReferenceAs(cause);
        await Assert.That(exception.Message).Contains(nameof(OperationCanceledException));
        await Assert.That(exception.Message).Contains(nameof(IOException));
    }

    [Test]
    public async Task TimeoutRemainsDistinguishable()
    {
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TimeoutException("backend timeout"));

        await Assert.That(() => DistributedCoordinatorContract.CanceledDequeueThrowsAsync(coordinator.Object))
            .Throws<TimeoutException>();
    }

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
    [Arguments("PipelineFailure", 1)]
    [Arguments("Withdraw", 1)]
    [Arguments("Expired", 1)]
    [Arguments("Expired", 2)]
    [Arguments("Heartbeat", 1)]
    [Arguments("Publishing", 1)]
    [Arguments("ScarceCapability", 1)]
    [Arguments("Priority", 1)]
    [Arguments("Priority", 2)]
    [Arguments("Priority", 3)]
    [Arguments("AlternativeCapabilities", 1)]
    [Arguments("AlternativeCapabilities", 2)]
    public async Task MissingRequiredLeaseIncludesContractContext(string scenario, int missingClaim)
    {
        var coordinator = new Mock<IDistributedMasterCoordinator>();
        var claimCount = 0;
        var lease = new ModuleLease
        {
            LeaseId = "lease",
            WorkerId = new WorkerId("contract-worker"),
            Assignment = DistributedCoordinatorContract.CreateAssignment("Contract.Expired"),
        };
        coordinator.Setup(x => x.DequeueModuleAsync(It.IsAny<WorkerId>(), It.IsAny<IReadOnlySet<Capability>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => ++claimCount == missingClaim ? null : lease);
        coordinator.SetupSequence(x => x.WithdrawAssignmentAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true).ReturnsAsync(false);
        coordinator.Setup(x => x.GetActiveLeasesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([lease]);
        coordinator.Setup(x => x.RequeueExpiredLeasesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([lease.Assignment.ModuleId]);

        Task RunContract() => scenario switch
        {
            "PipelineFailure" => DistributedCoordinatorContract.PipelineFailureOnlyReleasesAlwaysRunWorkAsync(coordinator.Object),
            "Withdraw" => DistributedCoordinatorContract.WithdrawRemovesQueuedAssignmentAsync(coordinator.Object),
            "Expired" => DistributedCoordinatorContract.ExpiredLeaseIsRequeuedAsync(coordinator.Object),
            "Heartbeat" => DistributedCoordinatorContract.HeartbeatRenewsLeaseAsync(coordinator.Object),
            "Publishing" => DistributedCoordinatorContract.PublishingReleasesLeaseAsync(coordinator.Object),
            "ScarceCapability" => DistributedCoordinatorContract.ClaimPrefersScarceCapabilityWorkAsync(coordinator.Object),
            "Priority" => DistributedCoordinatorContract.ClaimPrefersPriorityThenCriticalPathAsync(coordinator.Object),
            "AlternativeCapabilities" => DistributedCoordinatorContract.ClaimMatchesAlternativeCapabilitiesAsync(coordinator.Object),
            _ => throw new ArgumentOutOfRangeException(nameof(scenario)),
        };

        var exception = await Assert.That(RunContract).Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("Coordinator contract failed:");
        await Assert.That(exception.Message).Contains("is not null");
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
