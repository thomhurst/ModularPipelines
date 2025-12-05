namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log events for batch processing with thread-safe operations.
/// Used by ModuleLogger to collect log events during module execution and flush them
/// upon disposal for organized, grouped output.
/// </summary>
/// <example>
/// <code>
/// // Typically used by ModuleLogger internally
/// var buffer = new LogEventBuffer();
///
/// // Add log events during execution
/// buffer.Add(logEvent);
/// buffer.Add("Direct string message");
///
/// // Check if there are any events
/// if (buffer.HasEvents)
/// {
///     // Get all events and clear the buffer
///     var events = buffer.GetAndClear();
///     foreach (var evt in events)
///     {
///         // Process event...
///     }
/// }
/// </code>
/// </example>
internal class LogEventBuffer : ILogEventBuffer
{
    private List<StringOrLogEvent> _events = new();
    private readonly object _lock = new();

    public void Add(StringOrLogEvent logEvent)
    {
        lock (_lock)
        {
            _events.Add(logEvent);
        }
    }

    public IReadOnlyList<StringOrLogEvent> GetAndClear()
    {
        lock (_lock)
        {
            var events = _events;
            _events = new List<StringOrLogEvent>();
            return events;
        }
    }

    public bool HasEvents
    {
        get
        {
            lock (_lock)
            {
                return _events.Count > 0;
            }
        }
    }
}

/// <summary>
/// Interface for buffering log events.
/// </summary>
internal interface ILogEventBuffer
{
    /// <summary>
    /// Adds a log event to the buffer.
    /// </summary>
    void Add(StringOrLogEvent logEvent);

    /// <summary>
    /// Gets all buffered events and clears the buffer.
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<StringOrLogEvent> GetAndClear();

    /// <summary>
    /// Gets a value indicating whether gets whether the buffer contains any events.
    /// </summary>
    bool HasEvents { get; }
}

/// <summary>
/// Represents either a string or a log event.
/// </summary>
internal class StringOrLogEvent
{
    public (Microsoft.Extensions.Logging.LogLevel LogLevel, Microsoft.Extensions.Logging.EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter)? LogEvent
    {
        get;
        private init;
    }

    public string? StringValue { get; private init; }

    public bool IsString => StringValue != null;

    public static implicit operator StringOrLogEvent(string value) => new()
    {
        StringValue = value,
    };

    public static implicit operator StringOrLogEvent((Microsoft.Extensions.Logging.LogLevel LogLevel, Microsoft.Extensions.Logging.EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter) value) => new()
    {
        LogEvent = value,
    };
}