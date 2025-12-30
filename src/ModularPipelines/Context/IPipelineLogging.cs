using ModularPipelines.Logging;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to logging functionality.
/// </summary>
/// <remarks>
/// This interface groups logging-related members for better Interface Segregation.
/// </remarks>
public interface IPipelineLogging
{
    /// <summary>
    /// Gets the logger to be used from the pipeline.
    /// </summary>
    /// <remarks>
    /// This logger is module-scoped and will include the module name in log output.
    /// </remarks>
    IModuleLogger Logger { get; }
}
