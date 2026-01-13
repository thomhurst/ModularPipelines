namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for module logger provider operations not exposed to consumers.
/// </summary>
internal interface IInternalModuleLoggerProvider : IModuleLoggerProvider
{
    /// <summary>
    /// Gets a logger for a specific module type.
    /// </summary>
    /// <param name="type">The module type.</param>
    /// <returns>A module logger instance for the specified type.</returns>
    IModuleLogger GetLogger(Type type);
}
