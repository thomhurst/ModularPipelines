using System.Collections;
using Microsoft.Extensions.Logging;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

/// <summary>
/// Tests for <see cref="PipelineLevelLogger"/>.
/// </summary>
public class PipelineLevelLoggerTests
{
    [Test]
    public void Log_DelegatesToUnderlyingLogger()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Information)).Returns(true);
        var pipelineLevelLogger = CreateLogger(mockLogger.Object);
        var eventId = new EventId(1, "TestEvent");
        const string message = "Test message";

        // Act
        pipelineLevelLogger.Log(LogLevel.Information, eventId, message, null, (s, _) => s);

        // Assert
        mockLogger.Verify(x => x.Log(
            LogLevel.Information,
            eventId,
            message,
            null,
            It.IsAny<Func<string, Exception?, string>>()), Times.Once);
    }

    [Test]
    public void Log_DoesNotObfuscateWhenDisabled()
    {
        var mockLogger = new Mock<ILogger>();
        var secretObfuscator = new Mock<ISecretObfuscator>();
        var pipelineLevelLogger = new PipelineLevelLogger(
            mockLogger.Object,
            secretObfuscator.Object,
            new FormattedLogValuesObfuscator(secretObfuscator.Object));

        pipelineLevelLogger.LogError(
            new InvalidOperationException("secret"),
            "Token {Token}",
            "secret");

        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
        mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Test]
    public async Task Log_PreservesNullState()
    {
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(underlyingLogger);

        pipelineLevelLogger.Log<string?>(
            LogLevel.Information,
            new EventId(2, "NullState"),
            null,
            null,
            static (state, _) => state ?? "null-state");

        using (Assert.Multiple())
        {
            await Assert.That(underlyingLogger.State).IsNull();
            await Assert.That(underlyingLogger.Message).IsEqualTo("null-state");
        }
    }

    [Test]
    public async Task IsEnabled_DelegatesToUnderlyingLogger()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(true);
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(false);
        var pipelineLevelLogger = CreateLogger(mockLogger.Object);

        // Act & Assert
        await Assert.That(pipelineLevelLogger.IsEnabled(LogLevel.Warning)).IsTrue();
        await Assert.That(pipelineLevelLogger.IsEnabled(LogLevel.Trace)).IsFalse();
    }

    [Test]
    public async Task BeginScope_DelegatesToUnderlyingLogger()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var expectedScope = new Mock<IDisposable>();
        mockLogger.Setup(x => x.BeginScope("test scope")).Returns(expectedScope.Object);
        var pipelineLevelLogger = CreateLogger(mockLogger.Object);

        // Act
        var scope = pipelineLevelLogger.BeginScope("test scope");

        // Assert
        await Assert.That(scope).IsEqualTo(expectedScope.Object);
    }

    [Test]
    public void Dispose_DoesNotThrow()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        var pipelineLevelLogger = CreateLogger(mockLogger.Object);

        // Act & Assert - should not throw
        pipelineLevelLogger.Dispose();
    }

    [Test]
    public async Task Log_ObfuscatesStateMessageAndExceptionBeforeDelegating()
    {
        const string secret = "pipeline-secret";
        var underlyingLogger = new RecordingLogger();
        var originalException = new InvalidOperationException($"Failure: {secret}");
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);

        pipelineLevelLogger.LogError(
            originalException,
            "Token {Token}",
            secret);

        await Assert.That(underlyingLogger.State?.ToString()).DoesNotContain(secret);
        await Assert.That(underlyingLogger.Message).DoesNotContain(secret);
        await Assert.That(underlyingLogger.Exception).IsNotSameReferenceAs(originalException);
        await Assert.That(underlyingLogger.Exception?.ToString()).DoesNotContain(secret);
    }

    [Test]
    public async Task Log_PreservesOriginalExceptionWhenNoSecretsAreRegistered()
    {
        var underlyingLogger = new RecordingLogger();
        var originalException = new InvalidOperationException("Failure");
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(false);
        var pipelineLevelLogger = new PipelineLevelLogger(
            underlyingLogger,
            secretObfuscator.Object,
            new FormattedLogValuesObfuscator(secretObfuscator.Object));

        pipelineLevelLogger.LogError(originalException, "Failure");

        await Assert.That(underlyingLogger.Exception).IsSameReferenceAs(originalException);
        secretObfuscator.Verify(
            x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()),
            Times.Never);
    }

    [Test]
    public async Task Log_PreservesSanitizedExceptionDiagnostics()
    {
        const string secret = "pipeline-secret";
        var underlyingLogger = new RecordingLogger();
        var innerException = new ArgumentException($"Inner: {secret}");
        var originalException = CaptureException(
            new InvalidOperationException($"Outer: {secret}", innerException));
        originalException.HelpLink = $"https://example.invalid/{secret}";
        originalException.Source = $"source-{secret}";
        originalException.Data[$"key-{secret}"] = $"value-{secret}";
        originalException.Data["number"] = 42;
        originalException.Data["hostile"] = new ThrowingToStringValue();
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);

        pipelineLevelLogger.LogError(originalException, "Failure");

        var exception = underlyingLogger.Exception;
        using (Assert.Multiple())
        {
            await Assert.That(exception?.StackTrace).Contains(nameof(CaptureException));
            await Assert.That(exception?.StackTrace).DoesNotContain(secret);
            await Assert.That(exception?.TargetSite).IsEqualTo(originalException.TargetSite);
            await Assert.That(exception?.InnerException).IsNotNull();
            await Assert.That(exception?.InnerException?.Message).IsEqualTo("Inner: ********");
            await Assert.That(exception?.InnerException).IsNotSameReferenceAs(innerException);
            await Assert.That(exception?.HelpLink).IsEqualTo("https://example.invalid/********");
            await Assert.That(exception?.Source).IsEqualTo("source-********");
            await Assert.That(exception?.Data["key-********"]).IsEqualTo("value-********");
            await Assert.That(exception?.Data["number"]).IsEqualTo(42);
            await Assert.That(exception?.Data["hostile"]).IsEqualTo(LoggingConstants.SecretMask);
        }
    }

    [Test]
    public async Task Log_GuardsHostileExceptionDiagnostics()
    {
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(underlyingLogger);

        await Assert.That(
                () => pipelineLevelLogger.LogError(new ThrowingDiagnosticException(), "Failure"))
            .ThrowsNothing();

        using (Assert.Multiple())
        {
            await Assert.That(underlyingLogger.Exception?.Message)
                .IsEqualTo(LoggingConstants.SecretMask);
            await Assert.That(underlyingLogger.Exception?.ToString())
                .Contains(nameof(ThrowingDiagnosticException));
        }
    }

    [Test]
    public async Task Log_GuardsHostileStructuredTraversal()
    {
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(underlyingLogger);

        await Assert.That(() => pipelineLevelLogger.Log(
                LogLevel.Information,
                new EventId(3, "HostileState"),
                new ThrowingCountStructuredState(),
                null,
                static (_, _) => throw new InvalidOperationException("Cannot format state.")))
            .ThrowsNothing();

        using (Assert.Multiple())
        {
            await Assert.That(underlyingLogger.State).IsEqualTo(LoggingConstants.SecretMask);
            await Assert.That(underlyingLogger.Message).IsEqualTo(LoggingConstants.SecretMask);
        }
    }

    [Test]
    public async Task Log_PreservesEverySanitizedAggregateExceptionBranch()
    {
        const string secret = "pipeline-secret";
        var underlyingLogger = new RecordingLogger();
        var firstException = new InvalidOperationException($"First: {secret}");
        var secondException = new ArgumentException($"Second: {secret}");
        var originalException = new AggregateException(
            $"Aggregate: {secret}",
            firstException,
            secondException);
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);

        pipelineLevelLogger.LogError(originalException, "Failure");

        var aggregateException = underlyingLogger.Exception as AggregateException;
        using (Assert.Multiple())
        {
            await Assert.That(aggregateException).IsNotNull();
            await Assert.That(aggregateException!.InnerExceptions).Count().IsEqualTo(2);
            await Assert.That(aggregateException.InnerExceptions[0].Message).IsEqualTo("First: ********");
            await Assert.That(aggregateException.InnerExceptions[1].Message).IsEqualTo("Second: ********");
            await Assert.That(aggregateException.InnerExceptions[0]).IsNotSameReferenceAs(firstException);
            await Assert.That(aggregateException.InnerExceptions[1]).IsNotSameReferenceAs(secondException);
        }
    }

    [Test]
    public async Task BeginScope_ObfuscatesStateBeforeDelegating()
    {
        const string secret = "scope-secret";
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);

        pipelineLevelLogger.BeginScope($"Scope: {secret}");

        await Assert.That(underlyingLogger.Scope?.ToString()).IsEqualTo("Scope: ********");
    }

    [Test]
    public async Task BeginScope_PreservesStructuredFormattedRenderingAfterObfuscation()
    {
        const string secret = "scope-secret";
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);

        pipelineLevelLogger.BeginScope("Token {Token}", secret);

        await Assert.That(underlyingLogger.Scope?.ToString()).IsEqualTo("Token ********");
        var structuredScope = underlyingLogger.Scope
            as IReadOnlyList<KeyValuePair<string, object?>>;
        await Assert.That(structuredScope).IsNotNull();
        await Assert.That(structuredScope![0].Value?.ToString()).IsEqualTo("********");
    }

    [Test]
    public async Task BeginScope_PreservesStructuredStateWhenRenderingThrows()
    {
        const string secret = "scope-secret";
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(
            underlyingLogger,
            value => value?.Replace(secret, "********", StringComparison.Ordinal) ?? string.Empty);
        var state = new ThrowingStructuredScopeState("Token", secret);

        await Assert.That(() => pipelineLevelLogger.BeginScope(state)).ThrowsNothing();

        var structuredScope = underlyingLogger.Scope
            as IReadOnlyList<KeyValuePair<string, object?>>;
        await Assert.That(structuredScope).IsNotNull();
        await Assert.That(structuredScope![0].Value).IsEqualTo("********");
        await Assert.That(underlyingLogger.Scope?.ToString()).IsEqualTo(LoggingConstants.SecretMask);
    }

    [Test]
    public async Task BeginScope_GuardsHostileStructuredTraversal()
    {
        var underlyingLogger = new RecordingLogger();
        var pipelineLevelLogger = CreateLogger(underlyingLogger);

        await Assert.That(
                () => pipelineLevelLogger.BeginScope(new ThrowingCountStructuredState()))
            .ThrowsNothing();

        await Assert.That(underlyingLogger.Scope).IsEqualTo(LoggingConstants.SecretMask);
    }

    private static PipelineLevelLogger CreateLogger(
        ILogger logger,
        Func<string?, string>? obfuscate = null)
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => obfuscate?.Invoke(value) ?? value ?? string.Empty);

        return new PipelineLevelLogger(
            logger,
            secretObfuscator.Object,
            new FormattedLogValuesObfuscator(secretObfuscator.Object));
    }

    private static Exception CaptureException(Exception exception)
    {
        try
        {
            throw exception;
        }
        catch (Exception captured)
        {
            return captured;
        }
    }

    private sealed class ThrowingStructuredScopeState(
        string key,
        object? value) : IReadOnlyList<KeyValuePair<string, object?>>
    {
        private readonly KeyValuePair<string, object?>[] _values =
        [
            new(key, value),
        ];

        public int Count => _values.Length;

        public KeyValuePair<string, object?> this[int index] => _values[index];

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
            ((IEnumerable<KeyValuePair<string, object?>>) _values).GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public override string ToString() => throw new InvalidOperationException("Cannot format scope.");
    }

    private sealed class ThrowingCountStructuredState
        : IReadOnlyList<KeyValuePair<string, object?>>
    {
        public int Count => throw new InvalidOperationException("Cannot count scope values.");

        public KeyValuePair<string, object?> this[int index] =>
            throw new InvalidOperationException("Cannot read scope value.");

        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
            throw new InvalidOperationException("Cannot enumerate scope values.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private sealed class ThrowingToStringValue
    {
        public override string ToString() => throw new InvalidOperationException("Cannot format value.");
    }

    private sealed class ThrowingDiagnosticException : Exception
    {
        public override string Message => throw new InvalidOperationException("Cannot read message.");

        public override IDictionary Data => throw new InvalidOperationException("Cannot read data.");

        public override string? HelpLink
        {
            get => throw new InvalidOperationException("Cannot read help link.");
            set => base.HelpLink = value;
        }

        public override string? Source
        {
            get => throw new InvalidOperationException("Cannot read source.");
            set => base.Source = value;
        }

        public override string? StackTrace =>
            throw new InvalidOperationException("Cannot read stack trace.");

        public override string ToString() => throw new InvalidOperationException("Cannot format exception.");
    }

    private sealed class RecordingLogger : ILogger
    {
        public object? State { get; private set; }

        public object? Scope { get; private set; }

        public string? Message { get; private set; }

        public Exception? Exception { get; private set; }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            Scope = state;
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            State = state;
            Exception = exception;
            Message = formatter(state, exception);
        }
    }
}
