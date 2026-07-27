using MEL.Spectre;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ConsoleCoordinatorTests
{
    [Test]
    public async Task PeriodicFlush_SchedulesCompletedBufferPopulatedByRetainedWrite()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(output => output.OnModuleCompletedAsync(
                It.IsAny<IModuleOutputBuffer>(),
                It.IsAny<Type>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Secrets).Returns([]);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(obfuscator => obfuscator.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var coordinator = new ConsoleCoordinator(
            Mock.Of<IBuildSystemFormatterProvider>(),
            Mock.Of<IResultsPrinter>(),
            secretObfuscator.Object,
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            Mock.Of<IBuildSystemDetector>(),
            Mock.Of<IServiceProvider>(),
            outputCoordinator.Object,
            Mock.Of<ISpectreConsoleLoggerControl>());
        var previousModule = ModuleLogger.CurrentModuleType.Value;

        try
        {
            coordinator.Install();
            coordinator.EnableOutputBuffering();
            ModuleLogger.CurrentModuleType.Value = typeof(ConsoleCoordinatorTests);
            System.Console.Write("retained fragment");
            var buffer = coordinator.GetModuleBuffer(typeof(ConsoleCoordinatorTests));
            buffer.MarkComplete();

            await coordinator.FlushInProgressModuleOutputAsync();

            outputCoordinator.Verify(
                output => output.OnModuleCompletedAsync(
                    buffer,
                    typeof(ConsoleCoordinatorTests),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousModule;
            await coordinator.DisposeAsync();
        }
    }
}
