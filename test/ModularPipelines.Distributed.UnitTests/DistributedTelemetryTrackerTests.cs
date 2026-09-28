namespace ModularPipelines.Distributed.UnitTests;

public class DistributedTelemetryTrackerTests
{
    [Test]
    public async Task CreateReport_CalculatesQueueWaitOverheadAndWorkerUtilization()
    {
        var pipelineStart = new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);
        var tracker = new DistributedTelemetryTracker();
        tracker.RecordAssignment(
            new ModuleAssignment
            {
                ModuleId = "Example.BuildModule",
                RequiredCapabilities = CapabilityRequirement.None,
                AlwaysRun = false,
                DependencyResultReferences = [new DependencyResultReference
                {
                    ModuleId = "Example.DependencyModule",
                    IsAvailable = true,
                }],
                EnqueuedAt = pipelineStart.AddSeconds(1),
                PipelineSchemaVersion = string.Empty,
            },
            TimeSpan.FromMilliseconds(100));
        tracker.RecordResult(
            new SerializedModuleResult
            {
                ModuleId = "Example.BuildModule",
                WorkerId = WorkerId.FromInstanceIndex(0),
                Payload = "{}",
                CompletedAt = pipelineStart.AddSeconds(8.5),
                ExecutionTelemetry = new DistributedModuleExecutionTelemetry
                {
                    ClaimedAt = pipelineStart.AddSeconds(3),
                    ExecutionStartedAt = pipelineStart.AddSeconds(4),
                    ExecutionFinishedAt = pipelineStart.AddSeconds(8),
                    DependencyResultTransferDuration = TimeSpan.FromMilliseconds(250),
                    DependencyResultProcessingDuration = TimeSpan.FromMilliseconds(200),
                    ArtifactDownloadDuration = TimeSpan.FromMilliseconds(300),
                    ArtifactUploadDuration = TimeSpan.FromMilliseconds(400),
                },
            },
            pipelineStart.AddSeconds(9),
            "Example.BuildModule");

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
            await Assert.That(module.WorkerId).IsEqualTo(WorkerId.FromInstanceIndex(0));
            await Assert.That(module.ModuleTypeName).IsEqualTo("Example.BuildModule");
            await Assert.That(module.QueueWaitDuration).IsEqualTo(TimeSpan.FromSeconds(2));
            await Assert.That(module.ExecutionDuration).IsEqualTo(TimeSpan.FromSeconds(4));
            await Assert.That(module.DependencyResultTransferDuration).IsEqualTo(TimeSpan.FromMilliseconds(250));
            await Assert.That(module.ResultTransferDuration).IsEqualTo(TimeSpan.FromMilliseconds(500));
            await Assert.That(module.TotalOverheadDuration).IsEqualTo(TimeSpan.FromSeconds(1.75));
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
            new SerializedModuleResult
            {
                ModuleId = "Module",
                WorkerId = WorkerId.FromInstanceIndex(0),
                Payload = "{}",
                CompletedAt = now,
            },
            now,
            "Module");

        await Assert.That(tracker.CreateReport(now, now, configuredWorkerCount: 1)).IsNull();
    }

    [Test]
    public async Task CreateReport_OverlappingModulesCountWorkerBusyTimeOnce()
    {
        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var tracker = new DistributedTelemetryTracker();
        tracker.RecordResult(new SerializedModuleResult
        {
            ModuleId = "First",
            WorkerId = WorkerId.FromInstanceIndex(0),
            Payload = "{}",
            CompletedAt = start.AddSeconds(6),
            ExecutionTelemetry = new DistributedModuleExecutionTelemetry { ClaimedAt = start.AddSeconds(-1) },
        }, start.AddSeconds(6), "First");
        tracker.RecordResult(new SerializedModuleResult
        {
            ModuleId = "Second",
            WorkerId = WorkerId.FromInstanceIndex(0),
            Payload = "{}",
            CompletedAt = start.AddSeconds(12),
            ExecutionTelemetry = new DistributedModuleExecutionTelemetry { ClaimedAt = start.AddSeconds(4) },
        }, start.AddSeconds(12), "Second");

        var report = tracker.CreateReport(start, start.AddSeconds(10), configuredWorkerCount: 2)!;

        await Assert.That(report.Workers[0].BusyDuration).IsEqualTo(TimeSpan.FromSeconds(10));
        await Assert.That(report.Workers[0].IdleDuration).IsEqualTo(TimeSpan.Zero);
        await Assert.That(report.Workers[0].ModuleCount).IsEqualTo(2);
        await Assert.That(report.FleetUtilizationPercentage).IsEqualTo(50);
    }
}
