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
        var coordinator = CreateCoordinator(outputCoordinator.Object);
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

    [Test]
    public async Task PeriodicFlush_ContinuesAfterOneBufferFails()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        var coordinator = CreateCoordinator(outputCoordinator.Object);
        var failingBuffer = coordinator.GetModuleBuffer(typeof(string));
        var succeedingBuffer = coordinator.GetModuleBuffer(typeof(int));
        failingBuffer.WriteLine("fails");
        succeedingBuffer.WriteLine("succeeds");
        outputCoordinator
            .Setup(output => output.EnqueueAndFlushAsync(
                failingBuffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("sink failed"));
        outputCoordinator
            .Setup(output => output.EnqueueAndFlushAsync(
                succeedingBuffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await coordinator.FlushInProgressModuleOutputAsync();

        outputCoordinator.Verify(
            output => output.EnqueueAndFlushAsync(
                failingBuffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()),
            Times.Once);
        outputCoordinator.Verify(
            output => output.EnqueueAndFlushAsync(
                succeedingBuffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task PeriodicFlush_IncludesUnattributedOutput()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(output => output.EnqueueAndFlushAsync(
                It.IsAny<IModuleOutputBuffer>(),
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(outputCoordinator.Object);
        var unattributedBuffer = coordinator.GetUnattributedBuffer();
        unattributedBuffer.WriteLine("pipeline output");

        await coordinator.FlushInProgressModuleOutputAsync();

        outputCoordinator.Verify(
            output => output.EnqueueAndFlushAsync(
                unattributedBuffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    private static ConsoleCoordinator CreateCoordinator(IOutputCoordinator outputCoordinator)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Secrets).Returns([]);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(obfuscator => obfuscator.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);

        return new ConsoleCoordinator(
            Mock.Of<IBuildSystemFormatterProvider>(),
            Mock.Of<IResultsPrinter>(),
            secretObfuscator.Object,
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            Mock.Of<IBuildSystemDetector>(),
            Mock.Of<IServiceProvider>(),
            outputCoordinator,
            Mock.Of<ISpectreConsoleLoggerControl>());
    }
}
