using System.Text;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log messages to be written after pipeline completion.
/// </summary>
internal class AfterPipelineLogger : IAfterPipelineLogger
{
    private readonly ILogger<AfterPipelineLogger> _logger;
    private readonly StringBuilder _stringBuilder = new();
    private readonly List<string> _values = [];

    public AfterPipelineLogger(ILogger<AfterPipelineLogger> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Adds a log message to be written after the pipeline completes.
    /// </summary>
    public void LogOnPipelineEnd(string value)
    {
        _values.Add(value);
    }

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    public string GetOutput()
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

    /// <summary>
    /// Writes all buffered log messages to the logger.
    /// </summary>
    public void WriteLogs()
    {
        foreach (var value in _values)
        {
            _logger.LogInformation("{Value}", value);
        }
    }
}
