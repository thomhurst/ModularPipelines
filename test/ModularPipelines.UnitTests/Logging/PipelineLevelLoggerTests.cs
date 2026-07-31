using Microsoft.Extensions.Logging;
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

    private static PipelineLevelLogger CreateLogger(
        ILogger logger,
        Func<string?, string>? obfuscate = null)
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => obfuscate?.Invoke(value) ?? value ?? string.Empty);

        return new PipelineLevelLogger(
            logger,
            secretObfuscator.Object,
            new FormattedLogValuesObfuscator(secretObfuscator.Object));
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
