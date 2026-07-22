using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class FormattedLogValuesObfuscatorTests
{
    [Test]
    public async Task TryObfuscateValues_PreservesStructuredValueTypes()
    {
        var startTime = DateTimeOffset.UtcNow;
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        if (logger.Object.IsEnabled(LogLevel.Debug))
        {
            logger.Object.LogDebug("Started at {StartTime:O} with {Secret}", startTime, "secret");
        }

        var state = logger.Invocations.Single(x => x.Method.Name == nameof(ILogger.Log)).Arguments[2];
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value == "secret" ? "********" : value ?? string.Empty);

        new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) state)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["StartTime"]).IsTypeOf<DateTimeOffset>();
        await Assert.That(properties["StartTime"]).IsEqualTo(startTime);
        await Assert.That(properties["Secret"]).IsEqualTo("********");
    }
}
