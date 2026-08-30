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
        IReadOnlySet<string> expected = new HashSet<string> { "Docker", "GPU" };

        var json = JsonSerializer.Serialize(expected, JsonOptions);
        var actual = JsonSerializer.Deserialize<IReadOnlySet<string>>(json, JsonOptions);

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
            new HashSet<string> { "Docker" },
            null,
            DateTimeOffset.UtcNow,
            new ModuleAssignmentConfig(null, 0, false))
        {
            SatisfiedConditionGroups = ["Conditions.CrossPlatform"],
        };

        var json = JsonSerializer.Serialize(expected);
        var actual = JsonSerializer.Deserialize<ModuleAssignment>(json);

        await Assert.That(actual).IsNotNull();
        await Assert.That(actual!.RequiredCapabilities.Contains("docker")).IsTrue();
        await Assert.That(actual.SatisfiedConditionGroups).Contains("Conditions.CrossPlatform");
    }

    [Test]
    public async Task WorkerRegistration_RoundTrips_With_DefaultOptions()
    {
        var expected = new WorkerRegistration(
            1,
            new HashSet<string> { "Docker" },
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
}
