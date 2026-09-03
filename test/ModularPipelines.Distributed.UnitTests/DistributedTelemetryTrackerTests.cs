namespace ModularPipelines.Distributed.UnitTests;

public class DistributedTelemetryTrackerTests
{
    [Test]
    public async Task CreateReport_CalculatesQueueWaitOverheadAndWorkerUtilization()
    {
        var pipelineStart = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var tracker = new DistributedTelemetryTracker();
        tracker.RecordAssignment(
            new ModuleAssignment(
                "Example.BuildModule",
                "System.String",
                [],
                pipelineStart,
                new ModuleAssignmentOptions(null, false),
                [CreateDependencyResult(pipelineStart)])
            {
                EnqueuedAt = pipelineStart.AddSeconds(1),
            },
            TimeSpan.FromMilliseconds(100));
        tracker.RecordResult(
            new SerializedModuleResult(
                "Example.BuildModule",
                "System.String",
                0,
                "{}",
                pipelineStart.AddSeconds(8.5))
            {
                ExecutionTelemetry = new DistributedModuleExecutionTelemetry
                {
                    ClaimedAt = pipelineStart.AddSeconds(3),
                    ExecutionStartedAt = pipelineStart.AddSeconds(4),
                    ExecutionFinishedAt = pipelineStart.AddSeconds(8),
                    DependencyResultProcessingDuration = TimeSpan.FromMilliseconds(200),
                    ArtifactDownloadDuration = TimeSpan.FromMilliseconds(300),
                    ArtifactUploadDuration = TimeSpan.FromMilliseconds(400),
                },
            },
            pipelineStart.AddSeconds(9));

        var report = tracker.CreateReport(
            pipelineStart,
            pipelineStart.AddSeconds(10),
            configuredWorkerCount: 2);

        await Assert.That(report).IsNotNull();
        var module = report!.Modules.Single();
        using (Assert.Multiple())
        {
            await Assert.That(report.WorkerCount).IsEqualTo(2);
            await Assert.That(report.FleetUtilizationPercentage).IsEqualTo(27.5);
            await Assert.That(module.WorkerIndex).IsEqualTo(0);
            await Assert.That(module.QueueWaitDuration).IsEqualTo(TimeSpan.FromSeconds(2));
            await Assert.That(module.ExecutionDuration).IsEqualTo(TimeSpan.FromSeconds(4));
            await Assert.That(module.DependencyResultTransferDuration).IsEqualTo(TimeSpan.FromMilliseconds(100));
            await Assert.That(module.ResultTransferDuration).IsEqualTo(TimeSpan.FromMilliseconds(500));
            await Assert.That(module.TotalOverheadDuration).IsEqualTo(TimeSpan.FromSeconds(1.5));
            await Assert.That(report.Workers[0].ModuleCount).IsEqualTo(1);
            await Assert.That(report.Workers[0].BusyDuration).IsEqualTo(TimeSpan.FromSeconds(5.5));
            await Assert.That(report.Workers[0].IdleDuration).IsEqualTo(TimeSpan.FromSeconds(4.5));
            await Assert.That(report.Workers[0].UtilizationPercentage).IsEqualTo(55);
            await Assert.That(report.Workers[1].ModuleCount).IsEqualTo(0);
            await Assert.That(report.Workers[1].IdleDuration).IsEqualTo(TimeSpan.FromSeconds(10));
            await Assert.That(report.Workers[1].UtilizationPercentage).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CreateReport_WithoutWorkerTelemetry_ReturnsNull()
    {
        var now = DateTimeOffset.UtcNow;
        var tracker = new DistributedTelemetryTracker();
        tracker.RecordResult(
            new SerializedModuleResult("Module", "System.String", 0, "{}", now),
            now);

        await Assert.That(tracker.CreateReport(now, now, configuredWorkerCount: 1)).IsNull();
    }

    private static SerializedModuleResult CreateDependencyResult(DateTimeOffset completedAt) =>
        new("Example.DependencyModule", "System.String", 0, "{}", completedAt);
}
