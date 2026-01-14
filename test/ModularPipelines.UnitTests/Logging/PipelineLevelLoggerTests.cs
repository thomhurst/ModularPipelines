using Microsoft.Extensions.Logging;
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
        var pipelineLevelLogger = new PipelineLevelLogger(mockLogger.Object);
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
    public async Task IsEnabled_DelegatesToUnderlyingLogger()
    {
        // Arrange
        var mockLogger = new Mock<ILogger>();
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(true);
        mockLogger.Setup(x => x.IsEnabled(LogLevel.Trace)).Returns(false);
        var pipelineLevelLogger = new PipelineLevelLogger(mockLogger.Object);

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
        var pipelineLevelLogger = new PipelineLevelLogger(mockLogger.Object);

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
        var pipelineLevelLogger = new PipelineLevelLogger(mockLogger.Object);

        // Act & Assert - should not throw
        pipelineLevelLogger.Dispose();
    }
}
