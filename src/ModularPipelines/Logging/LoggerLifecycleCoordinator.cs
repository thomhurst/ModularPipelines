using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Coordinates the lifecycle of a module logger, handling disposal and flushing of buffered log events.
/// Manages the orchestration of: event buffer → collapsible sections → console/logger output.
/// </summary>
/// <remarks>
/// This class implements the disposal pattern for module loggers, ensuring that all buffered
/// log events are flushed to their final destinations when the logger is disposed.
/// It coordinates with the collapsible section manager to wrap output in build system-specific blocks.
/// </remarks>
/// <example>
/// <code>
/// var coordinator = new LoggerLifecycleCoordinator(
///     buffer,
///     sectionManager,
///     consoleWriter,
///     defaultLogger,
///     exceptionBuffer);
///
/// // When logger is disposed
/// coordinator.FlushAndDispose(moduleName, exception);
/// </code>
/// </example>
internal class LoggerLifecycleCoordinator : ILoggerLifecycleCoordinator
{
    private readonly ILogEventBuffer _logEventBuffer;
    private readonly ICollapsibleSectionManager _sectionManager;
    private readonly IConsoleWriter _consoleWriter;
    private readonly IExceptionBuffer _exceptionBuffer;

    public LoggerLifecycleCoordinator(
        ILogEventBuffer logEventBuffer,
        ICollapsibleSectionManager sectionManager,
        IConsoleWriter consoleWriter,
        IExceptionBuffer exceptionBuffer)
    {
        _logEventBuffer = logEventBuffer;
        _sectionManager = sectionManager;
        _consoleWriter = consoleWriter;
        _exceptionBuffer = exceptionBuffer;
    }

    public void FlushAndDispose(string moduleName, Exception? exception, ILogger defaultLogger)
    {
        if (!_logEventBuffer.HasEvents)
        {
            return;
        }

        var logEvents = _logEventBuffer.GetAndClear();
        var sectionName = _sectionManager.FormatSectionName(moduleName, exception);

        // Start collapsible section
        _sectionManager.StartSection(sectionName);

        // Flush all buffered events
        foreach (var stringOrLogEvent in logEvents)
        {
            if (stringOrLogEvent.IsString)
            {
                _consoleWriter.LogToConsole(stringOrLogEvent.StringValue ?? string.Empty);
            }
            else if (stringOrLogEvent.LogEvent != null)
            {
                LogEvent(stringOrLogEvent.LogEvent.Value, defaultLogger);
            }
        }

        // End collapsible section
        _sectionManager.EndSection(sectionName);
    }

    private void LogEvent((LogLevel LogLevel, EventId EventId, object State, Exception? Exception, Func<object, Exception?, string> Formatter) logEvent, ILogger defaultLogger)
    {
        var (logLevel, eventId, state, exception, formatter) = logEvent;

        try
        {
            defaultLogger.Log(logLevel, eventId, state, exception, formatter);
        }
        catch (Exception e)
        {
            _exceptionBuffer.AddExceptionMessage(e.ToString());
            _exceptionBuffer.AddExceptionMessage($"{logLevel}: {eventId} - {state}{exception}");
        }
    }
}

/// <summary>
/// Interface for coordinating logger lifecycle operations.
/// </summary>
internal interface ILoggerLifecycleCoordinator
{
    /// <summary>
    /// Flushes all buffered log events and performs disposal cleanup.
    /// </summary>
    /// <param name="moduleName">The name of the module being logged.</param>
    /// <param name="exception">The exception, if the module failed.</param>
    /// <param name="defaultLogger">The logger to use for flushing events.</param>
    void FlushAndDispose(string moduleName, Exception? exception, ILogger defaultLogger);
}
