using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class FormattedLogValuesObfuscatorTests
{
    [Test]
    public async Task TryObfuscateValues_DoesNotInspectStateWhenNoSecretsAreRegistered()
    {
        var state = new ThrowingToStringState();
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(false);

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object)
            .TryObfuscateValues(state);

        await Assert.That(obfuscatedState).IsSameReferenceAs(state);
        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
    }

    [Test]
    public async Task TryObfuscateValues_MasksSecretsInOriginalFormat()
    {
        const string secret = "literal-secret";
        var logger = new Mock<ILogger>();
        logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
        logger.Object.LogDebug("Token literal-secret for {Resource}", "repository");

        var state = logger.Invocations.Single(x => x.Method.Name == nameof(ILogger.Log)).Arguments[2];
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => (value ?? string.Empty).Replace(secret, "********", StringComparison.Ordinal));

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["{OriginalFormat}"]).IsEqualTo("Token ******** for {Resource}");
        await Assert.That(properties["Resource"]).IsTypeOf<string>();
        await Assert.That(properties["Resource"]).IsEqualTo("repository");
    }

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
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
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
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
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
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value == "secret" ? "********" : value ?? string.Empty);

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object).TryObfuscateValues(state);
        var properties = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)
            .ToDictionary(x => x.Key, x => x.Value);

        await Assert.That(properties["ModuleName"]).IsEqualTo("********");
    }

    [Test]
    public async Task TryObfuscateValues_MasksUnstructuredState()
    {
        const string secret = "plain-state-secret";
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) =>
                (value ?? string.Empty).Replace(secret, "********", StringComparison.Ordinal));

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object)
            .TryObfuscateValues($"Value: {secret}");

        await Assert.That(obfuscatedState).IsEqualTo("Value: ********");
    }

    [Test]
    public async Task TryObfuscateValues_PreservesStateWhenToStringThrows()
    {
        var state = new ThrowingToStringState();
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object)
            .TryObfuscateValues(state);

        await Assert.That(obfuscatedState).IsSameReferenceAs(state);
        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
    }

    [Test]
    public async Task TryObfuscateValues_DoesNotRescanPreObfuscatedValues()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        var state = new[]
        {
            new KeyValuePair<string, object?>(
                "CommandOutput",
                new PreObfuscatedLogValue("already-masked")),
        };

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object)
            .TryObfuscateValues(state);
        var value = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)[0].Value;

        await Assert.That(value).IsEqualTo("already-masked");
        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
    }

    [Test]
    public async Task TryObfuscateValues_UnwrapsPreObfuscatedValuesWithoutSecrets()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(false);
        var state = new[]
        {
            new KeyValuePair<string, object?>(
                "CommandOutput",
                new PreObfuscatedLogValue("already-masked")),
        };

        var obfuscatedState = new FormattedLogValuesObfuscator(secretObfuscator.Object)
            .TryObfuscateValues(state);
        var value = ((IReadOnlyList<KeyValuePair<string, object?>>) obfuscatedState)[0].Value;

        await Assert.That(value).IsEqualTo("already-masked");
        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
    }

    private sealed class ThrowingToStringState
    {
        public override string ToString() => throw new InvalidOperationException("Cannot format state.");
    }
}
