using ModularPipelines.Context;
using ModularPipelines.Reporting;
using ModularPipelines.Secrets;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Engine.Executors;
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
    [Arguments(false, false, false)]
    [Arguments(true, false, false)]
    [Arguments(false, false, true)]
    [Arguments(true, false, true)]
    [Arguments(false, true, false)]
    [Arguments(true, true, false)]
    [Arguments(false, true, true)]
    [Arguments(true, true, true)]
    public async Task OverlappingCoordinators_RestoreOnlyLiveWriters(
        bool uninstallFirst, bool firstScoped, bool secondScoped)
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var originalAnsi = AnsiConsole.Console;
        var first = CreateCoordinator(Mock.Of<IOutputCoordinator>(), ansiConsole: CreateAnsiConsole(firstScoped));
        var second = CreateCoordinator(Mock.Of<IOutputCoordinator>(), ansiConsole: CreateAnsiConsole(secondScoped));

        try
        {
            first.Install();
            var firstOut = System.Console.Out;
            var firstError = System.Console.Error;
            var firstAnsi = AnsiConsole.Console;
            second.Install();
            var secondOut = System.Console.Out;
            var secondError = System.Console.Error;
            var secondAnsi = AnsiConsole.Console;

            await (uninstallFirst ? first : second).DisposeAsync();

            await Assert.That(System.Console.Out).IsSameReferenceAs(uninstallFirst ? secondOut : firstOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(uninstallFirst ? secondError : firstError);
            var expectedAnsi = uninstallFirst
                ? (secondScoped ? originalAnsi : secondAnsi)
                : (firstScoped ? originalAnsi : firstAnsi);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(expectedAnsi);

            await (uninstallFirst ? second : first).DisposeAsync();

            await Assert.That(System.Console.Out).IsSameReferenceAs(originalOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(originalError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(originalAnsi);
        }
        finally
        {
            second.Uninstall();
            first.Uninstall();
#pragma warning disable TUnit0055 // Restore process-wide streams even when the regression fails.
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
#pragma warning restore TUnit0055
            AnsiConsole.Console = originalAnsi;
        }
    }

    [Test]
    public async Task LaterInstallation_WritesToHostStreamsAfterFirstOwnerUninstalls()
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        using var output = new StringWriter();
        using var error = new StringWriter();
        var first = CreateCoordinator(Mock.Of<IOutputCoordinator>());
        var second = CreateCoordinator(Mock.Of<IOutputCoordinator>());
        var third = CreateCoordinator(Mock.Of<IOutputCoordinator>());

        try
        {
#pragma warning disable TUnit0055 // Globally non-parallel test restores both streams in finally.
            System.Console.SetOut(output);
            System.Console.SetError(error);
#pragma warning restore TUnit0055
            var hostOut = System.Console.Out;
            var hostError = System.Console.Error;
            first.Install();
            first.EnableOutputBuffering();
            second.Install();
            second.EnableOutputBuffering();
            first.Uninstall();
            third.Install();

            System.Console.WriteLine("host stdout");
            System.Console.Error.WriteLine("host stderr");
            AnsiConsole.Write(new Text("host spectre"));

            await Assert.That(output.ToString()).Contains("host stdout");
            await Assert.That(output.ToString()).Contains("host spectre");
            await Assert.That(error.ToString()).Contains("host stderr");
            await Assert.That(second.GetUnattributedBuffer().HasOutput).IsFalse();

            // Also exercise removal of a middle owner and repeated uninstall.
            second.Uninstall();
            second.Uninstall();
            third.Uninstall();
            await Assert.That(System.Console.Out).IsSameReferenceAs(hostOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(hostError);
        }
        finally
        {
            third.Uninstall();
            second.Uninstall();
            first.Uninstall();
#pragma warning disable TUnit0055 // Restore the host's process-wide streams.
            System.Console.SetOut(originalOut);
            System.Console.SetError(originalError);
#pragma warning restore TUnit0055
        }
    }

    [Test]
    public async Task FailedInstallation_PreservesActiveOwnerAndCanBeRetried()
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var originalAnsi = AnsiConsole.Console;
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory.Setup(factory => factory.CreateLogger(OutputLoggerCategories.Pipeline))
            .Throws(new InvalidOperationException("logger installation failed"));
        var first = CreateCoordinator(Mock.Of<IOutputCoordinator>());
        var second = CreateCoordinator(Mock.Of<IOutputCoordinator>(), loggerFactory: loggerFactory.Object);

        try
        {
            first.Install();
            var activeOut = System.Console.Out;
            var activeError = System.Console.Error;
            var activeAnsi = AnsiConsole.Console;

            await Assert.That(() => second.Install()).Throws<InvalidOperationException>();

            await Assert.That(System.Console.Out).IsSameReferenceAs(activeOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(activeError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(activeAnsi);

            loggerFactory.Setup(factory => factory.CreateLogger(OutputLoggerCategories.Pipeline))
                .Returns(NullLogger.Instance);
            second.Install();
            first.Uninstall();
            second.Uninstall();

            await Assert.That(System.Console.Out).IsSameReferenceAs(originalOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(originalError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(originalAnsi);
        }
        finally
        {
            second.Uninstall();
            first.Uninstall();
        }
    }

    [Test]
    public async Task FailedDispose_ReleasesConsoleOwnership()
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var originalAnsi = AnsiConsole.Console;
        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.Setup(provider => provider.GetFormatter())
            .Throws(new InvalidOperationException("flush failed"));
        var first = CreateCoordinator(
            Mock.Of<IOutputCoordinator>(), formatterProvider: formatterProvider.Object);
        var second = CreateCoordinator(Mock.Of<IOutputCoordinator>());

        try
        {
            first.Install();
            second.Install();
            var activeOut = System.Console.Out;
            var activeError = System.Console.Error;
            var activeAnsi = AnsiConsole.Console;

            await Assert.That(async () => await first.DisposeAsync()).Throws<InvalidOperationException>();

            await Assert.That(System.Console.Out).IsSameReferenceAs(activeOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(activeError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(activeAnsi);
            second.Uninstall();

            await Assert.That(System.Console.Out).IsSameReferenceAs(originalOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(originalError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(originalAnsi);
        }
        finally
        {
            second.Uninstall();
            first.Uninstall();
        }
    }

    [Test]
    public async Task ConcurrentInstallAndUninstall_RestoresHostConsole()
    {
        var originalOut = System.Console.Out;
        var originalError = System.Console.Error;
        var originalAnsi = AnsiConsole.Console;
        var coordinators = Enumerable.Range(0, 8)
            .Select(_ => CreateCoordinator(Mock.Of<IOutputCoordinator>()))
            .ToArray();

        try
        {
            await Task.WhenAll(coordinators.Select(coordinator => Task.Run(coordinator.Install)));
            await Task.WhenAll(coordinators.Select(coordinator => Task.Run(coordinator.Uninstall)));

            await Assert.That(System.Console.Out).IsSameReferenceAs(originalOut);
            await Assert.That(System.Console.Error).IsSameReferenceAs(originalError);
            await Assert.That(AnsiConsole.Console).IsSameReferenceAs(originalAnsi);
        }
        finally
        {
            foreach (var coordinator in coordinators)
            {
                coordinator.Uninstall();
            }
        }
    }

    [Test]
    public async Task OverlappingOutputCoordinator_FlushesToHostWriter()
    {
        var originalOut = System.Console.Out;
        var first = CreateCoordinator(Mock.Of<IOutputCoordinator>());
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(output => output.ModuleType).Returns(typeof(void));
        buffer.Setup(output => output.FlushToAsync(
                originalOut, It.IsAny<IBuildSystemFormatter>(), It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(), OutputFlushKind.Complete,
                It.IsAny<IReadOnlyList<ILogger>?>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        try
        {
            first.Install();
            var output = new OutputCoordinator(
                Mock.Of<IBuildSystemFormatterProvider>(),
                NullLoggerFactory.Instance,
                Mock.Of<IServiceProvider>(),
                Mock.Of<ISpectreConsoleLoggerControl>(),
                Mock.Of<INonSpectreLoggerFactory>());
            first.Uninstall();

            await output.EnqueueAndFlushAsync(buffer.Object, OutputFlushKind.Complete);

            buffer.Verify(output => output.FlushToAsync(
                originalOut, It.IsAny<IBuildSystemFormatter>(), It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(), OutputFlushKind.Complete,
                It.IsAny<IReadOnlyList<ILogger>?>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            first.Uninstall();
        }
    }

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

    [Test]
    [Arguments(false, true)]
    [Arguments(true, true)]
    [Arguments(true, false)]
    public async Task Teardown_RecoversCanceledCompletionWithoutRepeatingOutputOrCompletingUnfinishedModules(
        bool renderedPrefix,
        bool pendingDiagnostics)
    {
        using var cancellation = new CancellationTokenSource();
        var capturedOutput = new StringWriter();
        var originalOutput = System.Console.Out;
        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.Setup(provider => provider.GetFormatter()).Returns(new DefaultFormatter());
        var loggerControl = new Mock<ISpectreConsoleLoggerControl>();
        loggerControl.SetupGet(control => control.SynchronizationLock).Returns(new object());
        loggerControl.Setup(control => control.TryAcquireRenderGateAsync(
                It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<IDisposable?>(Mock.Of<IDisposable>()));
        var nonSpectreLoggerFactory = new Mock<INonSpectreLoggerFactory>();
        nonSpectreLoggerFactory.Setup(factory => factory.CreateLoggers(It.IsAny<string>())).Returns([]);
        OutputCoordinator outputCoordinator;
        try
        {
#pragma warning disable TUnit0055 // Globally non-parallel test restores the writer in finally.
            System.Console.SetOut(capturedOutput);
#pragma warning restore TUnit0055
            outputCoordinator = new OutputCoordinator(
                formatterProvider.Object,
                NullLoggerFactory.Instance,
                Mock.Of<IServiceProvider>(),
                loggerControl.Object,
                nonSpectreLoggerFactory.Object);
        }
        finally
        {
#pragma warning disable TUnit0055 // Restore the process-wide writer before leaving setup.
            System.Console.SetOut(originalOutput);
#pragma warning restore TUnit0055
        }

        await using var consoleCoordinator = CreateCoordinator(outputCoordinator);
        var progress = new Mock<IPrintProgressExecutor>();
        progress.Setup(executor => executor.InitializeAsync()).ReturnsAsync(progress.Object);
        var pipelineOutput = new PipelineOutputCoordinator(
            progress.Object,
            Mock.Of<IConsolePrinter>(),
            Mock.Of<IInternalSummaryLogger>(),
            Mock.Of<IExceptionBuffer>(),
            consoleCoordinator,
            outputCoordinator,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions
            {
                Console = new PipelineConsoleOptions { ModuleOutputFlushInterval = TimeSpan.Zero },
            }),
            NullLogger<PipelineOutputCoordinator>.Instance,
            TimeProvider.System);
        var scope = await pipelineOutput.InitializeAsync();
        // Disabled progress switches completion output to the immediate queue.
        outputCoordinator.SetProgressActive(false);
        // A ready-event logger can preserve output while its module awaits rescheduling.
        var unfinishedBuffer = consoleCoordinator.GetModuleBuffer(typeof(double));
        unfinishedBuffer.Write("unfinished ready output");
        var buffer = consoleCoordinator.GetModuleBuffer(typeof(string));
        if (renderedPrefix)
        {
            buffer.WriteLine("rendered prefix");
            await outputCoordinator.EnqueueAndFlushAsync(buffer, OutputFlushKind.Incremental);
        }

        if (pendingDiagnostics)
        {
            buffer.WriteLine("remaining failure diagnostics");
        }

        buffer.SetException(new InvalidOperationException("module failed"));
        buffer.MarkComplete();
        var queueEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseQueue = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var controller = new Mock<IProgressController>();
        controller.Setup(controller => controller.PauseAsync())
            .Callback(() => queueEntered.TrySetResult())
            .Returns(releaseQueue.Task);
        controller.Setup(controller => controller.ResumeAsync()).Returns(Task.CompletedTask);
        await outputCoordinator.WaitForPendingFlushesAsync();
        outputCoordinator.SetProgressController(controller.Object);
        var earlierBuffer = consoleCoordinator.GetModuleBuffer(typeof(int));
        earlierBuffer.WriteLine("earlier completion");
        earlierBuffer.MarkComplete();
        var earlierFlush = outputCoordinator.OnModuleCompletedAsync(earlierBuffer, earlierBuffer.ModuleType);
        try
        {
            await queueEntered.Task.WaitAsync(TimeSpan.FromSeconds(10));
            var canceledFlush = outputCoordinator.OnModuleCompletedAsync(buffer, buffer.ModuleType, cancellation.Token);
            await cancellation.CancelAsync();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await canceledFlush);
            await Assert.That(capturedOutput.ToString()).DoesNotContain("remaining failure diagnostics");
        }
        finally
        {
            releaseQueue.TrySetResult();
            await earlierFlush;
            await outputCoordinator.WaitForPendingFlushesAsync();
        }

        await Assert.That(buffer.NeedsCompletionFlush).IsTrue();
        await scope.DisposeAsync();

        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
        await Assert.That(earlierBuffer.NeedsCompletionFlush).IsFalse();
        await Assert.That(unfinishedBuffer.IsComplete).IsFalse();
        await Assert.That(unfinishedBuffer.NeedsCompletionFlush).IsFalse();
        var text = capturedOutput.ToString();
        await Assert.That(text).DoesNotContain("Double \u2713");
        await Assert.That(text.Split("unfinished ready output").Length - 1).IsEqualTo(1);
        await Assert.That(text).Contains("InvalidOperationException");
        await Assert.That(text.Split("earlier completion").Length - 1).IsEqualTo(1);
        await Assert.That(text.Split("rendered prefix").Length - 1).IsEqualTo(renderedPrefix ? 1 : 0);
        await Assert.That(text.Split("remaining failure diagnostics").Length - 1).IsEqualTo(pendingDiagnostics ? 1 : 0);
    }

    private static ConsoleCoordinator CreateCoordinator(
        IOutputCoordinator outputCoordinator,
        PipelineOptions? options = null,
        ILoggerFactory? loggerFactory = null,
        IAnsiConsole? ansiConsole = null,
        IBuildSystemFormatterProvider? formatterProvider = null)
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
            formatterProvider ?? Mock.Of<IBuildSystemFormatterProvider>(),
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
            ansiConsole ?? DelegatingAnsiConsole.Instance);
    }

    private static IAnsiConsole CreateAnsiConsole(bool scoped) => scoped
        ? AnsiConsole.Create(new AnsiConsoleSettings { Out = new AnsiConsoleOutput(TextWriter.Null) })
        : DelegatingAnsiConsole.Instance;
}
