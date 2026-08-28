using ModularPipelines.Enums;

namespace ModularPipelines.Logging;

/// <summary>
/// Internal interface for module logger operations not exposed to consumers.
/// </summary>
internal interface IInternalModuleLogger : IModuleLogger
{
    /// <summary>
    /// Sets the exception that occurred during module execution.
    /// </summary>
    void SetException(Exception exception);

    /// <summary>
    /// Sets the final module status used when rendering buffered output.
    /// </summary>
    void SetStatus(ModuleStatus status);
}
