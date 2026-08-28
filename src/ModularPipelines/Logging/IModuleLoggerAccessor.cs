using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides access to the logger for the current module execution context.
/// </summary>
public interface IModuleLoggerAccessor
{
    /// <summary>
    /// Gets the logger for the current module, or a pipeline-level logger outside a module.
    /// </summary>
    ILogger Logger { get; }
}
