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
    public async Task ImmediateFlush_BypassesConsoleBufferingForReplayedLogs()
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
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(coordinatedWriter));
        var moduleBuffer = new ModuleOutputBuffer(typeof(OutputCoordinatorTests));
        moduleBuffer.AddLogEvent(
            LogLevel.Information,
            default,
            "replayed log",
            null,
            static (state, _) => state.ToString()!);

        await coordinator.EnqueueAndFlushAsync(moduleBuffer);

        await Assert.That(directOutput.ToString()).Contains("replayed log");
        bufferedOutput.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
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
    public async Task DeferredFlush_FailureOnlyRequeuesUnstartedOutputs()
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

        await Assert.That(firstBuffer.FlushCount).IsEqualTo(1);
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_PropagatesCancellationToQueueOwner()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var buffer = new CancellingOutputBuffer(cancellationTokenSource);
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await coordinator.EnqueueAndFlushAsync(buffer, cancellationTokenSource.Token));

        await Assert.That(buffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_DoesNotApplyOwnerCancellationToLaterBuffers()
    {
        using var ownerCancellation = new CancellationTokenSource();
        var firstBuffer = new BlockingOutputBuffer();
        var secondBuffer = new CancellingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var ownerFlush = coordinator.EnqueueAndFlushAsync(firstBuffer, ownerCancellation.Token);
        await firstBuffer.FlushStarted.Task;
        var secondFlush = coordinator.EnqueueAndFlushAsync(secondBuffer);

        await ownerCancellation.CancelAsync();
        firstBuffer.ReleaseFlush.TrySetResult();

        await Assert.ThrowsAsync<OperationCanceledException>(async () => await ownerFlush);
        await secondFlush;
        await Assert.That(secondBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_OwnerReturnsBeforeLaterBufferCompletes()
    {
        var firstBuffer = new BlockingOutputBuffer();
        var secondBuffer = new BlockingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        var ownerFlush = coordinator.EnqueueAndFlushAsync(firstBuffer);
        await firstBuffer.FlushStarted.Task;
        var secondFlush = coordinator.EnqueueAndFlushAsync(secondBuffer);

        firstBuffer.ReleaseFlush.TrySetResult();
        await secondBuffer.FlushStarted.Task;

        var completedTask = await Task.WhenAny(ownerFlush, Task.Delay(TimeSpan.FromSeconds(1)));
        secondBuffer.ReleaseFlush.TrySetResult();
        await secondFlush;

        await Assert.That(completedTask).IsSameReferenceAs(ownerFlush);
    }

    [Test]
    public async Task ImmediateFlush_ProcessorStartsOutsideCallersStack()
    {
        var buffer = new SynchronouslyBlockingOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));
        var invocation = Task.Factory.StartNew(
            () => coordinator.EnqueueAndFlushAsync(buffer),
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
            await coordinator.EnqueueAndFlushAsync(abandonedBuffer).WaitAsync(TimeSpan.FromSeconds(1)));
        await coordinator.EnqueueAndFlushAsync(abandonedBuffer).WaitAsync(TimeSpan.FromSeconds(1));

        await Assert.That(abandonedBuffer.FlushCount).IsEqualTo(1);
    }

    [Test]
    public async Task ImmediateFlush_DoesNotRetryPartiallyDeliveredOutput()
    {
        var buffer = new PartiallyDeliveringOutputBuffer();
        var coordinator = CreateCoordinator(new ConsoleWritingLoggerFactory(TextWriter.Null));

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await coordinator.EnqueueAndFlushAsync(buffer));

        await Assert.That(buffer.DeliveryCount).IsEqualTo(1);
    }

    private static OutputCoordinator CreateCoordinator(
        ILoggerFactory loggerFactory,
        IBuildSystemFormatterProvider? formatterProvider = null)
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

        return new OutputCoordinator(
            formatterProvider,
            loggerFactory,
            serviceProvider.Object,
            loggerControl.Object);
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

        public void WriteLine(string message)
        {
        }

        public void AddLogEvent(
            LogLevel level,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
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

    private sealed class FailingOnceOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(FailingOnceOutputBuffer);

        public int FlushCount { get; private set; }

        public bool HasOutput => true;

        public void WriteLine(string message)
        {
        }

        public void AddLogEvent(
            LogLevel level,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
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
            CancellationToken cancellationToken = default)
        {
            FlushCount++;
            if (FlushCount == 1)
            {
                throw new InvalidOperationException("simulated deferred flush failure");
            }

            return Task.CompletedTask;
        }
    }

    private sealed class BlockingOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(BlockingOutputBuffer);

        public bool HasOutput => true;

        public TaskCompletionSource FlushStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseFlush { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void WriteLine(string message)
        {
        }

        public void AddLogEvent(
            LogLevel level,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
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
            CancellationToken cancellationToken = default)
        {
            FlushStarted.TrySetResult();
            await ReleaseFlush.Task;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    private sealed class SynchronouslyBlockingOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(SynchronouslyBlockingOutputBuffer);

        public bool HasOutput => true;

        public TaskCompletionSource FlushStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource ReleaseFlush { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void WriteLine(string message)
        {
        }

        public void AddLogEvent(
            LogLevel level,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
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

        public int DeliveryCount { get; private set; }

        public void WriteLine(string message)
        {
        }

        public void AddLogEvent(
            LogLevel level,
            EventId eventId,
            object state,
            Exception? exception,
            Func<object, Exception?, string> formatter)
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
            CancellationToken cancellationToken = default)
        {
            DeliveryCount++;
            throw new InvalidOperationException("provider failed after partial delivery");
        }
    }
}
