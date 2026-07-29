using System.Runtime.ExceptionServices;
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

internal sealed class DeferredCommandOutputLogger : IDisposable
{
    private static readonly TimeSpan DefaultStreamingDelay = TimeSpan.FromMilliseconds(500);
    private readonly CommandExecutionOptions _executionOptions;
    private readonly Lock _lock = new();
    private readonly ICommandOutputLogger _outputLogger;
    private readonly List<BufferedLine> _pendingLines = [];
    private readonly TimeSpan _streamingDelay;
    private readonly CommandLineToolOptions _toolOptions;
    private DeferredCommandOutputCompletion? _completion;
    private ExceptionDispatchInfo? _loggingFailure;
    private Timer? _timer;
    private bool _isCompleted;

    internal DeferredCommandOutputLogger(
        ICommandOutputLogger outputLogger,
        CommandLineToolOptions toolOptions,
        CommandExecutionOptions executionOptions)
        : this(outputLogger, toolOptions, executionOptions, DefaultStreamingDelay)
    {
    }

    internal DeferredCommandOutputLogger(
        ICommandOutputLogger outputLogger,
        CommandLineToolOptions toolOptions,
        CommandExecutionOptions executionOptions,
        TimeSpan streamingDelay)
    {
        _outputLogger = outputLogger;
        _toolOptions = toolOptions;
        _executionOptions = executionOptions;
        _streamingDelay = streamingDelay;
    }

    public void LogStandardOutputLine(string line)
    {
        LogLine(new BufferedLine(line, IsError: false));
    }

    public void LogStandardErrorLine(string line)
    {
        LogLine(new BufferedLine(line, IsError: true));
    }

    public DeferredCommandOutputCompletion Complete()
    {
        lock (_lock)
        {
            if (_completion is not null)
            {
                ThrowLoggingFailure();
                return _completion;
            }

            _isCompleted = true;
            _timer?.Dispose();
            _timer = null;
            var pendingStandardOutput = HasStreamedOutput || _pendingLines.Count == 0
                ? string.Empty
                : _pendingLines[0].Text;
            _pendingLines.Clear();
            _completion = new DeferredCommandOutputCompletion(
                HasStreamedOutput,
                pendingStandardOutput);
            ThrowLoggingFailure();
            return _completion;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            _isCompleted = true;
            _timer?.Dispose();
            _timer = null;
            _pendingLines.Clear();
        }
    }

    private bool HasStreamedOutput { get; set; }

    private void LogLine(BufferedLine line)
    {
        lock (_lock)
        {
            if (_isCompleted || _loggingFailure is not null)
            {
                return;
            }

            if (HasStreamedOutput)
            {
                WriteLineSafely(line);
                return;
            }

            _pendingLines.Add(line);

            // Only a single short stdout line can join the compact command summary.
            // Stream anything else immediately and keep the deferred buffer bounded.
            if (line.IsError
                || line.Text.Length > CommandLogger.MaximumInlineOutputLength
                || _pendingLines.Count > 1)
            {
                FlushPendingLinesSafely();
                return;
            }

            _timer ??= new Timer(
                static state => ((DeferredCommandOutputLogger) state!).FlushPendingLines(),
                this,
                _streamingDelay,
                Timeout.InfiniteTimeSpan);
        }
    }

    private void FlushPendingLines()
    {
        lock (_lock)
        {
            if (_isCompleted || _loggingFailure is not null)
            {
                return;
            }

            FlushPendingLinesSafely();
        }
    }

    private void FlushPendingLinesSafely()
    {
        try
        {
            FlushPendingLinesUnderLock();
        }
        catch (Exception exception)
        {
            CaptureLoggingFailure(exception);
        }
    }

    private void FlushPendingLinesUnderLock()
    {
        HasStreamedOutput = true;
        foreach (var line in _pendingLines)
        {
            WriteLine(line);
        }

        _pendingLines.Clear();
        _timer?.Dispose();
        _timer = null;
    }

    private void WriteLineSafely(BufferedLine line)
    {
        try
        {
            WriteLine(line);
        }
        catch (Exception exception)
        {
            CaptureLoggingFailure(exception);
        }
    }

    private void CaptureLoggingFailure(Exception exception)
    {
        _loggingFailure ??= ExceptionDispatchInfo.Capture(exception);
        _timer?.Dispose();
        _timer = null;
    }

    private void WriteLine(BufferedLine line)
    {
        if (line.IsError)
        {
            _outputLogger.LogStandardErrorLine(_toolOptions, _executionOptions, line.Text);
        }
        else
        {
            _outputLogger.LogStandardOutputLine(_toolOptions, _executionOptions, line.Text);
        }
    }

    private void ThrowLoggingFailure()
    {
        var loggingFailure = _loggingFailure;
        _loggingFailure = null;
        loggingFailure?.Throw();
    }

    private readonly record struct BufferedLine(string Text, bool IsError);
}

internal sealed class DeferredCommandOutputCompletion(
    bool hasStreamedOutput,
    string pendingStandardOutput)
{
    public bool HasStreamedOutput { get; } = hasStreamedOutput;

    public string PendingStandardOutput { get; } = pendingStandardOutput;
}
