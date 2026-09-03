using System.Text.Json;
namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class DistributedDtoSerializationTests
{
    [Test]
    public async Task ModuleAssignment_RoundTrips_With_DefaultOptions()
    {
        var expected = new ModuleAssignment(
            "BuildModule",
            "System.String",
            ["Docker"],
            DateTimeOffset.UtcNow,
            new ModuleAssignmentOptions(null, false))
        {
            SatisfiedConditionGroups = ["Conditions.CrossPlatform"],
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<ModuleAssignment>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(json).Contains("\"RequiredCapabilities\":[");
        await Assert.That(json).DoesNotContain("MatrixTarget");
        await Assert.That(actual!.RequiredCapabilities).Contains((Capability) "Docker");
        await Assert.That(actual.SatisfiedConditionGroups).Contains("Conditions.CrossPlatform");
    }

    [Test]
    public async Task WorkerDtos_RoundTrip_With_DefaultOptions()
    {
        var registration = new WorkerRegistration(
            1,
            ["Docker"],
            DateTimeOffset.UtcNow)
        {
            RunIdentifier = "run-1",
        };
        var status = new WorkerStatus(1)
        {
            RunIdentifier = "run-1",
            UnattributedCommandCount = 3,
        };

        var registrationJson = JsonSerializer.Serialize(registration);
        var statusJson = JsonSerializer.Serialize(status);
        var actualRegistration = JsonSerializer.Deserialize<WorkerRegistration>(registrationJson);
        var actualStatus = JsonSerializer.Deserialize<WorkerStatus>(statusJson);

        await Assert.That(actualRegistration).IsNotNull();
        await Assert.That(actualRegistration!.Capabilities).Contains((Capability) "Docker");
        await Assert.That(actualRegistration.RunIdentifier).IsEqualTo("run-1");
        await Assert.That(actualStatus).IsNotNull();
        await Assert.That(actualStatus!.RunIdentifier).IsEqualTo("run-1");
        await Assert.That(actualStatus!.UnattributedCommandCount).IsEqualTo(3);
    }

    [Test]
    public async Task SerializedModuleResult_Uses_Transport_Neutral_Payload_Name()
    {
        var result = new SerializedModuleResult(
            "BuildModule",
            "System.String",
            1,
            "{}",
            DateTimeOffset.UtcNow);

        var json = JsonSerializer.Serialize(result);

        await Assert.That(json).Contains("\"Payload\":\"{}\"");
        await Assert.That(json).DoesNotContain("SerializedJson");
    }

    [Test]
    public async Task SerializedModuleResult_RoundTrips_ExecutionTelemetry()
    {
        var now = DateTimeOffset.UtcNow;
        var expected = new SerializedModuleResult(
            "BuildModule",
            "System.String",
            1,
            "{}",
            now)
        {
            ExecutionTelemetry = new DistributedModuleExecutionTelemetry
            {
                ClaimedAt = now.AddSeconds(-4),
                ExecutionStartedAt = now.AddSeconds(-3),
                ExecutionFinishedAt = now.AddSeconds(-1),
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
        var registration = new WorkerRegistration(
            1,
            [default],
            DateTimeOffset.UtcNow);

        await Assert.That(() => JsonSerializer.Serialize(registration))
            .Throws<JsonException>();
    }
}
