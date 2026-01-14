using System.Text;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log messages to be written after pipeline completion.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. All public methods can be called
/// concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Synchronization Strategy:</b> Uses a simple lock for mutual exclusion. This is
/// appropriate because write operations are infrequent (typically once per module)
/// and the protected operations are fast (list/StringBuilder operations).
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class AfterPipelineLogger : IInternalAfterPipelineLogger
{
    private readonly ILogger<AfterPipelineLogger> _logger;
    private readonly List<SummaryLogEntry> _entries = [];
    private readonly object _lock = new();
    private string? _cachedOutput;

    public AfterPipelineLogger(ILogger<AfterPipelineLogger> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Info(string message)
        => AddEntry(SummaryLogLevel.Information, message, null);

    /// <inheritdoc />
    public void Info(string category, string message)
        => AddEntry(SummaryLogLevel.Information, message, category);

    /// <inheritdoc />
    public void Success(string message)
        => AddEntry(SummaryLogLevel.Success, message, null);

    /// <inheritdoc />
    public void Success(string category, string message)
        => AddEntry(SummaryLogLevel.Success, message, category);

    /// <inheritdoc />
    public void Warning(string message)
        => AddEntry(SummaryLogLevel.Warning, message, null);

    /// <inheritdoc />
    public void Warning(string category, string message)
        => AddEntry(SummaryLogLevel.Warning, message, category);

    /// <inheritdoc />
    public void Error(string message)
        => AddEntry(SummaryLogLevel.Error, message, null);

    /// <inheritdoc />
    public void Error(string category, string message)
        => AddEntry(SummaryLogLevel.Error, message, category);

    /// <inheritdoc />
    public void KeyValue(string key, string value)
        => AddEntry(SummaryLogLevel.Information, $"{key}: {value}", null);

    /// <inheritdoc />
    public void KeyValue(string category, string key, string value)
        => AddEntry(SummaryLogLevel.Information, $"{key}: {value}", category);

    /// <inheritdoc />
    public void Log(SummaryLogLevel level, string message)
        => AddEntry(level, message, null);

    /// <inheritdoc />
    public void Log(SummaryLogLevel level, string category, string message)
        => AddEntry(level, message, category);

    /// <inheritdoc />
    public IReadOnlyList<SummaryLogEntry> GetEntries()
    {
        lock (_lock)
        {
            return _entries.ToList();
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<SummaryLogEntry> GetEntries(string category)
    {
        lock (_lock)
        {
            return _entries.Where(e => e.Category == category).ToList();
        }
    }

    /// <summary>
    /// Adds a log message to be written after the pipeline completes.
    /// </summary>
#pragma warning disable CS0618 // Type or member is obsolete
    public void LogOnPipelineEnd(string value)
#pragma warning restore CS0618
    {
        // Treat legacy calls as information level with no category
        AddEntry(SummaryLogLevel.Information, value, null);
    }

    /// <summary>
    /// Gets all buffered log messages as a single string.
    /// </summary>
    public string GetOutput()
    {
        lock (_lock)
        {
            if (_cachedOutput != null)
            {
                return _cachedOutput;
            }

            var sb = new StringBuilder();
            string? currentCategory = null;

            // Group entries by category for better organization
            var groupedEntries = _entries
                .GroupBy(e => e.Category ?? string.Empty)
                .OrderBy(g => string.IsNullOrEmpty(g.Key) ? 1 : 0) // Categorized first
                .ThenBy(g => g.Key);

            foreach (var group in groupedEntries)
            {
                if (!string.IsNullOrEmpty(group.Key) && group.Key != currentCategory)
                {
                    if (currentCategory != null)
                    {
                        sb.AppendLine();
                    }
                    sb.AppendLine($"[{group.Key}]");
                    currentCategory = group.Key;
                }

                foreach (var entry in group)
                {
                    var prefix = entry.Level switch
                    {
                        SummaryLogLevel.Success => "[OK] ",
                        SummaryLogLevel.Warning => "[WARN] ",
                        SummaryLogLevel.Error => "[ERR] ",
                        _ => string.Empty,
                    };
                    sb.AppendLine($"{prefix}{entry.Message}");
                }
            }

            _cachedOutput = sb.ToString();
            return _cachedOutput;
        }
    }

    /// <summary>
    /// Writes all buffered log messages to the logger.
    /// </summary>
    public void WriteLogs()
    {
        List<SummaryLogEntry> entriesCopy;
        lock (_lock)
        {
            entriesCopy = new List<SummaryLogEntry>(_entries);
        }

        foreach (var entry in entriesCopy)
        {
            var logLevel = entry.Level switch
            {
                SummaryLogLevel.Error => LogLevel.Error,
                SummaryLogLevel.Warning => LogLevel.Warning,
                SummaryLogLevel.Success => LogLevel.Information,
                _ => LogLevel.Information,
            };

            var message = entry.Category != null
                ? $"[{entry.Category}] {entry.Message}"
                : entry.Message;

            _logger.Log(logLevel, "{Value}", message);
        }
    }

    private void AddEntry(SummaryLogLevel level, string message, string? category)
    {
        lock (_lock)
        {
            _entries.Add(SummaryLogEntry.Create(level, message, category));
            _cachedOutput = null; // Invalidate cache
        }
    }
}
