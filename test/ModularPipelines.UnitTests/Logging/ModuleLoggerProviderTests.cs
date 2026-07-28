using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Logging;

public class ModuleLoggerProviderTests
{
    [Test]
    public async Task ProviderLifetimeEnd_DoesNotPreventAsyncLoggerFlush()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator
            .Setup(x => x.GetModuleBuffer(typeof(TestModule)))
            .Returns(buffer.Object);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.OnModuleCompletedAsync(
                buffer.Object,
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var logger = new ModuleLogger<TestModule>(
            NullLogger<TestModule>.Instance,
            Mock.Of<ISecretObfuscator>(),
            Mock.Of<IFormattedLogValuesObfuscator>(),
            consoleCoordinator.Object,
            outputCoordinator.Object);
        var provider = new ModuleLoggerProvider(
            Mock.Of<IServiceProvider>(),
            Mock.Of<IStackTraceModuleDetector>(),
            NullLoggerFactory.Instance);
        var previousLogger = ModuleLogger.Values.Value;

        try
        {
            ModuleLogger.Values.Value = logger;
            _ = provider.GetLogger();

            (provider as IDisposable)?.Dispose();
            await logger.DisposeAsync();

            outputCoordinator.Verify(
                x => x.OnModuleCompletedAsync(
                    buffer.Object,
                    typeof(TestModule),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            ModuleLogger.Values.Value = previousLogger;
            await logger.DisposeAsync();
        }
    }

    private sealed class TestModule;
}
