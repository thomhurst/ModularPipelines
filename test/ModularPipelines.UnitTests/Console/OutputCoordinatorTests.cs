using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using Moq;

namespace ModularPipelines.UnitTests.Console;

public class OutputCoordinatorTests
{
    [Test]
    public async Task ImmediateFlush_BypassesStickyPartialLineBufferingForReplayedLogs()
    {
        var directOutput = new StringWriter();
        var bufferedOutput = new Mock<IModuleOutputBuffer>();
        var consoleCoordinator = new Mock<IConsoleCoordinator>();
        consoleCoordinator.Setup(x => x.GetUnattributedBuffer()).Returns(bufferedOutput.Object);

        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? input, object? _) => input ?? string.Empty);

        var coordinatedWriter = new CoordinatedTextWriter(
            consoleCoordinator.Object,
            directOutput,
            () => true,
            secretObfuscator.Object,
            Mock.Of<ISecretProvider>());
        coordinatedWriter.Write("partial");
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(coordinatedWriter));
        var moduleBuffer = new ModuleOutputBuffer(typeof(OutputCoordinatorTests));
        moduleBuffer.AddLogEvent(new BufferedLogEvent<string>(
            LogLevel.Information,
            default,
            "replayed log",
            "replayed log",
            null,
            static (state, _) => state,
            secretObfuscator.Object));

        await coordinator.EnqueueAndFlushAsync(moduleBuffer, OutputFlushKind.Complete);
        await coordinatedWriter.FlushAsync();

        await Assert.That(directOutput.ToString()).Contains("replayed log");
        await Assert.That(directOutput.ToString()).DoesNotContain("partial");
        bufferedOutput.Verify(x => x.Write("partial"), Times.Once);
    }

    [Test]
    public async Task DeferredFlush_CancellationRequeuesCurrentAndRemainingOutputs()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var firstBuffer = new CancellingOutputBuffer(cancellationTokenSource);
        var secondBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        coordinator.SetProgressActive(true);
        await coordinator.OnModuleCompletedAsync(firstBuffer, firstBuffer.ModuleType);
        await coordinator.OnModuleCompletedAsync(secondBuffer, secondBuffer.ModuleType);
        coordinator.SetProgressActive(false);

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await coordinator.FlushDeferredAsync(cancellationTokenSource.Token));

        await coordinator.FlushDeferredAsync();

        await Assert.That(firstBuffer.FlushCount).IsEqualTo(2);
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task DeferredFlush_FailureRequeuesCurrentAndUnstartedOutputs()
    {
        var firstBuffer = new FailingOnceOutputBuffer();
        var secondBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        coordinator.SetProgressActive(true);
        await coordinator.OnModuleCompletedAsync(firstBuffer, firstBuffer.ModuleType);
        await coordinator.OnModuleCompletedAsync(secondBuffer, secondBuffer.ModuleType);
        coordinator.SetProgressActive(false);

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await coordinator.FlushDeferredAsync());

        await coordinator.FlushDeferredAsync();

        await Assert.That(firstBuffer.FlushCount).IsEqualTo(2);
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task DeferredFlush_ProviderCancellationRequeuesCurrentAndUnstartedOutputs()
    {
        var firstBuffer = new FailingOnceOutputBuffer(new OperationCanceledException("provider cancelled"));
        var secondBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        coordinator.SetProgressActive(true);
        await coordinator.OnModuleCompletedAsync(firstBuffer, firstBuffer.ModuleType);
        await coordinator.OnModuleCompletedAsync(secondBuffer, secondBuffer.ModuleType);
        coordinator.SetProgressActive(false);

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await coordinator.FlushDeferredAsync());

        await coordinator.FlushDeferredAsync();

        await Assert.That(firstBuffer.FlushCount).IsEqualTo(2);
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task Completion_Skips_Buffer_That_Has_Never_Produced_Output()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(x => x.ModuleType).Returns(typeof(OutputCoordinatorTests));
        buffer.SetupGet(x => x.NeedsCompletionFlush).Returns(false);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await coordinator.OnModuleCompletedAsync(buffer.Object, buffer.Object.ModuleType);
        await coordinator.FlushDeferredAsync();

        buffer.Verify(x => x.FlushToAsync(
            It.IsAny<TextWriter>(),
            It.IsAny<IBuildSystemFormatter>(),
            It.IsAny<ILogger>(),
            It.IsAny<ISpectreConsoleLoggerControl>(),
            It.IsAny<OutputFlushKind>(),
            It.IsAny<IReadOnlyList<ILogger>?>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task ImmediateFlush_PropagatesCancellationToQueueOwner()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var buffer = new CancellingOutputBuffer(cancellationTokenSource);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await coordinator.EnqueueAndFlushAsync(
                buffer,
                OutputFlushKind.Complete,
                cancellationTokenSource.Token));

        await Assert.That(buffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task IncrementalFlush_UsesInProgressRendering()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(x => x.ModuleType).Returns(typeof(OutputCoordinatorTests));
        buffer.SetupGet(x => x.HasOutput).Returns(true);
        buffer
            .Setup(x => x.FlushToAsync(
                It.IsAny<TextWriter>(),
                It.IsAny<IBuildSystemFormatter>(),
                It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(),
                OutputFlushKind.Incremental,
                It.IsAny<IReadOnlyList<ILogger>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await coordinator.EnqueueAndFlushAsync(buffer.Object, OutputFlushKind.Incremental);

        buffer.Verify(x => x.FlushToAsync(
            It.IsAny<TextWriter>(),
            It.IsAny<IBuildSystemFormatter>(),
            It.IsAny<ILogger>(),
            It.IsAny<ISpectreConsoleLoggerControl>(),
            OutputFlushKind.Incremental,
            It.IsAny<IReadOnlyList<ILogger>?>(),
            It.IsAny<CancellationToken>()), Times.Once);
        buffer.Verify(x => x.FlushToAsync(
            It.IsAny<TextWriter>(),
            It.IsAny<IBuildSystemFormatter>(),
            It.IsAny<ILogger>(),
            It.IsAny<ISpectreConsoleLoggerControl>(),
            OutputFlushKind.Complete,
            It.IsAny<IReadOnlyList<ILogger>?>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task IncrementalFlush_UsesNonGenericLoggerForUnattributedOutput()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(x => x.ModuleType).Returns(typeof(void));
        buffer.SetupGet(x => x.HasOutput).Returns(true);
        buffer
            .Setup(x => x.FlushToAsync(
                It.IsAny<TextWriter>(),
                It.IsAny<IBuildSystemFormatter>(),
                It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(),
                OutputFlushKind.Incremental,
                It.IsAny<IReadOnlyList<ILogger>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await coordinator.EnqueueAndFlushAsync(buffer.Object, OutputFlushKind.Incremental);

        buffer.Verify(x => x.FlushToAsync(
            It.IsAny<TextWriter>(),
            It.IsAny<IBuildSystemFormatter>(),
            It.IsAny<ILogger>(),
            It.IsAny<ISpectreConsoleLoggerControl>(),
            OutputFlushKind.Incremental,
            It.IsAny<IReadOnlyList<ILogger>?>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UnattributedFlush_UsesPipelineLoggerCategory()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(x => x.ModuleType).Returns(typeof(void));
        buffer.SetupGet(x => x.HasOutput).Returns(true);
        buffer
            .Setup(x => x.FlushToAsync(
                It.IsAny<TextWriter>(),
                It.IsAny<IBuildSystemFormatter>(),
                It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(),
                OutputFlushKind.Incremental,
                It.IsAny<IReadOnlyList<ILogger>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory
            .Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(Mock.Of<ILogger>());
        var nonSpectreLoggerFactory = new Mock<INonSpectreLoggerFactory>();
        nonSpectreLoggerFactory
            .Setup(x => x.CreateLoggers(It.IsAny<string>()))
            .Returns([]);
        var coordinator = CreateCoordinator(
            loggerFactory.Object,
            nonSpectreLoggerFactory: nonSpectreLoggerFactory.Object);

        await coordinator.EnqueueAndFlushAsync(buffer.Object, OutputFlushKind.Incremental);

        loggerFactory.Verify(
            x => x.CreateLogger(OutputLoggerCategories.Pipeline),
            Times.Once);
        nonSpectreLoggerFactory.Verify(
            x => x.CreateLoggers(OutputLoggerCategories.Pipeline),
            Times.Once);
    }

    [Test]
    public async Task Completion_RetriesStructuredProviderDeliveryOnce()
    {
        var buffer = new Mock<IModuleOutputBuffer>();
        buffer.SetupGet(x => x.ModuleType).Returns(typeof(OutputCoordinatorTests));
        buffer.SetupGet(x => x.NeedsCompletionFlush).Returns(true);
        buffer.SetupGet(x => x.HasStructuredDeliveryRetries).Returns(true);
        buffer
            .Setup(x => x.FlushToAsync(
                It.IsAny<TextWriter>(),
                It.IsAny<IBuildSystemFormatter>(),
                It.IsAny<ILogger>(),
                It.IsAny<ISpectreConsoleLoggerControl>(),
                OutputFlushKind.Complete,
                It.IsAny<IReadOnlyList<ILogger>?>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await coordinator.OnModuleCompletedAsync(buffer.Object, buffer.Object.ModuleType);

        buffer.Verify(x => x.FlushToAsync(
            It.IsAny<TextWriter>(),
            It.IsAny<IBuildSystemFormatter>(),
            It.IsAny<ILogger>(),
            It.IsAny<ISpectreConsoleLoggerControl>(),
            OutputFlushKind.Complete,
            It.IsAny<IReadOnlyList<ILogger>?>(),
            It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    [Test]
    public async Task Completion_IsQueuedWhileIncrementalFlushOwnsOutput()
    {
        var buffer = new BlockingIncrementalOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var incrementalFlush = coordinator.EnqueueAndFlushAsync(buffer, OutputFlushKind.Incremental);
        await buffer.IncrementalFlushStarted.Task.WaitAsync(TimeSpan.FromSeconds(1));

        buffer.MarkComplete();
        var completionFlush = coordinator.OnModuleCompletedAsync(buffer, buffer.ModuleType);

        buffer.ReleaseIncrementalFlush.TrySetResult();
        await incrementalFlush;
        await completionFlush;

        await Assert.That(buffer.CompleteFlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_DoesNotApplyOwnerCancellationToLaterBuffers()
    {
        using var ownerCancellation = new CancellationTokenSource();
        var firstBuffer = new BlockingOutputBuffer();
        var secondBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var ownerFlush = coordinator.EnqueueAndFlushAsync(
            firstBuffer,
            OutputFlushKind.Complete,
            ownerCancellation.Token);
        await firstBuffer.FlushStarted.Task;
        var secondFlush = coordinator.EnqueueAndFlushAsync(secondBuffer, OutputFlushKind.Complete);

        await ownerCancellation.CancelAsync();
        firstBuffer.ReleaseFlush.TrySetResult();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await ownerFlush);
        await secondFlush;
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_QueuedCallerObservesCancellationWhileWaiting()
    {
        using var queuedCancellation = new CancellationTokenSource();
        var firstBuffer = new BlockingOutputBuffer();
        var queuedBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var firstFlush = coordinator.EnqueueAndFlushAsync(firstBuffer, OutputFlushKind.Complete);
        await firstBuffer.FlushStarted.Task;
        var queuedFlush = coordinator.EnqueueAndFlushAsync(
            queuedBuffer,
            OutputFlushKind.Complete,
            queuedCancellation.Token);

        await queuedCancellation.CancelAsync();
        var completedTask = await Task.WhenAny(queuedFlush, Task.Delay(TimeSpan.FromSeconds(1)));

        firstBuffer.ReleaseFlush.TrySetResult();
        await firstFlush;

        await Assert.That(completedTask).IsSameReferenceAs(queuedFlush);
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await queuedFlush);
        await Assert.That(queuedBuffer.FlushCount).IsEqualTo(0);
    }

    [Test]
    public async Task WaitForPendingFlushes_WaitsForCanceledActiveFlushToReleaseBuffer()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var buffer = new BlockingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var flush = coordinator.EnqueueAndFlushAsync(
            buffer,
            OutputFlushKind.Incremental,
            cancellationTokenSource.Token);
        await buffer.FlushStarted.Task;

        await cancellationTokenSource.CancelAsync();
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await flush);

        var waitForPendingFlushes = coordinator.WaitForPendingFlushesAsync();
        var completedBeforeRelease = ReferenceEquals(
            await Task.WhenAny(waitForPendingFlushes, Task.Delay(TimeSpan.FromMilliseconds(50))),
            waitForPendingFlushes);

        buffer.ReleaseFlush.TrySetResult();
        await waitForPendingFlushes;

        await Assert.That(completedBeforeRelease).IsFalse();
    }

    [Test]
    public async Task ImmediateFlush_OwnerReturnsBeforeLaterBufferCompletes()
    {
        var firstBuffer = new BlockingOutputBuffer();
        var secondBuffer = new BlockingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var ownerFlush = coordinator.EnqueueAndFlushAsync(firstBuffer, OutputFlushKind.Complete);
        await firstBuffer.FlushStarted.Task;
        var secondFlush = coordinator.EnqueueAndFlushAsync(secondBuffer, OutputFlushKind.Complete);

        firstBuffer.ReleaseFlush.TrySetResult();
        await secondBuffer.FlushStarted.Task;

        var completedTask = await Task.WhenAny(ownerFlush, Task.Delay(TimeSpan.FromSeconds(1)));
        secondBuffer.ReleaseFlush.TrySetResult();
        await secondFlush;

        await Assert.That(completedTask).IsSameReferenceAs(ownerFlush);
    }

    [Test]
    public async Task ImmediateFlush_PausesProgressOnceForQueuedBatch()
    {
        var firstBuffer = new BlockingOutputBuffer();
        var secondBuffer = new CancellingOutputBuffer();
        var progressController = new Mock<IProgressController>();
        progressController.Setup(x => x.PauseAsync()).Returns(Task.CompletedTask);
        progressController.Setup(x => x.ResumeAsync()).Returns(Task.CompletedTask);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        coordinator.SetProgressController(progressController.Object);

        var firstFlush = coordinator.EnqueueAndFlushAsync(firstBuffer, OutputFlushKind.Complete);
        await firstBuffer.FlushStarted.Task;
        var secondFlush = coordinator.EnqueueAndFlushAsync(secondBuffer, OutputFlushKind.Complete);

        firstBuffer.ReleaseFlush.TrySetResult();
        await Task.WhenAll(firstFlush, secondFlush);
        await coordinator.WaitForPendingFlushesAsync();

        progressController.Verify(x => x.PauseAsync(), Times.Once);
        progressController.Verify(x => x.ResumeAsync(), Times.Once);
    }

    [Test]
    public async Task ImmediateFlush_ProcessorStartsOutsideCallersStack()
    {
        var buffer = new SynchronouslyBlockingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        var invocation = Task.Factory.StartNew(
            () => coordinator.EnqueueAndFlushAsync(buffer, OutputFlushKind.Complete),
            CancellationToken.None,
            TaskCreationOptions.DenyChildAttach,
            TaskScheduler.Default);

        await buffer.FlushStarted.Task.WaitAsync(TimeSpan.FromSeconds(1));
        var returnedBeforeRelease = false;
        try
        {
            returnedBeforeRelease = ReferenceEquals(
                await Task.WhenAny(invocation, Task.Delay(TimeSpan.FromSeconds(1))),
                invocation);
        }
        finally
        {
            buffer.ReleaseFlush.TrySetResult();
        }

        await await invocation;
        await Assert.That(returnedBeforeRelease).IsTrue();
    }

    [Test]
    public async Task ImmediateFlush_UnexpectedProcessorFailureFailsPendingRequestWithoutWedge()
    {
        var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
        formatterProvider.SetupSequence(x => x.GetFormatter())
            .Throws<InvalidOperationException>()
            .Returns(new DefaultFormatter());
        var coordinator = CreateCoordinator(
            new ConsoleWritingLoggerFactory(TextWriter.Null),
            formatterProvider.Object);
        var abandonedBuffer = new CancellingOutputBuffer();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await coordinator
                .EnqueueAndFlushAsync(abandonedBuffer, OutputFlushKind.Complete)
                .WaitAsync(TimeSpan.FromSeconds(1)));
        await coordinator
            .EnqueueAndFlushAsync(abandonedBuffer, OutputFlushKind.Complete)
            .WaitAsync(TimeSpan.FromSeconds(1));

        await Assert.That(abandonedBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_DoesNotRetryPartiallyDeliveredOutput()
    {
        var buffer = new PartiallyDeliveringOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await coordinator.EnqueueAndFlushAsync(buffer, OutputFlushKind.Complete));

        await Assert.That(buffer.DeliveryCount).IsEqualTo(1);
    }

    private static OutputCoordinator CreateCoordinator(
        ILoggerFactory loggerFactory,
        IBuildSystemFormatterProvider? formatterProvider = null,
        INonSpectreLoggerFactory? nonSpectreLoggerFactory = null)
    {
        if (formatterProvider is null)
        {
            var formatterProviderMock = new Mock<IBuildSystemFormatterProvider>();
            formatterProviderMock.Setup(x => x.GetFormatter()).Returns(new DefaultFormatter());
            formatterProvider = formatterProviderMock.Object;
        }

        var serviceProvider = new Mock<IServiceProvider>();
        var loggerControl = new Mock<ISpectreConsoleLoggerControl>();
        loggerControl.SetupGet(x => x.SynchronizationLock).Returns(new object());
        loggerControl
            .Setup(x => x.TryAcquireRenderGateAsync(
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<IDisposable?>(Mock.Of<IDisposable>()));
        if (nonSpectreLoggerFactory is null)
        {
            var nonSpectreLoggerFactoryMock = new Mock<INonSpectreLoggerFactory>();
            nonSpectreLoggerFactoryMock
                .Setup(factory => factory.CreateLoggers(It.IsAny<string>()))
                .Returns([]);
            nonSpectreLoggerFactory = nonSpectreLoggerFactoryMock.Object;
        }

        return new OutputCoordinator(
            formatterProvider,
            loggerFactory,
            serviceProvider.Object,
            loggerControl.Object,
            nonSpectreLoggerFactory);
    }

    private sealed class ConsoleWritingLoggerFactory(TextWriter writer) : ILoggerFactory
    {
        private readonly ILogger _logger = new ConsoleWritingLogger(writer);

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName) => _logger;

        public void Dispose()
        {
        }
    }

    private sealed class ConsoleWritingLogger(TextWriter writer) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            writer.WriteLine(formatter(state, exception));
        }
    }

    private sealed class CancellingOutputBuffer(CancellationTokenSource? cancellationTokenSource = null)
        : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(CancellingOutputBuffer);

        public int FlushCount { get; private set; }

        public bool HasOutput => true;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public void MarkComplete() => IsComplete = true;

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            FlushCount++;
            if (FlushCount == 1 && cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationToken.ThrowIfCancellationRequested();
            }

            return Task.CompletedTask;
        }
    }

    private sealed class FailingOnceOutputBuffer(Exception? exception = null) : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(FailingOnceOutputBuffer);

        public int FlushCount { get; private set; }

        public bool HasOutput => true;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public void MarkComplete() => IsComplete = true;

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            FlushCount++;
            if (FlushCount == 1)
            {
                throw exception ?? new InvalidOperationException("simulated deferred flush failure");
            }

            return Task.CompletedTask;
        }
    }

    private sealed class BlockingOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(BlockingOutputBuffer);

        public bool HasOutput => true;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public void MarkComplete() => IsComplete = true;

        public TaskCompletionSource FlushStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseFlush { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public async Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            FlushStarted.TrySetResult();
            await ReleaseFlush.Task;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private sealed class BlockingIncrementalOutputBuffer : IModuleOutputBuffer
    {
        private volatile bool _hasOutput = true;

        public Type ModuleType => typeof(BlockingIncrementalOutputBuffer);

        public bool HasOutput => _hasOutput;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public int CompleteFlushCount { get; private set; }

        public TaskCompletionSource IncrementalFlushStarted { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseIncrementalFlush { get; } =
            new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void MarkComplete() => IsComplete = true;

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            if (flushKind is OutputFlushKind.Complete)
            {
                CompleteFlushCount++;
                return Task.CompletedTask;
            }

            return FlushIncrementallyAsync(cancellationToken);
        }

        private async Task FlushIncrementallyAsync(CancellationToken cancellationToken)
        {
            _hasOutput = false;
            IncrementalFlushStarted.TrySetResult();
            await ReleaseIncrementalFlush.Task;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private sealed class SynchronouslyBlockingOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(SynchronouslyBlockingOutputBuffer);

        public bool HasOutput => true;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public void MarkComplete() => IsComplete = true;

        public TaskCompletionSource FlushStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseFlush { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            FlushStarted.TrySetResult();
            ReleaseFlush.Task.Wait(TimeSpan.FromSeconds(5), cancellationToken);
            return Task.CompletedTask;
        }
    }

    private sealed class PartiallyDeliveringOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(PartiallyDeliveringOutputBuffer);

        public bool HasOutput => true;

        public bool IsComplete { get; private set; }

        public bool NeedsCompletionFlush => true;

        public void MarkComplete() => IsComplete = true;

        public int DeliveryCount { get; private set; }

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
            if (command is not null)
            {
                WriteLine(command);
            }
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default)
        {
            DeliveryCount++;
            throw new InvalidOperationException("provider failed after partial delivery");
        }
    }
}
