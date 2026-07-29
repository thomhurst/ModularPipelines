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

    public bool Complete()
    {
        lock (_lock)
        {
            _isCompleted = true;
            _timer?.Dispose();
            _timer = null;
            _pendingLines.Clear();
            return HasStreamedOutput;
        }
    }

    public void Dispose()
    {
        Complete();
    }

    private bool HasStreamedOutput { get; set; }

    private void LogLine(BufferedLine line)
    {
        lock (_lock)
        {
            if (_isCompleted)
            {
                return;
            }

            if (HasStreamedOutput)
            {
                WriteLine(line);
                return;
            }

            _pendingLines.Add(line);

            // Only a single short stdout line can join the compact command summary.
            // Stream anything else immediately and keep the deferred buffer bounded.
            if (line.IsError
                || line.Text.Length > CommandLogger.MaximumInlineOutputLength
                || _pendingLines.Count > 1)
            {
                FlushPendingLinesUnderLock();
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
            if (_isCompleted)
            {
                return;
            }

            FlushPendingLinesUnderLock();
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

    private readonly record struct BufferedLine(string Text, bool IsError);
}
