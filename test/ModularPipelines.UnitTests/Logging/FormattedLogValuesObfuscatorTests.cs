using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class FormattedLogValuesObfuscatorTests
{
    [Test]
    public async Task TryObfuscateValues_PreservesUnmaskedStructuredValueTypes()
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

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["StartTime"]).IsTypeOf<DateTimeOffset>();
        await Assert.That(properties["StartTime"]).IsEqualTo(startTime);
        await Assert.That(properties["Secret"]).IsEqualTo("********");
    }

    [Test]
    public async Task TryObfuscateValues_MasksValueTypeSecrets()
    {
        var secret = Guid.NewGuid();
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        logger.Object.LogDebug("Token: {Token}", secret);

        var state = logger.Invocations.Single(x => x.Method.Name == nameof(ILogger.Log)).Arguments[2];
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value == secret.ToString() ? "********" : value ?? string.Empty);

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["Token"]).IsEqualTo("********");
    }

    [Test]
    public async Task TryObfuscateValues_MasksCustomStructuredLogStates()
    {
        var state = new ModuleCompletionLogState("secret", TimeSpan.FromSeconds(1), "(none)", 0, 0);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value == "secret" ? "********" : value ?? string.Empty);

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["ModuleName"]).IsEqualTo("********");
    }
}
