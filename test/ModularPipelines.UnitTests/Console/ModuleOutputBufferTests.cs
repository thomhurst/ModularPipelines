using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine.BuildSystemFormatters;

namespace ModularPipelines.UnitTests.Console;

public class ModuleOutputBufferTests
{
    [Test]
    public async Task Flush_Writes_Structured_Logs_Before_Group_End()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        var output = writer.ToString();
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(structuredLog).IsGreaterThanOrEqualTo(0);
        await Assert.That(groupEnd).IsGreaterThan(structuredLog);
    }

    [Test]
    public async Task Flush_Preserves_Structured_And_Direct_Output_Order()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("direct output");

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        var output = writer.ToString();
        var groupStart = output.IndexOf("::group::", StringComparison.Ordinal);
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var directOutput = output.IndexOf("direct output", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(groupStart).IsGreaterThanOrEqualTo(0);
        await Assert.That(structuredLog).IsGreaterThan(groupStart);
        await Assert.That(directOutput).IsGreaterThan(structuredLog);
        await Assert.That(groupEnd).IsGreaterThan(directOutput);
    }

    [Test]
    public async Task Flush_Renders_Markup_For_Direct_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("[green]direct output[/]");

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        var output = writer.ToString();
        await Assert.That(output).Contains("direct output");
        await Assert.That(output).DoesNotContain("[green]");
    }

    [Test]
    public async Task Flush_Keeps_Concurrent_Logs_Outside_Module_Group()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        Task? outsideLog = null;
        using var gateAttemptCompleted = new ManualResetEventSlim();
        loggerControl.AfterLog = () =>
        {
            outsideLog = Task.Run(() =>
            {
                if (!loggerControl.TryWrite("outside log", TimeSpan.FromMilliseconds(100)))
                {
                    gateAttemptCompleted.Set();
                    loggerControl.Write("outside log");
                }
                else
                {
                    gateAttemptCompleted.Set();
                }
            });
            gateAttemptCompleted.Wait();
        };

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);
        await outsideLog!;

        var output = writer.ToString();
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);
        var outside = output.IndexOf("outside log", StringComparison.Ordinal);

        await Assert.That(groupEnd).IsGreaterThanOrEqualTo(0);
        await Assert.That(outside).IsGreaterThan(groupEnd);
    }

    [Test]
    public async Task Flush_Observes_Cancellation()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                cancellationTokenSource.Token));

        await Assert.That(buffer.HasOutput).IsTrue();
    }

    [Test]
    public async Task Flush_Cancellation_Closes_Group_And_Retains_Unrendered_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("remaining output");
        using var cancellationTokenSource = new CancellationTokenSource();
        loggerControl.AfterLog = cancellationTokenSource.Cancel;

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                cancellationTokenSource.Token));

        var cancelledOutput = writer.ToString();
        await Assert.That(cancelledOutput.IndexOf("::endgroup::", StringComparison.Ordinal))
            .IsGreaterThan(cancelledOutput.IndexOf("structured log", StringComparison.Ordinal));
        await Assert.That(buffer.HasOutput).IsTrue();

        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        await Assert.That(writer.ToString()).Contains("remaining output");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_RenderingFailure_Retains_Unrendered_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            LogException = new InvalidOperationException("render failed"),
        };
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("remaining output");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl));

        await Assert.That(buffer.HasOutput).IsTrue();

        loggerControl.LogException = null;
        await buffer.FlushToAsync(writer, new GitHubActionsFormatter(), loggerControl, loggerControl);

        await Assert.That(writer.ToString()).Contains("structured log");
        await Assert.That(writer.ToString()).Contains("remaining output");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_CancellationInterruptsSynchronizationLockWait()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TimeSpan.FromSeconds(1));
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
        try
        {
            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await buffer.FlushToAsync(
                    writer,
                    new GitHubActionsFormatter(),
                    loggerControl,
                    loggerControl,
                    cancellationTokenSource.Token));
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        await Assert.That(buffer.HasOutput).IsTrue();
    }

    private static ModuleOutputBuffer CreateBufferWithStructuredLog()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.AddLogEvent(
            LogLevel.Information,
            default,
            "structured log",
            null,
            static (state, _) => state.ToString()!);
        return buffer;
    }

    private sealed class SynchronousLoggerControl(TextWriter writer) : ILogger, ISpectreConsoleLoggerControl
    {
        public object SynchronizationLock { get; } = new();

        public Action? AfterLog { get; set; }

        public Exception? LogException { get; set; }

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
            if (LogException is not null)
            {
                throw LogException;
            }

            Write(formatter(state, exception));
            AfterLog?.Invoke();
        }

        public void Write(string message)
        {
            lock (SynchronizationLock)
            {
                writer.WriteLine(message);
            }
        }

        public bool TryWrite(string message, TimeSpan timeout)
        {
            if (!Monitor.TryEnter(SynchronizationLock, timeout))
            {
                return false;
            }

            try
            {
                writer.WriteLine(message);
                return true;
            }
            finally
            {
                Monitor.Exit(SynchronizationLock);
            }
        }

        public Task FlushAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
