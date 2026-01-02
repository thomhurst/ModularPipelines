using System.Text;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log messages to be written after pipeline completion.
/// Thread-safe implementation using lock synchronization.
/// </summary>
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
    /// </summary>
    public void LogOnPipelineEnd(string value)
    {
        lock (_lock)
        {
            _values.Add(value);
            // Clear cached output so GetOutput() rebuilds it with the new value
            _stringBuilder.Clear();
        }
    }

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    public string GetOutput()
    {
        lock (_lock)
        {
            if (_stringBuilder.Length > 0)
            {
                return _stringBuilder.ToString();
            }

            // Build once and cache
            foreach (var value in _values)
            {
                _stringBuilder.AppendLine(value);
            }

            return _stringBuilder.ToString();
        }
    }

    /// <summary>
    /// Writes all buffered log messages to the logger.
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
