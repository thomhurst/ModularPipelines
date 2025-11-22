namespace ModularPipelines.Logging;

/// <summary>
/// Interface for managing the lifecycle of module loggers.
/// </summary>
internal interface IModuleLoggerContainer
{
    /// <summary>
    /// Flushes all buffered log events and disposes all registered loggers.
    /// Loggers are processed in order of last log written time.
    /// </summary>
    void FlushAndDisposeAll();

    /// <summary>
    /// Gets an existing logger for the specified type.
    /// </summary>
    /// <param name="type">The logger type to retrieve.</param>
    /// <returns>The logger instance, or null if not found.</returns>
    IModuleLogger? GetLogger(Type type);

    /// <summary>
    /// Adds a logger to the container registry.
    /// </summary>
    /// <param name="logger">The logger to add.</param>
    void AddLogger(ModuleLogger logger);
}
