using System.Text.Json;
namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class DistributedDtoSerializationTests
{
    [Test]
    public async Task ModuleAssignment_RoundTrips_With_DefaultOptions()
    {
        var expected = new ModuleAssignment
        {
            ModuleId = "BuildModule",
            RequiredCapabilities = CapabilityRequirement.AllOf(new Capability("Docker")),
            AlwaysRun = true,
            DependencyResultReferences = [new DependencyResultReference
            {
                ModuleId = "DependencyModule",
                IsAvailable = true,
            }],
            EnqueuedAt = DateTimeOffset.UtcNow,
            SatisfiedConditionGroups = ["Conditions.CrossPlatform"],
            PipelineSchemaVersion = "schema",
            RequiredArtifacts = ["package"],
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<ModuleAssignment>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(json).Contains("\"RequiredCapabilities\":[[\"Docker\"]]");
        await Assert.That(json).Contains("\"ModuleId\":\"BuildModule\"");
        await Assert.That(json).DoesNotContain("MatrixTarget");
        await Assert.That(actual!.EnqueuedAt).IsEqualTo(expected.EnqueuedAt);
        await Assert.That(actual.RequiredCapabilities).IsEqualTo(expected.RequiredCapabilities);
        await Assert.That(actual.SatisfiedConditionGroups).Contains("Conditions.CrossPlatform");
        await Assert.That(actual.AlwaysRun).IsTrue();
        await Assert.That(actual.PipelineSchemaVersion).IsEqualTo("schema");
        await Assert.That(actual.RequiredArtifacts).IsEquivalentTo(expected.RequiredArtifacts);
        await Assert.That(json).DoesNotContain("AssignedAt");
        await Assert.That(actual.DependencyResultReferences).IsEquivalentTo(expected.DependencyResultReferences);
    }

    [Test]
    public async Task WorkerDtos_RoundTrip_With_DefaultOptions()
    {
        var registration = new WorkerRegistration { WorkerId = WorkerId.FromInstanceIndex(1), Capabilities = [new Capability("Docker")], RegisteredAt = DateTimeOffset.UtcNow, RunId = "run-1" };
        var status = new WorkerStatus
        {
            WorkerId = WorkerId.FromInstanceIndex(1),
            RunId = "run-1",
            UnattributedCommandCount = 3,
        };

        var registrationJson = JsonSerializer.Serialize(registration);
        var statusJson = JsonSerializer.Serialize(status);
        var actualRegistration = JsonSerializer.Deserialize<WorkerRegistration>(registrationJson);
        var actualStatus = JsonSerializer.Deserialize<WorkerStatus>(statusJson);

        await Assert.That(actualRegistration).IsNotNull();
        await Assert.That(actualRegistration!.Capabilities).Contains((Capability) "Docker");
        await Assert.That(actualRegistration.RunId).IsEqualTo("run-1");
        await Assert.That(actualStatus).IsNotNull();
        await Assert.That(actualStatus!.RunId).IsEqualTo("run-1");
        await Assert.That(actualStatus!.UnattributedCommandCount).IsEqualTo(3);
    }

    [Test]
    public async Task SerializedModuleResult_Uses_Transport_Neutral_Payload_Name()
    {
        var result = new SerializedModuleResult
        {
            ModuleId = "BuildModule",
            WorkerId = WorkerId.FromInstanceIndex(1),
            Payload = "{}",
            CompletedAt = DateTimeOffset.UtcNow,
        };

        var json = JsonSerializer.Serialize(result);

        await Assert.That(json).Contains("\"Payload\":\"{}\"");
        await Assert.That(json).Contains("\"ModuleId\":\"BuildModule\"");
        await Assert.That(json).DoesNotContain("ResultTypeName");
        await Assert.That(json).DoesNotContain("SerializedJson");
    }

    [Test]
    public async Task SerializedModuleResult_RoundTrips_ExecutionTelemetry()
    {
        var now = DateTimeOffset.UtcNow;
        var expected = new SerializedModuleResult
        {
            ModuleId = "BuildModule",
            WorkerId = WorkerId.FromInstanceIndex(1),
            Payload = "{}",
            CompletedAt = now,
            ExecutionTelemetry = new DistributedModuleExecutionTelemetry
            {
                ClaimedAt = now.AddSeconds(-4),
                ExecutionStartedAt = now.AddSeconds(-3),
                ExecutionFinishedAt = now.AddSeconds(-1),
                DependencyResultTransferDuration = TimeSpan.FromMilliseconds(50),
                DependencyResultProcessingDuration = TimeSpan.FromMilliseconds(100),
                ArtifactDownloadDuration = TimeSpan.FromMilliseconds(200),
                ArtifactUploadDuration = TimeSpan.FromMilliseconds(300),
            },
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<SerializedModuleResult>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(actual!.ExecutionTelemetry).IsEqualTo(expected.ExecutionTelemetry);
    }

    [Test]
    public async Task WorkerRegistration_Rejects_Default_Capabilities()
    {
        var registration = new WorkerRegistration { WorkerId = WorkerId.FromInstanceIndex(1), Capabilities = [default], RegisteredAt = DateTimeOffset.UtcNow };

        await Assert.That(() => JsonSerializer.Serialize(registration))
            .Throws<JsonException>();
    }
}
