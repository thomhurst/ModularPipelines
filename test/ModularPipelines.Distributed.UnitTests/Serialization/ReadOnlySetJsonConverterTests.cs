using System.Text.Json;
using ModularPipelines.Distributed.Serialization;

namespace ModularPipelines.Distributed.UnitTests.Serialization;

public class ReadOnlySetJsonConverterTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new ReadOnlySetJsonConverter() },
    };

    [Test]
    public async Task RoundTrip_PreservesValuesAndCaseInsensitiveComparison()
    {
        IReadOnlySet<Capability> expected = new HashSet<Capability> { "Docker", "GPU" };

        var json = JsonSerializer.Serialize(expected, JsonOptions);
        var actual = JsonSerializer.Deserialize<IReadOnlySet<Capability>>(json, JsonOptions);

        await Assert.That(actual).IsNotNull();
        await Assert.That(actual!).IsEquivalentTo(expected);
        await Assert.That(actual.Contains("docker")).IsTrue();
    }

    [Test]
    public async Task ModuleAssignment_RoundTrips_With_DefaultOptions()
    {
        var expected = new ModuleAssignment(
            "BuildModule",
            "System.String",
            new HashSet<Capability> { "Docker" },
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfiguration(null, false))
        {
            SatisfiedConditionGroups = ["Conditions.CrossPlatform"],
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<ModuleAssignment>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(json).DoesNotContain("MatrixTarget");
        await Assert.That(actual!.RequiredCapabilities.Contains("docker")).IsTrue();
        await Assert.That(actual.SatisfiedConditionGroups).Contains("Conditions.CrossPlatform");
    }

    [Test]
    public async Task WorkerRegistration_RoundTrips_With_DefaultOptions()
    {
        var expected = new WorkerRegistration(
            1,
            new HashSet<Capability> { "Docker" },
            DateTimeOffset.UtcNow)
        {
            UnattributedCommandCount = 3,
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<WorkerRegistration>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(actual!.Capabilities.Contains("docker")).IsTrue();
        await Assert.That(actual.UnattributedCommandCount).IsEqualTo(3);
    }

    [Test]
    public async Task WorkerRegistration_Rejects_Default_Capabilities()
    {
        var registration = new WorkerRegistration(
            1,
            new HashSet<Capability> { default },
            DateTimeOffset.UtcNow);

        await Assert.That(() => JsonSerializer.Serialize(registration))
            .Throws<JsonException>();
    }
}
