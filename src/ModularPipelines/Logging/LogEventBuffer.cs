namespace ModularPipelines.Logging;

/// <summary>
/// Buffers log events for batch processing.
/// </summary>
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
    IReadOnlyList<StringOrLogEvent> GetAndClear();

    /// <summary>
    /// Gets whether the buffer contains any events.
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