namespace ModularPipelines.Logging;

/// <summary>
/// Provides module-specific loggers for pipeline execution.
/// </summary>
public interface IModuleLoggerProvider
{
    /// <summary>
    /// Gets a logger for the current context.
    /// </summary>
    /// <returns>A module logger instance.</returns>
    IModuleLogger GetLogger();

    internal IModuleLogger GetLogger(Type type);
}
