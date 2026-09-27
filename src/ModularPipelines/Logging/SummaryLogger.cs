using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;

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
internal class SummaryLogger : IInternalSummaryLogger
{
    private readonly ILogger<SummaryLogger> _logger;
    private readonly INonSpectreLoggerFactory? _nonConsoleLoggerFactory;
    private readonly List<SummaryLogEntry> _entries = [];
    private readonly object _lock = new();
    private string? _cachedOutput;
    private int _displayedCount;

    public SummaryLogger(
        ILogger<SummaryLogger> logger,
        INonSpectreLoggerFactory? nonConsoleLoggerFactory = null)
    {
        _logger = logger;
        _nonConsoleLoggerFactory = nonConsoleLoggerFactory;
    }

    /// <inheritdoc />
    public void Information(string message)
        => AddEntry(SummaryLogLevel.Information, message, null);

    /// <inheritdoc />
    public void Information(string category, string message)
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public IReadOnlyList<SummaryLogEntry> GetEntriesForDisplay()
    {
        lock (_lock)
        {
            return _entries.ToList();
        }
    }

    /// <inheritdoc />
    public void MarkDisplayed(int count)
    {
        lock (_lock)
        {
            _displayedCount = Math.Clamp(count, _displayedCount, _entries.Count);
        }
    }

    /// <inheritdoc />
    public void WriteLogs()
    {
        List<SummaryLogEntry> displayedEntries;
        List<SummaryLogEntry> pendingEntries;
        lock (_lock)
        {
            displayedEntries = _entries.Take(_displayedCount).ToList();
            pendingEntries = _entries.Skip(_displayedCount).ToList();
        }

        // Entries rendered in the results output still reach file, telemetry, and build-system
        // providers; only the console, which already shows them, is skipped.
        if (displayedEntries.Count > 0 && _nonConsoleLoggerFactory is not null)
        {
            var nonConsoleLoggers = _nonConsoleLoggerFactory.CreateLoggers(typeof(SummaryLogger).FullName!);
            foreach (var entry in displayedEntries)
            {
                foreach (var logger in nonConsoleLoggers)
                {
                    Log(logger, entry);
                }
            }
        }

        foreach (var entry in pendingEntries)
        {
            Log(_logger, entry);
        }
    }

    private static void Log(ILogger logger, SummaryLogEntry entry)
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

        logger.Log(logLevel, "{Value}", message);
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
