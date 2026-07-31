using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine;

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
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IFormattedLogValuesObfuscator _formattedLogValuesObfuscator;

    public PipelineLevelLogger(
        ILogger logger,
        ISecretObfuscator secretObfuscator,
        IFormattedLogValuesObfuscator formattedLogValuesObfuscator)
    {
        _logger = logger;
        _secretObfuscator = secretObfuscator;
        _formattedLogValuesObfuscator = formattedLogValuesObfuscator;
    }

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var obfuscatedState = state is null
            ? null
            : _formattedLogValuesObfuscator.TryObfuscateValues(state);
        new BufferedLogEvent<TState>(
                logLevel,
                eventId,
                state,
                obfuscatedState,
                exception,
                formatter,
                _secretObfuscator)
            .WriteTo(_logger);
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
        var obfuscatedState = _formattedLogValuesObfuscator.TryObfuscateValues(state);
        return obfuscatedState is TState typedState
            ? _logger.BeginScope(typedState)
            : _logger.BeginScope(obfuscatedState);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Nothing to dispose - the underlying logger is managed by the LoggerFactory
    }
}
