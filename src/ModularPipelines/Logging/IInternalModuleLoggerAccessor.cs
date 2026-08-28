namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for module logger accessor operations not exposed to consumers.
/// </summary>
internal interface IInternalModuleLoggerAccessor : IModuleLoggerAccessor
{
    /// <summary>
    /// Gets the internal logger for the current context.
    /// </summary>
    IModuleLogger GetLogger();

    /// <summary>
    /// Gets a logger for a specific module type.
    /// </summary>
    /// <param name="type">The module type.</param>
    /// <returns>A module logger instance for the specified type.</returns>
    IModuleLogger GetLogger(Type type);
}
