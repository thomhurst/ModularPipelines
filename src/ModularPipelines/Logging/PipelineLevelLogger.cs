using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// A logger for pipeline-level operations that occur outside any module context.
/// </summary>
/// <remarks>
/// This logger is used when commands or other operations log output but there is no
/// module context (e.g., during initialization, condition evaluation before module context is set).
/// Unlike module loggers, this does not create a separate output section in the console.
/// Output goes directly to the underlying logger without module buffering.
/// </remarks>
internal sealed class PipelineLevelLogger : IModuleLogger
{
    private readonly ILogger _logger;

    public PipelineLevelLogger(ILogger logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _logger.Log(logLevel, eventId, state, exception, formatter);
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return _logger.BeginScope(state);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Nothing to dispose - the underlying logger is managed by the LoggerFactory
    }
}
