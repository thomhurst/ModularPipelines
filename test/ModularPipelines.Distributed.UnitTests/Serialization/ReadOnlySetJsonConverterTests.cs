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
}
