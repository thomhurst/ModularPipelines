using ModularPipelines.Context;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ConsoleCoordinatorTests
{
    [Test]
    public async Task DisabledProgress_CompletesWithoutWaitingForCancellation()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        await using var coordinator = CreateCoordinator(
            outputCoordinator.Object,
            new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ShowProgress = false },
            });
        using var cancellation = new CancellationTokenSource();
        var progress = ((IProgressDisplay) coordinator).RunAsync(
            new OrganizedModules([], []),
            cancellation.Token);

        try
        {
            // A no-op display must not make pipeline shutdown wait for the five-second grace period.
            await Assert.That(progress.IsCompletedSuccessfully).IsTrue();
            outputCoordinator.Verify(output => output.SetProgressActive(false), Times.Once);
        }
        finally
        {
            await cancellation.CancelAsync();
            await progress;
        }
    }

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

        try
        {
            coordinator.Install();
            coordinator.EnableOutputBuffering();
            using (new ModuleOutputContextScope(typeof(ConsoleCoordinatorTests)))
            {
                System.Console.Write("retained fragment");
            }

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

    [Test]
    public async Task BufferThreshold_RequestsImmediateIncrementalFlush()
    {
        var flushRequested = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var outputCoordinator = new Mock<IOutputCoordinator>();
        outputCoordinator
            .Setup(output => output.EnqueueAndFlushAsync(
                It.IsAny<IModuleOutputBuffer>(),
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()))
            .Callback(() => flushRequested.TrySetResult())
            .Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(
            outputCoordinator.Object,
            new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ModuleOutputFlushThreshold = 2 },
            });
        var buffer = coordinator.GetModuleBuffer(typeof(ConsoleCoordinatorTests));

        buffer.WriteLine("first");
        outputCoordinator.Verify(
            output => output.EnqueueAndFlushAsync(
                It.IsAny<IModuleOutputBuffer>(),
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()),
            Times.Never);

        buffer.WriteLine("second");
        await flushRequested.Task.WaitAsync(TimeSpan.FromSeconds(1));

        outputCoordinator.Verify(
            output => output.EnqueueAndFlushAsync(
                buffer,
                OutputFlushKind.Incremental,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Install_ConfiguresInteractiveCapability_AndDisposeRestoresConsole(bool showProgress)
    {
        var originalConsole = AnsiConsole.Console;
        var coordinator = CreateCoordinator(
            Mock.Of<IOutputCoordinator>(),
            new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ShowProgress = showProgress },
            });

        try
        {
            coordinator.Install();

            await Assert.That(AnsiConsole.Profile.Capabilities.Interactive).IsEqualTo(showProgress);
        }
        finally
        {
            await coordinator.DisposeAsync();
        }

        await Assert.That(AnsiConsole.Console).IsSameReferenceAs(originalConsole);
    }

    [Test]
    public async Task GetModuleBuffer_DoesNotUseDisposedLoggerFactory()
    {
        var loggerFactory = LoggerFactory.Create(static _ => { });
        var coordinator = CreateCoordinator(
            Mock.Of<IOutputCoordinator>(),
            new PipelineOptions
            {
                RunReport = new RunReportOptions { IncludeModuleOutput = true },
            },
            loggerFactory);
        loggerFactory.Dispose();

        var buffer = coordinator.GetModuleBuffer(typeof(ConsoleCoordinatorTests));

        await Assert.That(buffer).IsNotNull();
    }

    private static ConsoleCoordinator CreateCoordinator(
        IOutputCoordinator outputCoordinator,
        PipelineOptions? options = null,
        ILoggerFactory? loggerFactory = null)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Secrets).Returns([]);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(obfuscator => obfuscator.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var nonSpectreLoggerFactory = new Mock<INonSpectreLoggerFactory>();
        nonSpectreLoggerFactory
            .Setup(factory => factory.CreateLoggers(It.IsAny<string>()))
            .Returns([]);
        var loggerControl = new Mock<ISpectreConsoleLoggerControl>();
        loggerControl.SetupGet(control => control.SynchronizationLock).Returns(new object());
        loggerControl
            .Setup(control => control.WouldRender(It.IsAny<string>(), It.IsAny<LogLevel>()))
            .Returns(true);
        loggerControl
            .Setup(control => control.TryAcquireRenderGateAsync(
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<IDisposable?>(Mock.Of<IDisposable>()));

        return new ConsoleCoordinator(
            Mock.Of<IBuildSystemFormatterProvider>(),
            Mock.Of<IResultsPrinter>(),
            secretObfuscator.Object,
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(options ?? new PipelineOptions()),
            loggerFactory ?? NullLoggerFactory.Instance,
            Mock.Of<IBuildSystemDetector>(),
            Mock.Of<IServiceProvider>(),
            outputCoordinator,
            loggerControl.Object,
            nonSpectreLoggerFactory.Object,
            DelegatingAnsiConsole.Instance);
    }
}
