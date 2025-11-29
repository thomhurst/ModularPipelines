using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to enable using cached/historical results when a module is skipped.
/// </summary>
/// <remarks>
/// This works in conjunction with <see cref="ISkippable"/>:
/// <list type="bullet">
/// <item>If a module is skipped and history is available, the historical result is used</item>
/// <item>If no historical result exists, the module will be marked as skipped with no value</item>
/// </list>
/// Requires an <see cref="Engine.IModuleResultRepository"/> to be configured for the pipeline.
/// </remarks>
public interface IHistoryAware
{
    /// <summary>
    /// Determines whether to use a historical result if this module is skipped.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>True to use historical data if available; otherwise, false.</returns>
    Task<bool> UseResultFromHistoryIfSkipped(IPipelineContext context);
}
