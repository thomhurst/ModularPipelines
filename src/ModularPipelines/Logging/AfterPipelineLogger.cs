using System.Text;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log messages to be written after pipeline completion.
/// Thread-safe implementation using lock synchronization.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread-safety:</b> This class is thread-safe for concurrent calls to
/// <see cref="LogOnPipelineEnd"/>. The <see cref="WriteLogs"/> method should
/// only be called once after all logging is complete (typically at pipeline end).
/// </para>
/// <para>
/// <b>Performance:</b> Uses append-only StringBuilder strategy - new values are
/// appended directly without rebuilding from scratch.
/// </para>
/// </remarks>
internal class AfterPipelineLogger : IAfterPipelineLogger
{
    private readonly ILogger<AfterPipelineLogger> _logger;
    private readonly StringBuilder _stringBuilder = new();
    private readonly List<string> _values = [];
    private readonly object _lock = new();

    public AfterPipelineLogger(ILogger<AfterPipelineLogger> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Adds a log message to be written after the pipeline completes.
    /// Thread-safe for concurrent calls.
    /// </summary>
    public void LogOnPipelineEnd(string value)
    {
        lock (_lock)
        {
            _values.Add(value);
            // Append directly to StringBuilder (append-only strategy)
            _stringBuilder.AppendLine(value);
        }
    }

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    public string GetOutput()
    {
        lock (_lock)
        {
            return _stringBuilder.ToString();
        }
    }

    /// <summary>
    /// Writes all buffered log messages to the logger.
    /// Should only be called once after all logging is complete.
    /// </summary>
    public void WriteLogs()
    {
        List<string> valuesCopy;
        lock (_lock)
        {
            valuesCopy = new List<string>(_values);
        }

        foreach (var value in valuesCopy)
        {
            _logger.LogInformation("{Value}", value);
        }
    }
}
